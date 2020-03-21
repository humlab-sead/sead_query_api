using Autofac.Features.Indexed;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public interface IQuerySetupCompiler
    {
        IFacetsGraph Graph { get; set; }

        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables = null);
        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables, List<string> facetCodes);
    }

    public interface IQuerySetupCompilerArgs
    {
        IFacetsGraph Graph { get; }
        IIndex<int, IPickFilterCompiler> PickCompilers { get; }
        IEdgeSqlCompiler EdgeCompiler { get; }

    }
    public class QuerySetupCompiler : IQuerySetupCompiler {
        public IFacetsGraph Graph { get; set; }
        public IIndex<int, IPickFilterCompiler> PickCompilers { get; set; }
        public IEdgeSqlCompiler EdgeCompiler { get; }

        public QuerySetupCompiler(
            IFacetsGraph graph,
            IIndex<int, IPickFilterCompiler> pickCompilers,
            IEdgeSqlCompiler edgeCompiler
        ) {
            Graph = graph;
            PickCompilers = pickCompilers;
            EdgeCompiler = edgeCompiler;
        }
        public QuerySetupCompiler(IQuerySetupCompilerArgs args) : this(args.Graph, args.PickCompilers, args.EdgeCompiler)
        {
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables = null)
        {
            List<string> facetCodes = facetsConfig.GetFacetCodes().AddIfMissing(facet.FacetCode);
            return Build(facetsConfig, facet, extraTables ?? new List<string>(), facetCodes);
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> extraTables, List<string> facetCodes)
        {
            // Noteworthy: TargetFacet differs from facetsConfig.TargetFacet when a result-facet is applied

            Debug.Assert(facetsConfig.TargetFacet != null, "facetsConfig.TargetFacet is NULL ");

            facetCodes ??= new List<string>();

            var involvedConfigs = facetsConfig.GetFacetConfigsAffectedBy(targetFacet, facetCodes);

            // Find all tables that are involved in the final query
            List<string> tables = GetInvolvedTables(targetFacet, extraTables, involvedConfigs);

            // Compute criteria clauses for user picks and store in Dictionary keyed by tablename
            var tableCriterias = CompilePickCriterias(targetFacet, involvedConfigs);

            // Find all routes from target facet's table to all tables collected in affected facets
            List<GraphRoute> routes = Graph.Find(targetFacet.TargetTable.ResolvedAliasOrTableOrUdfName, tables, true);

            // Compile list of joins for the reduced route
            List<string> joins = CompileJoins(tableCriterias, routes, facetsConfig);

            // Add TargetFacets Query Criteria (if exists)
            //Debug.Assert(false,"False all affected facets' critera must be added!!!!");

            // TODO change QueryCriteria to return List<string>, rename to Clauses
            var criterias = tableCriterias.Values
                .AppendIf(targetFacet.QueryCriteria).ToList();

            QuerySetup querySetup = new QuerySetup() {
                TargetConfig = facetsConfig?.TargetConfig,
                Facet = targetFacet,
                Routes = routes,
                Joins = joins,
                Criterias = criterias
            };

            return querySetup;
        }

        private FacetTable GetFacetTable(FacetsConfig2 facetsConfig, TableRelation edge)
        {
            return facetsConfig.GetFacetTable(edge.TargetName);
        }

        protected List<string> CompileJoins(Dictionary<string, string> pickCriterias, List<GraphRoute> routes, FacetsConfig2 facetsConfig)
        {
            var aliasTables = Graph.AliasTables;
            return routes
                .SelectMany(route => route.Items)
                .OrderByDescending(z => z.TargetTable.IsUdf)
                .Select(edge => EdgeCompiler.Compile(
                    Graph,
                    edge,
                    GetFacetTable(facetsConfig, edge),
                    true /* HasUserPicks(edge, pickCriterias) */
                ))
                .ToList();
        }

        protected Dictionary<string, string> CompilePickCriterias(Facet targetFacet, List<FacetConfig2> involvedConfigs)
        {
            // Compute criteria clauses for user picks for each affected facet
            var criterias = involvedConfigs
                .Select(config => (
                    (
                        Tablename: config.Facet.TargetTable.ResolvedAliasOrTableOrUdfName,
                        Criteria: PickCompiler(config).Compile(targetFacet, config.Facet, config))
                    )
                 )
                .ToList();

            // Group and concatenate the criterias for each table
            var pickCriterias = criterias
                .GroupBy(p => p.Tablename, p => p.Criteria, (key, g) => new { TableName = key, Criterias = g.ToList() })
                .ToDictionary(z => z.TableName, z => $"({z.Criterias.Combine(" AND ")})");

            return pickCriterias;
        }

        /// <summary>
        /// Collect all affected tables (those defined in affected facets)
        /// </summary>
        /// <param name="targetFacet"></param>
        /// <param name="extraTables"></param>
        /// <param name="involvedConfigs"></param>
        /// <returns></returns>
        protected List<string> GetInvolvedTables(Facet targetFacet, List<string> extraTables, List<FacetConfig2> involvedConfigs)
        {
            var tables =

                // ...extra tables (if any)...
                (extraTables ?? new List<string>())

                // ...target facet's tables...
                .Concat(
                    targetFacet.Tables.Select(z => z.ResolvedAliasOrTableOrUdfName)
                )

                // ...tables from affected facets...
                .Concat(
                    involvedConfigs.SelectMany(c => c.Facet.Tables.Select(z => z.ResolvedAliasOrTableOrUdfName).ToList())
                );

            return tables.Distinct().ToList();
        }

        protected bool HasUserPicks(TableRelation edge, Dictionary<string, string> tableCriterias)
        {
            // FIXME: SHould SourceName really be considered here...?
            return /* tableCriterias.ContainsKey(edge.SourceName) || */ tableCriterias.ContainsKey(edge.TargetName);
        }

        protected IPickFilterCompiler PickCompiler(FacetConfig2 c)
        {
            return PickCompilers[(int)c.Facet.FacetTypeId];
        }
    }
}