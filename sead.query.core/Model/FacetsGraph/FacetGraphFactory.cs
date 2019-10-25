using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, GraphNode>;

    public class FacetGraphFactory : IFacetGraphFactory
    {

        public IFacetsGraph Build(List<GraphNode> nodes, List<GraphEdge> edges, List<Facet> aliasFacets)
        {

            var id = 10000;
            var aliasNodes = aliasFacets
                .Where(f => !nodes.Any(n => f.AliasName == n.TableName))
                .Select(f => new GraphNode() { NodeId = id++, TableName = f.AliasName });

            nodes = nodes.Concat(aliasNodes).Distinct().ToList();

            var aliasEdges = GetAliasEdges(edges, nodes, aliasFacets).ToList<GraphEdge>();
            aliasEdges = aliasEdges.Where(x => !edges.Contains(x)).ToList();
            edges.AddRange(aliasEdges);

            Dictionary<string, string> aliases = CreateAliasNameToObjectNameMapping(aliasFacets);

            return new FacetsGraph(nodes, edges, aliases);
        }

        private static Dictionary<string, string> CreateAliasNameToObjectNameMapping(List<Facet> aliasFacets)
        {
            //var aliases = aliasFacets?
            //    .ToDictionary(
            //        x => x.AliasName,
            //        x => (x.TargetTable?.TableOrUdfName ?? "")
            //    );
            return aliasFacets?
                .Select(z => (
                    AliasName: z.AliasName,
                    TableOrUdfName: z.TargetTable?.TableOrUdfName ?? ""
                ))
                .ToList()
                .Distinct()
                .ToDictionary(
                    x => x.AliasName,
                    x => x.TableOrUdfName
                );
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
                var targetEdges = edges.Where(x => x.SourceName == facet.TargetTable?.TableOrUdfName || x.TargetName == facet.TargetTable?.TableOrUdfName);

                // ...add a corresponding alias relation for each target relation, ...
                aliasEdges.AddRange(
                    targetEdges.Select(z => z.Alias(nodesDict[facet.TargetTable.TableOrUdfName], nodesDict[facet.AliasName]))
                );

            }
            return aliasEdges.Distinct().ToList();
        }
    }
}