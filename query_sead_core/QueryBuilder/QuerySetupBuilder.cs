using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain.QueryBuilder {

    public class QuerySetupBuilder : IQuerySetupBuilder {
        public IUnitOfWork Context { get; set; }
        public IFacetsGraph Graph { get; set; }

        public QuerySetupBuilder(IUnitOfWork _context, IFacetsGraph graph) {
            Context = _context;
            Graph = graph;
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables = null)
        {
            List<string> facetCodes = facetsConfig.GetFacetCodes();
            if (facetCodes.Contains(facetCode)) {
                facetCodes.Add(facetCode);
            }
            return Build(facetsConfig, facetCode, extraTables ?? new List<string>(), facetCodes);
        }

        public QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables, List<string> facetCodes)
        {
            extraTables = extraTables ?? new List<string>();
            FacetDefinition targetFacet = Context.Facets.GetByCode(facetCode);
            IEnumerable<string> tables = extraTables.Concat(targetFacet.ExtraTables.Select(z => z.TableName)); 

            Dictionary<string, List<string>> filter_clauses = new Dictionary<string, List<string>>();

            if (facetsConfig == null)
                facetCodes = new List<string>();

            List<(string, string)> filterClauses = new List<(string, string)>();

            foreach (var currentCode in facetCodes) {

                FacetConfig2 config = facetsConfig.GetConfig(currentCode);

                if (config.Picks.Count == 0) {
                    continue;
                }

                FacetPickFilterCompiler compiler = FacetPickFilterCompiler.getCompiler(config.Facet.FacetTypeId);

                if (!compiler.is_affected_position(facetCodes.IndexOf(facetCode), facetCodes.IndexOf(currentCode))) {
                    continue;
                }

                string filterClause = compiler.compile(Context, facetCode, currentCode, config);

                filterClauses.Add((config.Facet.ResolvedName, filterClause));

                tables.Concat(config.Facet.ExtraTables.Select(z => z.TableName)).Append(config.Facet.ResolvedName);
            }

            var criteraGroups = filterClauses.GroupBy(p => p.Item1, p => p.Item2, (key, g) => new { TableName = key, Clauses = g.ToList() });

            // Dictionary<string, List<string>> tableCriterias = criteraGroups.ToDictionary(z => z.TableName, z => z.Clauses);
            Dictionary<string, string> joinCriterias = criteraGroups.ToDictionary(z => z.TableName, z => String.Join(" AND ", z.Clauses));

            List<GraphRoute> routes = computeShortestJoinPaths(targetFacet.ResolvedName, tables.Distinct().ToList());
            List<GraphRoute> reducedRoutes = RouteReducer.Reduce(routes);

            string categoryFilter = facetsConfig?.TargetConfig?.TextFilter ?? "";
            string joinClause = compileJoinClause(reducedRoutes, joinCriterias);

            QuerySetup querySetup = new QuerySetup(targetFacet, joinClause, joinCriterias, routes, reducedRoutes, categoryFilter);

            return querySetup;
        }

        private string compileJoinClause(List<GraphRoute> routes, Dictionary<string, string> tableCriteria)
        {
            // FIXME: Sub-Select i QVIZ används inte i Joins!
            StringBuilder joins = new StringBuilder("");
            foreach (var route in routes) {
                foreach (var edge in route.Items) {

                    GraphEdge relation = Graph.Edges[Tuple.Create(edge.TargetName, edge.SourceName)];

                    string joinType = tableCriteria.ContainsKey(edge.SourceName) || tableCriteria.ContainsKey(edge.TargetName) ? "inner" : "left";
                    string targetName = Graph.ResolveName(edge.TargetName);
                    string aliasClause = (targetName != edge.TargetName) ? $" AS {edge.TargetName} " : "";
                    string relationClause = relation.GetSqlJoinClause(joinType);

                    joins.Append($" {joinType} JOIN {targetName} {aliasClause}\n  ON {relationClause} \n");
                }
            }
            return joins.ToString();
        }

        private List<GraphRoute> computeShortestJoinPaths(string start_table, List<string> destination_tables)
        {
            return destination_tables.Select(z => computeShortestJoinPath(start_table, z)).ToList();
        }

        private GraphRoute computeShortestJoinPath(string start_table, string destination_table)
        {
            if (!Graph.Nodes.ContainsKey(start_table))
                throw new ArgumentException("Node undefined: " + start_table);

            if (!Graph.Nodes.ContainsKey(start_table))
                throw new ArgumentException("Node undefined: " + destination_table);

            int start_node = Graph.Nodes[start_table].TableId;
            int destination_node = Graph.Nodes[destination_table].TableId;

            DijkstrasGraph<int> dijkstra = new DijkstrasGraph<int>(Graph.Weights);

            List<int> trail = dijkstra.shortest_path(start_node, destination_node);

            return new GraphRoute(trail.Select(x => Graph.NodeIds[x].TableName).ToList());
        }
    }

}