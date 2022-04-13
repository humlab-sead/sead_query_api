using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    using WeightDictionary = Dictionary<int, Dictionary<int, int>>;
    using EdgeKey = Tuple<string, string>;
    using EdgeIdKey = Tuple<int, int>;

    public class GraphNodes<T> : IEnumerable where T : IGraphNode
    {
        public GraphNodes(IEnumerable<T> nodes)
        {
            Nodes = nodes;
            Name2Node = nodes.ToDictionary(x => x.GetName());
            Id2Node = nodes.ToDictionary(x => x.GetId());
        }

        public IEnumerable<T> Nodes { get; private set; }
        public Dictionary<string, T> Name2Node { get; private set; }
        public Dictionary<int, T> Id2Node { get; private set; }

        public T this[int index]
        {
            get
            {
                return Id2Node[index];
            }
        }
        public T this[string name]
        {
            get
            {
                return Name2Node[name];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }
    }

    public class GraphEdges<T> : IEnumerable where T : IGraphEdge
    {
        public Dictionary<EdgeKey, T> KeyLookup { get; private set; }
        public Dictionary<EdgeIdKey, T> IdKeyLookup { get; private set; }
        public IEnumerable<T> Edges { get; private set; }
        public GraphEdges(IEnumerable<T> edges, bool bidirectional = true)
        {
            if (bidirectional)
            {
                edges = edges.Concat(ReversedEdges(edges));
            }
            Edges = edges;
            KeyLookup = edges.ToDictionary(z => z.GetKey());
            IdKeyLookup = edges.ToDictionary(z => z.GetIdKey());
        }

        public T GetEdge(string source, string target)
            => KeyLookup[Tuple.Create(source, target)];

        public T GetEdge(int sourceId, int targetId)
            => IdKeyLookup[Tuple.Create(sourceId, targetId)];

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Edges.GetEnumerator();
        }
        public IEnumerable<T> ReversedEdges(IEnumerable<T> edges)
        {
            return edges
                .Where(z => z.GetSourceId() != z.GetTargetId())
                .Select(x => (T)x.Reverse())
                .Where(z => !edges.Any(w => w.Equals(z)));
        }

        public WeightDictionary ToWeightGraph() => Edges
         .GroupBy(p => p.GetSourceId(), (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.GetTargetId(), x => x.GetWeight())))
         .ToDictionary(x => x.SourceId, y => y.TargetWeights);
    }

    public class FacetsGraph : IFacetsGraph
    {

        public GraphNodes<Table> NodeContainer { get; private set; }
        public GraphEdges<TableRelation> EdgeContaniner { get; private set; }
        public IEnumerable<FacetTable> AliasedFacetTables { get; private set; }

        public FacetsGraph(IEnumerable<Table> nodes, IEnumerable<TableRelation> edges, IEnumerable<FacetTable> aliases, bool bidirectional = true)
        {
            NodeContainer = new GraphNodes<Table>(nodes);
            EdgeContaniner = new GraphEdges<TableRelation>(edges, bidirectional);
            AliasedFacetTables = aliases;
        }

        public List<GraphRoute> Find(string start_table, List<string> destination_tables, bool reduce = true)
        {
            // Make sure that start table doesn't exists in list of destination tables...
            destination_tables = destination_tables.Where(z => z != start_table).ToList();

            var routes = destination_tables.Select(z => Find(start_table, z)).ToList();

            if (reduce)
            {
                return GraphRouteUtility.Reduce(routes);
            }

            return routes;
        }

        public GraphRoute Find(string startTable, string destinationTable)
        {
            return Find(NodeContainer[startTable].TableId, NodeContainer[destinationTable].TableId);
        }

        public GraphRoute Find(int startId, int destId)
        {
            IEnumerable<int> trail = new DijkstrasGraph<int>(EdgeContaniner.ToWeightGraph())
                .shortest_path(startId, destId);

            if (trail == null)
                throw new ArgumentOutOfRangeException($"No route found between {startId} and {destId}");

            return ToGraphRoute(trail.Concat(new[] { startId }));
        }

        private GraphRoute ToGraphRoute(IEnumerable<int> trail)
            => new GraphRoute(ToEdges(trail.Reverse()));

        private IEnumerable<TableRelation> ToEdges(IEnumerable<int> trail)
            => trail
                .Select(x => NodeContainer[x])
                .PairWise((a, b) => EdgeContaniner.GetEdge(a.TableOrUdfName, b.TableOrUdfName));

        public string ToCSV()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableRelation edge in EdgeContaniner)
                sb.Append($"{edge.SourceName};{edge.TargetName};{edge.Weight}\n");
            return sb.ToString();
        }

        public FacetTable GetAliasedFacetTable(string aliasName)
            => AliasedFacetTables.Where(x => x.Alias == aliasName).FirstOrDefault();
    }
}