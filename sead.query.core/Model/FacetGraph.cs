using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, GraphNode>;
    using NodesDictI = Dictionary<int, GraphNode>;
    using WeightDictionary = Dictionary<int, Dictionary<int, int>>;

    public interface IFacetGraphFactory
    {
        IFacetsGraph Build();
    }

    public interface IFacetsGraph {
        //List<Facet> AliasFacets { get; }
        Dictionary<Tuple<string, string>, GraphEdge> Edges { get; }
        NodesDictI NodesIds { get; }
        NodesDictS Nodes { get; }
        Dictionary<int, Dictionary<int, int>> Weights { get; }
        GraphEdge GetEdge(int sourceId, int targetId);
        GraphEdge GetEdge(string source, string target);
        GraphRoute Find(string start_table, string destination_table);
        List<GraphRoute> Find(string start_table, List<string> destination_tables);
        bool IsAlias(string tableName);
        string ResolveTargetName(string aliasOrTable);
        string ToCSV();
        string ResolveAliasName(string targetTableName);
    }

    public class FacetsGraph : IFacetsGraph {

        public NodesDictS Nodes { get; set; }
        public NodesDictI NodesIds { get; set; }
        public Dictionary<Tuple<string, string>, GraphEdge> Edges { get; set; }
        //public List<Facet> AliasFacets { get; set; }
        public Dictionary<string, string> AliasTables;
        public WeightDictionary Weights { get; set; }

        public FacetsGraph(NodesDictS nodes, List<GraphEdge> edges, List<Facet> aliasFacets)
        {
            Nodes = nodes;

            NodesIds = nodes.Values.ToDictionary(x => x.NodeId);

            Edges = edges.ToDictionary(z => z.Key);

            //AliasFacets = aliasFacets ?? new List<Facet>();
            AliasTables = (aliasFacets ?? new List<Facet>())
                    .ToDictionary(x => x.AliasName, x => (x.TargetTable?.ObjectName ?? ""));

            Weights = edges.GroupBy(p => p.SourceNodeId, (key, g) => (key, g.ToDictionary(x => x.TargetNodeId, x => x.Weight)))
                .ToDictionary(x => x.Item1, y => y.Item2);
        }

        public GraphEdge GetEdge(string source, string target) => Edges[Tuple.Create(source, target)];
        public GraphEdge GetEdge(int sourceId, int targetId) => Edges[Tuple.Create(NodesIds[sourceId].TableName, NodesIds[targetId].TableName)];

        public bool IsAlias(string name) => AliasTables.ContainsKey(name);

        public string ResolveTargetName(string aliasOrTable)
        {
            return IsAlias(aliasOrTable) ? AliasTables[aliasOrTable] : aliasOrTable;
        }

        public string ResolveAliasName(string aliasOrTable)
        {
            return IsAlias(aliasOrTable) ? aliasOrTable : null;
        }

        public List<GraphRoute> Find(string start_table, List<string> destination_tables)
        {
            // Add start_table to destination tables???
            return destination_tables.Where(w => start_table != w).Select(z => Find(start_table, z)).ToList();
        }

        public GraphRoute Find(string startTable, string destinationTable)
        {
            return Find(Nodes[startTable].NodeId, Nodes[destinationTable].NodeId);
        }

        public GraphRoute Find(int startTableId, int destinationTableId)
        {
            GraphRoute CreateRoute(List<GraphNode> nodes)
            {
                List<GraphEdge> items = new List<GraphEdge>();
                for (int i = 0; i < nodes.Count - 1; i++)
                    items.Add(GetEdge(nodes[i].TableName, nodes[i + 1].TableName));
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