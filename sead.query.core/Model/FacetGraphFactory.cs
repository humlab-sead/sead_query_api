using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, GraphNode>;

    public class FacetGraphFactory : IFacetGraphFactory
    {
        public IRepositoryRegistry Context { get; private set; }

        public FacetGraphFactory(IRepositoryRegistry repositoryRegistry)
        {
            Context = repositoryRegistry;
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

        private NodesDictS AddAliasNodes(List<Facet> aliasFacets, NodesDictS nodes)
        {
            var aliases = GetAliasNodes(nodes, aliasFacets).ToList();
            aliases.ForEach(z => nodes[z.TableName] = z);
            return nodes;
        }

        private List<GraphNode> GetAliasNodes(NodesDictS nodes, List<Facet> aliasFacets)
        {
            int id = 10000;
            return aliasFacets
                .Select(z => new GraphNode() { NodeId = id++, TableName = z.AliasName })
                .Where(z => !nodes.ContainsKey(z.TableName)).ToList();
        }

        private List<GraphEdge> AddAliasEdges(List<GraphEdge> edges, NodesDictS nodes, List<Facet> aliasFacets)
        {
            // Copy target tables relations for all alias facets...
            foreach (var facet in aliasFacets)
            {
                // ...fetch all relations where target is a node...
                var targetEdges = edges.Where(x => x.SourceName == facet.TargetTable?.ObjectName || x.TargetName == facet.TargetTable?.ObjectName);

                // ...for each target relation, create a corresponding alias relation...
                var aliasEdges = targetEdges.Select(z => z.Alias(nodes[facet.TargetTable.ObjectName], nodes[facet.AliasName]));

                // ...filter away edges that already exists...
                aliasEdges = aliasEdges.Where(z => !edges.Exists(w => w.EqualAs(z)));

                edges.AddRange(aliasEdges);
            }
            return edges;
        }

        private List<GraphEdge> AddReverseEdges(List<GraphEdge> edges)
        {
            var reverse = edges.Where(z => z.SourceNodeId != z.TargetNodeId).Select(x => x.Reverse()).ToList();
            edges.AddRange(reverse);
            return edges;
        }
    }
}