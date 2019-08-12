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
        List<FacetDefinition> AliasFacets { get; }
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
        public List<FacetDefinition> AliasFacets { get; set; }
        public Dictionary<string, string> AliasTables;
        public WeightDictionary Weights { get; set; }

        public FacetsGraph(NodesDictS nodes, List<GraphEdge> edges, List<FacetDefinition> aliasFacets)
        {
            Nodes = nodes;
            NodesIds = nodes.Values.ToDictionary(x => x.TableId);
            Edges = edges.ToDictionary(z => z.Key);
            AliasFacets = aliasFacets;
            //AliasTables = aliasFacets.ToDictionary(x => x.TargetTableName, x => x.AliasName);
            AliasTables = aliasFacets.ToDictionary(x => x.AliasName, x => x.TargetTableName);

            Weights = edges.GroupBy(p => p.SourceTableId, (key, g) => (key, g.ToDictionary(x => x.TargetTableId, x => x.Weight)))
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
            return Find(Nodes[startTable].TableId, Nodes[destinationTable].TableId);
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
                sb.Append($"{edge.Value.SourceTableName};{edge.Value.TargetTableName};{edge.Value.Weight}\n");
            return sb.ToString();
        }
    }

    public class FacetGraphFactory : IFacetGraphFactory
    {
        public IUnitOfWork Context { get; private set; }

        public FacetGraphFactory(IUnitOfWork context)
        {
            Context = context;
        }

        public IFacetsGraph Build()
        {
            // FIXME! Edges where source_table = target_table?? Key=(tbl_dataset,tbl_dataset)
            // Could it be that key must be "table_name.column_name"

            var edges = Context.Edges.GetAll().ToList();
            var nodes = Context.Nodes.GetAll().ToDictionary(x => x.TableName);

            var aliasFacets = Context.Facets.FindThoseWithAlias().ToList();

            nodes = AddAliasNodes(aliasFacets, nodes);
            edges = AddAliasEdges(edges, nodes, aliasFacets);
            edges = AddReverseEdges(edges);

            return new FacetsGraph(nodes, edges, aliasFacets);
        }

        private NodesDictS AddAliasNodes(List<FacetDefinition> aliasFacets, NodesDictS nodes)
        {
            var aliases = GetAliasNodes(nodes, aliasFacets).ToList();
            aliases.ForEach(z => nodes[z.TableName] = z);
            return nodes;
        }

        private List<GraphNode> GetAliasNodes(NodesDictS nodes, List<FacetDefinition> aliasFacets)
        {
            int id = 10000;
            return aliasFacets
                .Select(z => new GraphNode() { TableId = id++, TableName = z.AliasName })
                .Where(z => !nodes.ContainsKey(z.TableName)).ToList();
        }

        private List<GraphEdge> AddAliasEdges(List<GraphEdge> edges, NodesDictS nodes, List<FacetDefinition> aliasFacets)
        {
            // Copy target tables relations for all alias facets...
            foreach (var facet in aliasFacets)
            {
                // ...fetch all relations where target is a node...
                var targetEdges = edges.Where(x => x.SourceTableName == facet.TargetTableName || x.TargetTableName == facet.TargetTableName);

                // ...for each target relation, create a corresponding alias relation...
                var aliasEdges = targetEdges.Select(z => z.Alias(nodes[facet.TargetTableName], nodes[facet.AliasName]));

                // ...filter away edges that already exists...
                aliasEdges = aliasEdges.Where(z => !edges.Exists(w => w.EqualAs(z)));

                edges.AddRange(aliasEdges);
            }
            return edges;
        }

        private List<GraphEdge> AddReverseEdges(List<GraphEdge> edges)
        {
            var reverse = edges.Where(z => z.SourceTableId != z.TargetTableId).Select(x => x.Reverse()).ToList();
            edges.AddRange(reverse);
            return edges;
        }
    }
}