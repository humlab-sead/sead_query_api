using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, GraphNode>;

    public class FacetGraphFactory : IFacetGraphFactory
    {

        public IFacetsGraph Build(List<GraphNode> nodes, List<GraphEdge> edges, List<Facet> aliasFacets)
        {
            // FIXME! Edges where source_table = target_table?? Key=(tbl_dataset,tbl_dataset)
            // Could it be that key must be "table_name.column_name"

            var id = 10000;
            var aliasNodes = aliasFacets
                .Where(f => !nodes.Any(n => f.AliasName == n.TableName))
                .Select(f => new GraphNode() { NodeId = id++, TableName = f.AliasName });

            nodes = nodes.Concat(aliasNodes).Distinct().ToList();

            var aliasEdges = GetAliasEdges(edges, nodes, aliasFacets).ToList<GraphEdge>();
            aliasEdges = aliasEdges.Where(x => !edges.Contains(x)).ToList();
            edges.AddRange(aliasEdges);

            var aliases = aliasFacets?
                .ToDictionary(x => x.AliasName, x => (x.TargetTable?.ObjectName ?? ""));

            return new FacetsGraph(nodes, edges, aliases);
        }

        private List<GraphEdge> GetAliasEdges(
            List<GraphEdge> edges,
            List<GraphNode> nodes,
            List<Facet> aliasFacets
        )
        {
            List<GraphEdge> aliasEdges = new List<GraphEdge>();

            var nodesDict = nodes.ToDictionary(x => x.TableName);

            // Copy target tables relations for all alias facets...
            foreach (var facet in aliasFacets)
            {
                // ...fetch all relations where target is a node...
                var targetEdges = edges.Where(x => x.SourceName == facet.TargetTable?.ObjectName || x.TargetName == facet.TargetTable?.ObjectName);

                // ...add a corresponding alias relation for each target relation, ...
                aliasEdges.AddRange(
                    targetEdges.Select(z => z.Alias(nodesDict[facet.TargetTable.ObjectName], nodesDict[facet.AliasName]))
                );

            }
            return aliasEdges.Distinct().ToList();
        }
    }
}