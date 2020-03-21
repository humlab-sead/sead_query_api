using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, Table>;
    using NodesDictI = Dictionary<int, Table>;
    using WeightDictionary = Dictionary<int, Dictionary<int, int>>;

    public class FacetsGraph : IFacetsGraph {

        public NodesDictS Nodes { get; set; }
        public NodesDictI NodesIds { get; set; }
        public Dictionary<Tuple<string, string>, TableRelation> Edges { get; set; }
        public Dictionary<string, FacetTable> AliasTables { get; private set; }
        public WeightDictionary Weights { get; set; }

        public FacetsGraph(
            List<Table> nodes,
            List<TableRelation> edges,
            Dictionary<string, FacetTable> aliasDict,
            bool addReversed=true
        )
        {
            Nodes = nodes.ToDictionary(x => x.TableOrUdfName);
            NodesIds = nodes.ToDictionary(x => x.TableId);

            //edges = edges.Distinct();

            if (addReversed) {
                IEnumerable<TableRelation> reversed = ReversedEdges(edges).ToList();
                edges.AddRange(reversed);
            }

            Edges = edges.ToDictionary(z => z.Key);
            AliasTables = aliasDict ?? new Dictionary<string, FacetTable>();

            Weights = edges
                .GroupBy(p => p.SourceTableId, (key, g) => (key, g.ToDictionary(x => x.TargetTableId, x => x.Weight)))
                .ToDictionary(x => x.Item1, y => y.Item2);
        }

        private IEnumerable<TableRelation> ReversedEdges(List<TableRelation> edges)
        {
            return edges
                .Where(z => z.SourceTableId != z.TargetTableId)
                .Select(x => x.Reverse())
                .Where(z => !edges.Any(w => w.Equals(z)));
        }

        public TableRelation GetEdge(string source, string target) => Edges[Tuple.Create(source, target)];
        public TableRelation GetEdge(int sourceId, int targetId) => Edges[Tuple.Create(NodesIds[sourceId].TableOrUdfName, NodesIds[targetId].TableOrUdfName)];

        //public bool IsAlias(string name) => AliasTables.ContainsKey(name);

        //public string ResolveTargetName(string aliasOrTable)
        //{
        //    return IsAlias(aliasOrTable) ? AliasTables[aliasOrTable] : aliasOrTable;
        //}

        //public string ResolveAliasName(string aliasOrTable)
        //{
        //    return IsAlias(aliasOrTable) ? aliasOrTable : null;
        //}

        public List<GraphRoute> Find(string start_table, List<string> destination_tables, bool reduce=true)
        {
            // Make sure that start table doesn't exists in list of destination tables...
            destination_tables = destination_tables.Where(z => z != start_table).ToList();

            var routes = destination_tables.Select(z => Find(start_table, z)).ToList();

            if (reduce) {
                return Reduce(routes);
            }
            return routes;
        }

        private static List<GraphRoute> Reduce(List<GraphRoute> routes)
        {
            return GraphRouteUtility.Reduce(routes);
        }

        public GraphRoute Find(string startTable, string destinationTable)
        {
            return Find(Nodes[startTable].TableId, Nodes[destinationTable].TableId);
        }

        public GraphRoute Find(int startTableId, int destinationTableId)
        {
            GraphRoute CreateRoute(List<Table> nodes)
            {
                List<TableRelation> items = new List<TableRelation>();
                for (int i = 0; i < nodes.Count - 1; i++)
                    items.Add(GetEdge(nodes[i].TableOrUdfName, nodes[i + 1].TableOrUdfName));
                return new GraphRoute(items);
            }
            List<int> trail = new DijkstrasGraph<int>(Weights).shortest_path(startTableId, destinationTableId);
            trail.Add(startTableId);
            trail.Reverse();
            return CreateRoute(trail.Select(x => NodesIds[x]).ToList());
        }

        public string ToCSV()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var edge in Edges)
                sb.Append($"{edge.Value.SourceName};{edge.Value.TargetName};{edge.Value.Weight}\n");
            return sb.ToString();
        }
    }
}