using SeadQueryCore.Model.Ext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class QuerySetupBuilder : IQuerySetupBuilder {

        public IFacetsGraph FacetsGraph { get; set; }
        public IPicksFilterCompiler PicksCompiler { get; set; }
        public IJoinsClauseCompiler JoinCompiler { get; }

        public QuerySetupBuilder(IFacetsGraph graph, IPicksFilterCompiler picksCompiler, IJoinsClauseCompiler joinCompiler)
        {
            FacetsGraph = graph;
            PicksCompiler = picksCompiler;
            JoinCompiler = joinCompiler;
        }

        /// <summary>
        /// This the core of the query builder that builds SQL query components for a given target facet, and the
        /// chain of facet configurations preceeding the target facet.
        ///
        /// Rules:
        ///   - Collect/compile users pick constraint for all involved facets current facet
        ///      - All picks *predeeding* target facet should be included
        ///      - If target is a "discrete" facet then the facet's own constraints should NOT be included.
        ///      - If target facet is "range" then the facet's own constraints should be included,
        ///          but the bound should be expanded to the facet's entire range.
        ///
        ///   - Get all selection preceding the target facet.
        ///   - Make where-clauses depending on  type of facets (range or discrete)
        ///
        /// OLD COMMENTS: Function: get_query_clauses
        /// This the core of the dynamic query builder.It's input are previous seleceted filter and the code of the facet that triggered the action.
        ///  It is also using a array of filter to filter by text each facet result,
        ///  Parameters:
        /// * $paramss all facet_params, selections, text_filter, positions of facets ie the view.state of the client)
        /// * the target facet to which the query should populate/compute counts etc
        /// * $data_tables, any extra tables that should be part of the query, the function uses the tables via get_joins to join the tables
        /// * $f_list, the list of the facets in the view-state
        ///  Logics:
        /// *  Get all selection preceding the target facet.
        /// *  Make query-where statements depending on which type of facets (range or discrete)
        ///  Exceptions:
        /// * a - for target facets(f_code) of "discrete" type it should be affected by all selection from facets preceeding the requested/target facets.
        /// * b - for "range" facet it should also include the condition of the range-facets itself, although the bound should be expanded to show values outside the limiting range.
        /// </summary>
        /// <param name="facetsConfig">The (complete) facet configuration, including user picks</param>
        /// <param name="targetFacet">Target facet that the query populates/computes/counts etc</param>
        /// <param name="extraTables">,Any extra tables that should be part of the query (path to these tables are included in final query)</param>
        /// <param name="facetCodes">FIXME: "The list of the facets in the view-state"</param>
        /// <returns>
        /// QuerySetup:
        ///     TargetConfig    Reference to targets facet's configuration              (only added for convenience)
        ///     Facet           Reference to target facet                               (only added for convenience)
        ///     Routes          Compiled routes                                         (added for easier testing purposes)
        ///     Joins           Join clauses to be used in final SQL
        ///     Criterias       Criteria clauses to be included in final SQL
        /// </returns>
        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> extraTables, List<string> facetCodes)
        {
            // Noteworthy: TargetFacet differs from facetsConfig.TargetFacet when a result-facet is applied

            extraTables ??= new List<string>();

            var involvedConfigs = facetsConfig.GetFacetConfigsAffectedBy(targetFacet, facetCodes);

            if (facetsConfig.HasDomainCode())
                involvedConfigs.Insert(0, facetsConfig.CreateDomainConfig());

            var involvedFacets = involvedConfigs.Facets().AddUnion(targetFacet).ToList();

            //System.Diagnostics.Debug.Assert(involvedFacets.Contains(facetsConfig.TargetFacet));

            var pickCriterias = PicksCompiler.Compile(targetFacet, involvedConfigs);
            var facetCriterias = involvedFacets.Criterias();

            var involvedJoins = JoinCompiler.Compile(facetsConfig, targetFacet, involvedFacets, extraTables);

            QuerySetup querySetup = new QuerySetup()
            {
                TargetConfig = facetsConfig?.TargetConfig,
                Facet = targetFacet,
                Joins = involvedJoins,
                Criterias = pickCriterias.Concat(facetCriterias).ToList()
            };

            return querySetup;
        }
 
        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet resultFacet, IEnumerable<ResultAggregateField> resultFields)
        {
            if (!resultFields?.Any() ?? false)
                throw new System.ArgumentNullException($"ResultConfig is null or is missing aggregate keys!");
            
            /* All tables referenced by the result fields must be included in query */
            return Build(
                facetsConfig: facetsConfig,
                targetFacet:  resultFacet,
                extraTables:  resultFields.GetResultFieldTableNames().ToList(),
                facetCodes:   null
            );
        }
    }

    public interface IJoinsClauseCompiler
    {
        List<string> Compile(FacetsConfig2 facetsConfig, Facet targetFacet, List<Facet> involvedFacets, List<string> extraTables);
    }

    public class JoinsClauseCompiler: IJoinsClauseCompiler
    {
        public JoinsClauseCompiler(IFacetsGraph graph, IJoinSqlCompiler joinCompiler)
        {
            FacetsGraph = graph;
            JoinCompiler = joinCompiler;
        }

        public IJoinSqlCompiler JoinCompiler { get; }
        public IFacetsGraph FacetsGraph { get; set; }

        private FacetTable GetFacetTableByNameOrAlias(FacetsConfig2 facetsConfig, TableRelation edge)
            => facetsConfig.GetFacetTable(edge.TargetName) ?? FacetsGraph.GetAliasedFacetTable(edge.TargetName);

        public virtual List<string> Compile(
            FacetsConfig2 facetsConfig,
            Facet targetFacet,
            List<Facet> involvedFacets,
            List<string> extraTables
        )
        {
            var involvedTables = involvedFacets.TableNames().Union(extraTables).Distinct().ToList();
            var routes = FacetsGraph.Find(targetFacet.TargetTable.ResolvedAliasOrTableOrUdfName, involvedTables, true);
            var joins = routes.Edges().Select(
                    edge => JoinCompiler.Compile(edge, GetFacetTableByNameOrAlias(facetsConfig, edge), true)
                ).ToList();
            return joins;
        }
    }
}