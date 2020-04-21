using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public interface IQuerySetupBuilder
    {
        IFacetsGraph Graph { get; set; }

        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet);
        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables);
        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables, List<string> facetCodes);
    }

    public interface IQuerySetupBuilderArgs
    {
        IFacetsGraph Graph { get; }
        IPickFilterCompilerLocator PickCompilers { get; }
        IJoinSqlCompiler JoinCompiler { get; }
    }

    public class QuerySetupBuilder : IQuerySetupBuilder {
        public IFacetsGraph Graph { get; set; }
        public IPickFilterCompilerLocator PickCompilers { get; set; }
        public IJoinSqlCompiler JoinCompiler { get; }

        public QuerySetupBuilder(IFacetsGraph graph, IPickFilterCompilerLocator pickCompilers, IJoinSqlCompiler joinCompiler) {
            Graph = graph;
            PickCompilers = pickCompilers;
            JoinCompiler = joinCompiler;
        }
        public QuerySetupBuilder(IQuerySetupBuilderArgs args) : this(args.Graph, args.PickCompilers, args.JoinCompiler)
        {
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet)
        {
            return Build(facetsConfig, facet, null);
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables)
        {
            return Build(facetsConfig, facet, extraTables, null);
        }
        /*
          Function: get_query_clauses
          This the core of the dynamic query builder.     It's input are previous seleceted filter and the code of the facet that triggered the action.
          It is also using a array of filter to filter by text each facet result,

          Parameters:
         * $paramss all facet_params, selections, text_filter, positions of facets ie the view.state of the client)
         * the target facet to which the query should populate/compute counts etc
         * $data_tables, any extra tables that should be part of the query, the function uses the tables via get_joins to join the tables
         * $f_list, the list of the facets in the view-state

          Logics:
         *  Get all selection preceding the target facet. 
         *  Make query-where statements depending on which type of facets (range or discrete)

          Exceptions:
         * a - for target facets (f_code) of "discrete" type it should be affected by all selection from facets preceeding the requested/target facets.
         * b - for "range" facet it should also include the condition of the range-facets itself, although the bound should be expanded to show values outside the limiting range.


          Returns:
          select clauses (not used)
          where clauses
          join conditions
          tables to used


 */
        /// <summary>
        /// This the core of the query builder that builds SQL query components for a given target facet, and the
        /// chain of facet configurations preceeding the target facet.
        /// 
        /// Rules:
        ///   - Collect/compile users pick constraint for all involved facets current facet
        ///      - If target facet is "discrete" then all picks *predeeding* target facet should be included - not the target facet's own constraints!
        ///      - for "range" facet it should also include the condition of the range-facets itself, although the bound should be expanded to show values outside the limiting range.
        ///      
        ///   - Get all selection preceding the target facet.
        ///   - Make where-clauses depending on  type of facets (range or discrete)
        ///   
        /// </summary>
        /// <param name="facetsConfig">The (complete) facet configuration, including user picks</param>
        /// <param name="targetFacet">Target facet that the query populates/computes/counts etc</param>
        /// <param name="extraTables">,Any extra tables that should be part of the query (path to these tables are included in final query)</param>
        /// <param name="facetCodes">FIXME: "The list of the facets in the view-state"</param>
        /// <returns>
        /// 
        /// QuerySetup 
        /// {
        ///     TargetConfig    Reference to targets facet's configuration              (only added for convenience)
        ///     Facet           Reference to target facet                               (only added for convenience)
        ///     Routes          Compiled routes                                         (added for easier testing purposes)
        ///     Joins           Join clauses to be used in final SQL
        ///     Criterias       Criteria clauses to be included in final SQL
        /// }
        /// 
        /// 
        /// </returns>
        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> extraTables, List<string> facetCodes)
        {
            // Noteworthy: TargetFacet differs from facetsConfig.TargetFacet when a result-facet is applied

            facetCodes ??= facetsConfig.GetFacetCodes();

            facetCodes = facetCodes.AddIfMissing(targetFacet.FacetCode);

            var involvedConfigs = facetsConfig.GetFacetConfigsAffectedBy(targetFacet, facetCodes);
            if (!String.IsNullOrEmpty(facetsConfig.DomainCode)) {
                var domainConfig = FacetConfigFactory.CreateSimple(facetsConfig.DomainFacet, 0);
                involvedConfigs.Insert(0, domainConfig);
            }
            var pickCriterias = CompilePickCriterias(targetFacet, involvedConfigs);
            var facetCriterias = CompileFacetCriterias(targetFacet, involvedConfigs);

            var routes = CompileRoutes(targetFacet, extraTables, involvedConfigs);
            var joins = CompileJoins(routes, facetsConfig);

            QuerySetup querySetup = new QuerySetup()
            {
                TargetConfig = facetsConfig?.TargetConfig,
                Facet = targetFacet,
                Routes = routes,
                Joins = joins,
                Criterias = pickCriterias.Concat(facetCriterias).ToList()
            };

            return querySetup;
        }

        private List<GraphRoute> CompileRoutes(Facet targetFacet, List<string> extraTables, List<FacetConfig2> involvedConfigs)
        {
            var tables = GetInvolvedTables(targetFacet, extraTables, involvedConfigs);
            var routes = Graph.Find(targetFacet.TargetTable.ResolvedAliasOrTableOrUdfName, tables, true);
            return routes;
        }

        protected List<string> CompileJoins(List<GraphRoute> routes, FacetsConfig2 facetsConfig)
        {
            var joins = routes
                .SelectMany(route => route.Items)
                    .OrderByDescending(z => z.TargetTable.IsUdf)
                        .Select(edge => CompileJoin(facetsConfig, edge))
                            .ToList();
            return joins;
        }

        /// <summary>
        /// Returns FacetTable instance for given relation
        /// Note: previoulsy, HasUserPicks(edge, pickCriterias) was used as join type argument
        /// </summary>
        /// <param name="facetsConfig"></param>
        /// <param name="edge"></param>
        /// <returns></returns>
        private string CompileJoin(FacetsConfig2 facetsConfig, TableRelation edge)
        {
            var facetTable = facetsConfig.GetFacetTable(edge.TargetName)
                 ?? Graph.Aliases.FindByAlias(edge.TargetName);
            var join = JoinCompiler.Compile(edge, facetTable, true);
            return join;
        }

        /// <summary>
        /// Returns Facet.QueryCriteria for all involved facets, target facet included
        /// </summary>
        /// <param name="targetFacet"></param>
        /// <param name="involvedConfigs"></param>
        /// <returns></returns>
        private IEnumerable<string> CompileFacetCriterias(Facet targetFacet, List<FacetConfig2> involvedConfigs)
        {
            var criterias = involvedConfigs
                .Select(c => c.Facet)
                    .Union(new List<Facet> { targetFacet })    // target facet is not always in affected list (ReloadIfTarget)
                        .Where(z => !z.QueryCriteria.IsEmpty())
                            .Select(z => z.QueryCriteria);
            return criterias;
        }


        /// <summary>
        /// Returns where-clauses based on user picks for all involved facets
        /// </summary>
        /// <param name="targetFacet"></param>
        /// <param name="involvedConfigs"></param>
        /// <returns></returns>
        protected IEnumerable<string> CompilePickCriterias(Facet targetFacet, List<FacetConfig2> involvedConfigs)
        {
            var criterias = involvedConfigs
                .Select(
                    config => PickCompiler(config).Compile(targetFacet, config.Facet, config)
                 )
                .Where(
                    x => x.IsNotEmpty()
                );
            return criterias;
        }

        /// <summary>
        /// Collect all tables specified ib affected facets and target facet
        /// </summary>
        /// <param name="targetFacet"></param>
        /// <param name="extraTables"></param>
        /// <param name="involvedConfigs"></param>
        /// <returns></returns>
        protected List<string> GetInvolvedTables(Facet targetFacet, List<string> extraTables, List<FacetConfig2> involvedConfigs)
        {
            var facets = involvedConfigs.Select(c => c.Facet).Append(targetFacet);
            var tables = facets
                .SelectMany(
                    f => f.Tables.Select(z => z.ResolvedAliasOrTableOrUdfName)
                 )
                .Concat(
                    extraTables ?? new List<string>()
                ).Distinct().ToList();
            return tables;
        }

        protected IPickFilterCompiler PickCompiler(FacetConfig2 c)
        {
            return PickCompilers.Locate(c.Facet.FacetTypeId);
        }
    }
}