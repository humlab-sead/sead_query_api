using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySeadDomain
{
    using NodesDict = Dictionary<string, GraphNode>;

    public interface IFacetsGraph {
        List<FacetDefinition> AliasFacets { get; }
        Dictionary<Tuple<string, string>, GraphEdge> Edges { get; }
        Dictionary<int, GraphNode> NodeIds { get; }
        NodesDict Nodes { get; }
        Dictionary<int, Dictionary<int, int>> Weights { get; }
        GraphEdge GetEdge(int sourceId, int targetId);
        GraphEdge GetEdge(string source, string target);
        string ResolveName(string tableName);
    }

    public class FacetGraphFactory {


        public static IFacetsGraph instance = null;

        public static IFacetsGraph Create(IUnitOfWork context)
        {
            return instance ?? (instance = new FacetGraphFactory().Build(context));
        }

        public IFacetsGraph Build(IUnitOfWork context)
        {
            // FIXME! Edges where source_table = target_table?? Key=(tbl_dataset,tbl_dataset)
            // Could it be that key must be "table_name.column_name"
            var edgesList = context.Edges.GetAll().ToList();

            var nodesList = context.Nodes.GetAll().ToList();

            var aliasFacets = context.Facets.FindThoseWithAlias().ToList();
            var aliasTables = aliasFacets.ToDictionary(x => x.TargetTableName, x => x.AliasName);

            var nodes = nodesList.ToDictionary(x => x.TableName);

            var aliasNodes = GetAliasNodes(nodes, aliasFacets).ToList();
            if (aliasNodes.Count > 0) {
                aliasNodes.ForEach(z => nodes[z.TableName] = z);
                edgesList = AddAliasEdges(edgesList, nodes, aliasFacets);
            }

            edgesList = AddReverseEdges(edgesList);

            /* Create a dictionary of dictionaries */
            var weights = edgesList
                .GroupBy(
                    p => p.SourceTableId,
                    (key, g) => (
                        key,
                        g.ToDictionary(x => x.TargetTableId, x => x.Weight)))
                .ToDictionary(x => x.Item1, y => y.Item2);

            return new FacetsGraph() {
                Nodes       = nodes,
                Edges       = edgesList.ToDictionary(z => z.Key),
                AliasFacets = aliasFacets,
                AliasTables = aliasFacets.ToDictionary(x => x.TargetTableName, x => x.AliasName),
                NodeIds     = nodes.Values.ToDictionary(x => x.TableId),
                Weights     = weights
            };
        }

        private List<GraphNode> GetAliasNodes(NodesDict nodes, List<FacetDefinition> aliasFacets)
        {
            int id = nodes.Max(z => z.Value.TableId) + 1;
            return aliasFacets
                .Select(z => new GraphNode() { TableId = id++, TableName = z.AliasName })
                .Where(z => !nodes.ContainsKey(z.TableName)).ToList();
        }

        private List<GraphEdge> AddAliasEdges(List<GraphEdge> edgesList, NodesDict nodes, List<FacetDefinition> aliasFacets)
        {
            foreach (var facet in aliasFacets) {
                var values = edgesList
                    .Where(x => x.SourceTableName == facet.TargetTableName)
                    .Select(z => z.makeAlias(nodes[facet.AliasName]))
                    .ToList();
                edgesList.AddRange(values);
            }
            return edgesList;
        }

        private List<GraphEdge> AddReverseEdges(List<GraphEdge> edgesList)
        {
            edgesList.AddRange(
                edgesList
                    .Where(z => z.SourceTableId != z.TargetTableId)
                    .Select(x => x.Reverse()).ToList());
            return edgesList;
        }

    }

    public class FacetsGraph : IFacetsGraph {

        public NodesDict Nodes { get; set; }
        public Dictionary<Tuple<string, string>, GraphEdge> Edges { get; set; }
        public List<FacetDefinition> AliasFacets { get; set; }
        public Dictionary<string, string> AliasTables;
        public Dictionary<int, GraphNode> NodeIds { get; set; }
        public Dictionary<int, Dictionary<int, int>> Weights { get; set; }

        public GraphEdge GetEdge(string source, string target)
            => Edges[Tuple.Create(source, target)];

        public GraphEdge GetEdge(int sourceId, int targetId)
            => Edges[Tuple.Create(NodeIds[sourceId].TableName, NodeIds[targetId].TableName)];

        public string ResolveName(string tableName) =>
            AliasTables.ContainsKey(tableName) ? AliasTables[tableName] : tableName;

    }
}