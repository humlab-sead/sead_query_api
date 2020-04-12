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

        public IFacetsGraph Build()
        {
            var nodes = Registry.Tables.GetAll();
            var edges = Registry.TableRelations.GetAll();
            var aliases = Registry.FacetTables.FindThoseWithAlias();

            nodes = nodes.Concat(GetAliasTables(aliases)).Distinct();
            edges = edges.Concat(GetAliasTableRelations(aliases, edges, nodes));

            return new FacetsGraph(nodes, edges, aliases);
        }

        private IEnumerable<Table> GetAliasTables(IEnumerable<FacetTable> aliases)
        {
            // ...project all FacetTable items that has an alias to a new Table item
            var aliasNodes = aliases
                .Select((x, id) => new Table {
                    TableId = 10000 + id,
                    TableOrUdfName = x.Alias
                });
            return aliasNodes;
        }

        private IEnumerable<TableRelation> GetAliasTableRelations(IEnumerable<FacetTable> aliases, IEnumerable<TableRelation> edges, IEnumerable<Table> tables)
        {
            List<TableRelation> aliasEdges = new List<TableRelation>();

            var tableLookup = tables.ToDictionary(x => x.TableOrUdfName);

            // Copy target tables relations for each alias...
            foreach (var facetTable in aliases) {

                // ...fetch all relations where target is a node...
                var targetEdges = edges.Where(x => x.SourceName == facetTable.TableOrUdfName || x.TargetName == facetTable.TableOrUdfName);

                // ...add a corresponding alias relation for each target relation, ...
                aliasEdges.AddRange(
                    targetEdges.Select(z => z.Alias(tableLookup[facetTable.TableOrUdfName], tableLookup[facetTable.Alias]))
                );

            }
            return aliasEdges
                .Where(x => !edges.Contains(x))
                .Distinct();
        }
    }
}