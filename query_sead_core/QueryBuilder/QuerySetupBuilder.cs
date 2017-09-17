using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain.QueryBuilder {

    public interface IQuerySetupBuilder
    {
        IFacetsGraph Graph { get; set; }

        QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables = null);
        QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables, List<string> facetCodes);
    }

    public class QuerySetupBuilder : IQuerySetupBuilder {
        public IUnitOfWork Context { get; set; }
        public IFacetsGraph Graph { get; set; }

        public QuerySetupBuilder(IUnitOfWork _context, IFacetsGraph graph) {
            Context = _context;
            Graph = graph;
            //File.WriteAllText(@"C:\TEMP\dotnet_facet_graph.csv", Graph.ToCSV());
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables = null)
        {
            List<string> facetCodes = facetsConfig.GetFacetCodes();
            if (!facetCodes.Contains(facetCode))
                facetCodes.Add(facetCode);
            return Build(facetsConfig, facetCode, extraTables ?? new List<string>(), facetCodes);
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables, List<string> facetCodes)
        {
            FacetDefinition targetFacet = Context.Facets.GetByCode(facetCode);

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
                .ToDictionary(z => z.TableName, z => "(" + String.Join(" AND ", z.Clauses) + ")");

            // FIXME Check start node should be ResolvedName or TargetTableName
            // Find all routes between target facet's table and all tabled collected in affected facets
            List<GraphRoute> routes = Graph.Find(targetFacet.ResolvedName /*TargetTableNamec*/, tables);

            // Reduce routes (remove duplicated edges)
            List<GraphRoute> reducedRoutes = GraphRoute.Utility.Reduce(routes);

            // Compile list of joins for the reduced route
            List<string> joins = reducedRoutes
                .SelectMany(route => route.Items)
                .Select(edge => JoinClauseCompiler.GenerateSQL(Graph, edge, HasUserPicks(edge, pickCriterias)))
                .ToList();
            Debug.Print(GraphRoute.Utility.ToString(reducedRoutes));
            string categoryFilter = facetsConfig?.TargetConfig?.TextFilter ?? "";

            QuerySetup querySetup = new QuerySetup(targetFacet, joins, pickCriterias, routes, reducedRoutes, categoryFilter);

            return querySetup;
        }

        private bool HasUserPicks(GraphEdge edge, Dictionary<string, string> tableCriterias)
        {
            return tableCriterias.ContainsKey(edge.SourceTableName) || tableCriterias.ContainsKey(edge.TargetTableName);
        }

        private FacetPickFilterCompiler GetCompiler(FacetConfig2 c)
        {
            return FacetPickFilterCompiler.GetCompiler(c.Facet.FacetTypeId);
        }
    }

    public class JoinClauseCompiler
    {
        public static string GenerateSQL(IFacetsGraph graph, GraphEdge edge, bool innerJoin = false)
        {
            var alias = graph.ResolveAlias(edge.TargetTableName);
            var joinType = innerJoin ? "INNER" : "LEFT";
            var sql = $" {joinType} JOIN {edge.TargetTableName} {alias ?? ""}" +
                    $" ON {alias ?? edge.TargetTableName}.\"{edge.TargetColumnName}\" = " +
                            $"{edge.SourceTableName}.\"{edge.SourceColumnName}\" \n";
            return sql;
        }
    }

}