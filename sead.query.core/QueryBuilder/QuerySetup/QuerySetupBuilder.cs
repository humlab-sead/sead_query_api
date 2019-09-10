using Autofac.Features.Indexed;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public interface IQuerySetupBuilder
    {
        IFacetsGraph Graph { get; set; }

        QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables = null);
        QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables, List<string> facetCodes);
    }

    public class QuerySetupBuilder : IQuerySetupBuilder {
        public IRepositoryRegistry Context { get; set; }
        public IFacetsGraph Graph { get; set; }
        public IIndex<int, IPickFilterCompiler> PickCompilers { get; set; }
        public IEdgeSqlCompiler EdgeCompiler { get; }

        public QuerySetupBuilder(
            IRepositoryRegistry _context,
            IFacetsGraph graph,
            IIndex<int, IPickFilterCompiler> pickCompilers,
            IEdgeSqlCompiler edgeCompiler
        ) {
            Context = _context;
            Graph = graph;
            PickCompilers = pickCompilers;
            EdgeCompiler = edgeCompiler;
            //File.WriteAllText(@"C:\TEMP\dotnet_facet_graph.csv", Graph.ToCSV());
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables = null)
        {
            List<string> facetCodes = facetsConfig.GetFacetCodes().AddIfMissing(facetCode);
            return Build(facetsConfig, facetCode, extraTables ?? new List<string>(), facetCodes);
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables, List<string> facetCodes)
        {
            Facet targetFacet = Context.Facets.GetByCode(facetCode);

            if (facetsConfig == null)
                facetCodes = new List<string>();

            var affectedConfigs = facetsConfig.GetFacetConfigsAffectedByFacet(facetCodes, targetFacet);

            // Collect all affected tables (those defined in affected facets)
            List<string> tables = (extraTables ?? new List<string>())               // ...extra tables (if any)...
                .Concat(targetFacet.ExtraTables.Select(z => z.TableName))           // ...target facet's extra tables...
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
            //Debug.Print(GraphRoute.Utility.ToString(reducedRoutes));

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