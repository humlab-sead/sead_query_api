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

        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables = null)
        {
            List<string> facetCodes = facetsConfig.GetFacetCodes().AddIfMissing(facet.FacetCode);
            return Build(facetsConfig, facet, extraTables ?? new List<string>(), facetCodes);
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> extraTables, List<string> facetCodes)
        {
            // Noteworthy: TargetFacet differs from facetsConfig.TargetFacet when a result-facet is applied

            Debug.Assert(facetsConfig.TargetFacet != null, "facetsConfig.TargetFacet is NULL ");

            if (facetsConfig == null)
                facetCodes = new List<string>();

            var affectedConfigs = facetsConfig.GetFacetConfigsAffectedByFacet(facetCodes, targetFacet);

            // Collect all affected tables (those defined in affected facets)
            List<string> tables = (extraTables ?? new List<string>())               // ...extra tables (if any)...
                .Concat(targetFacet.ExtraTables.Select(z => z.ObjectName))           // ...target facet's extra tables...
                .Concat(affectedConfigs.SelectMany(c => c.GetJoinTables()))         // ...tables from affected facets...
                .Distinct().ToList();

            // Compute criteria clauses for user picks for each affected facet
            var criterias = affectedConfigs
                .Select(config => (
                    (Tablename: config.Facet.ResolvedName,
                      Criteria: GetCompiler(config).Compile(targetFacet, config.Facet, config)))
                 )
                .ToList();

            // Group criterias per table, and store in a dictionary keyed by table name
            Dictionary<string, string> pickCriterias = criterias
                .GroupBy(p => p.Tablename, p => p.Criteria, (key, g) => new { TableName = key, Clauses = g.ToList() })
                .ToDictionary(z => z.TableName, z => "(" + z.Clauses.Combine(" AND ") + ")");

            // FIXME Check start node should be ResolvedName or TargetName
            // Find all routes between target facet's table and all tabled collected in affected facets
            List<GraphRoute> routes = Graph.Find(targetFacet.ResolvedName /*TargetTableNamec*/, tables);

            // Reduce routes (remove duplicated edges)
            List<GraphRoute> reducedRoutes = GraphRoute.Utility.Reduce(routes);

            // Compile list of joins for the reduced route
            List<string> joins = reducedRoutes
                .SelectMany(route => route.Items)
                .Select(edge => EdgeCompiler.Compile(Graph, edge, HasUserPicks(edge, pickCriterias)))
                .ToList();

            QuerySetup querySetup = new QuerySetup(facetsConfig?.TargetConfig, targetFacet, joins, pickCriterias, routes, reducedRoutes);

            return querySetup;
        }

        private bool HasUserPicks(GraphEdge edge, Dictionary<string, string> tableCriterias)
        {
            return tableCriterias.ContainsKey(edge.SourceName) || tableCriterias.ContainsKey(edge.TargetName);
        }

        private IPickFilterCompiler GetCompiler(FacetConfig2 c)
        {
            return PickCompilers[(int)c.Facet.FacetTypeId];
        }
    }
}