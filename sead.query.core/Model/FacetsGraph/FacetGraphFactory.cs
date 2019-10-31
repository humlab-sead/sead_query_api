using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, Table>;
    public class FacetGraphFactory: IFacetGraphFactory
    {
        public FacetGraphFactory(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        public IRepositoryRegistry Registry { get; }

        // FIXME Refactor to use FacetTableRepository.AliasMap
        public IFacetsGraph Build()
        {
            List<Table> nodes = Registry.Tables.GetAll().ToList();
            List<TableRelation> edges = Registry.TableRelations.GetAll().ToList();
            Dictionary<string, FacetTable> aliasTables = Registry.FacetTables.AliasTablesDict();

            IEnumerable<Table> aliasNodes = CreateAliasNodes(aliasTables);

            nodes = nodes.Concat(aliasNodes).Distinct().ToList();

            var aliasEdges = GetAliasEdges(edges, nodes, aliasTables).ToList<TableRelation>();
            aliasEdges = aliasEdges.Where(x => !edges.Contains(x)).ToList();
            edges.AddRange(aliasEdges);

            return new FacetsGraph(nodes, edges, aliasTables);
        }

        private static IEnumerable<Table> CreateAliasNodes(Dictionary<string, FacetTable> aliasDict)
        {
            var id = 10000;
            var aliasNodes = aliasDict.Select(x => new Table() { TableId = id++, TableOrUdfName = x.Key });
            return aliasNodes;
        }

        private List<TableRelation> GetAliasEdges(
            List<TableRelation> edges,
            List<Table> nodes,
            Dictionary<string, FacetTable> aliasDict
        )
        {
            List<TableRelation> aliasEdges = new List<TableRelation>();

            var nodesDict = nodes.ToDictionary(x => x.TableOrUdfName);

            // Copy target tables relations for each alias...
            foreach (var facetTable in aliasDict.Values) {

                // ...fetch all relations where target is a node...
                var targetEdges = edges.Where(x => x.SourceName == facetTable.TableOrUdfName || x.TargetName == facetTable.TableOrUdfName);

                // ...add a corresponding alias relation for each target relation, ...
                aliasEdges.AddRange(
                    targetEdges.Select(z => z.Alias(nodesDict[facetTable.TableOrUdfName], nodesDict[facetTable.Alias]))
                );

            }
            return aliasEdges.Distinct().ToList();
        }
    }

    //public class FacetGraphFactory : IFacetGraphFactory
    //{
    //    // FIXME Refactor to use FacetTableRepository.AliasMap
    //    public IFacetsGraph Build(List<Table> nodes, List<TableRelation> edges, List<Facet> aliasFacets)
    //    {

    //        var id = 10000;
    //        var aliasNodes = aliasFacets
    //            .Where(f => !nodes.Any(n => f.TargetTable.Alias == n.TableOrUdfName))
    //            .Select(f => new Table() {
				//	TableId = id++,
				//	TableOrUdfName = f.TargetTable.Alias
				//});

    //        nodes = nodes.Concat(aliasNodes).Distinct().ToList();

    //        var aliasEdges = GetAliasEdges(edges, nodes, aliasFacets).ToList<TableRelation>();
    //        aliasEdges = aliasEdges.Where(x => !edges.Contains(x)).ToList();
    //        edges.AddRange(aliasEdges);

    //        Dictionary<string, string> aliases = CreateAliasToTableOrUdfNameMapping(aliasFacets);

    //        return new FacetsGraph(nodes, edges, aliases);
    //    }

    //    private static Dictionary<string, string> CreateAliasToTableOrUdfNameMapping(List<Facet> aliasFacets)
    //    {
    //        return aliasFacets?
    //            .Select(z => (
    //                Alias: z.TargetTable.Alias,
    //                TableOrUdfName: z.TargetTable.TableOrUdfName
    //            ))
    //            .ToList()
    //            .Distinct()
    //            .ToDictionary(
    //                x => x.Alias,
    //                x => x.TableOrUdfName
    //            );
    //    }

    //    private List<TableRelation> GetAliasEdges(
    //        List<TableRelation> edges,
    //        List<Table> nodes,
    //        List<Facet> aliasFacets
    //    )
    //    {
    //        List<TableRelation> aliasEdges = new List<TableRelation>();

    //        var nodesDict = nodes.ToDictionary(x => x.TableOrUdfName);

    //        // Copy target tables relations for all alias facets...
    //        foreach (var facet in aliasFacets)
    //        {
    //            // ...fetch all relations where target is a node...
    //            var targetEdges = edges.Where(x => x.SourceName == facet.TargetTable?.TableOrUdfName || x.TargetName == facet.TargetTable?.TableOrUdfName);

    //            // ...add a corresponding alias relation for each target relation, ...
    //            aliasEdges.AddRange(
    //                targetEdges.Select(z => z.Alias(nodesDict[facet.TargetTable.TableOrUdfName], nodesDict[facet.TargetTable.Alias]))
    //            );

    //        }
    //        return aliasEdges.Distinct().ToList();
    //    }
    //}
}