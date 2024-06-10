using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{

    public class EdgeRepository(IRepositoryRegistry registry) : Repository<TableRelation, int>(registry), IEdgeRepository
    {

        public class EdgesLookup(IEnumerable<TableRelation> edges)
        {
            public Dictionary<Tuple<string, string>, TableRelation> NameLookup { get; private set; } = edges.ToDictionary(z => z.Key);
            public Dictionary<Tuple<int, int>, TableRelation> IdLookup { get; private set; } = edges.ToDictionary(z => z.IdKey);
            public TableRelation GetEdge(string source, string target) => NameLookup[Tuple.Create(source, target)];
            public TableRelation GetEdge(int sourceId, int targetId) => IdLookup[Tuple.Create(sourceId, targetId)];
        }

        private IEnumerable<TableRelation> __edges = null;
        public EdgesLookup __Lookup { get; private set; }


        public IEnumerable<TableRelation> GetEdges(bool bidirectional = true)
        {
            if (__edges == null)
            {
                var edges = GetAll();
                var aliases = Registry.FacetTables.FindThoseWithAlias();

                edges = edges.Concat(GetAliasedEdges(aliases, edges));

                if (bidirectional)
                {
                    edges = edges.Concat(GraphUtility.Reverse(edges));
                }

                __edges = edges;
            }
            return __edges;
        }

        private EdgesLookup GetLookup()
        {
            if (__Lookup == null)
            {
                __Lookup = new EdgesLookup(GetEdges());
            }
            return __Lookup;
        }

        public override IEnumerable<TableRelation> GetAll()
        {
            return Context.Set<TableRelation>().BuildEntity().ToList();
        }

        public TableRelation FindByName(string sourceName, string targetName)
        {
            string[] names = [sourceName, targetName];
            return Context.Set<TableRelation>().BuildEntity()
                .Where(
                    r => r.SourceTable.TableOrUdfName == sourceName && r.TargetTable.TableOrUdfName == targetName
                ).FirstOrDefault();
        }

        private IEnumerable<TableRelation> GetAliasedEdges(IEnumerable<FacetTable> aliases, IEnumerable<TableRelation> edges)
        {
            var nodes = Registry.Tables.GetNodes();

            List<TableRelation> aliasEdges = [];

            var tableLookup = nodes.ToDictionary(x => x.TableOrUdfName);

            // Copy target tables relations for each alias...
            foreach (var facetTable in aliases)
            {
                // ...fetch all relations where target is a node...
                var targetEdges = edges.Where(x => x.SourceName == facetTable.TableOrUdfName || x.TargetName == facetTable.TableOrUdfName);

                // ...add a corresponding alias relation for each target relation, ...
                aliasEdges.AddRange(
                    targetEdges.Select(z => z.Alias(tableLookup[facetTable.TableOrUdfName], tableLookup[facetTable.Alias]))
                );

            }
            return aliasEdges.Where(x => !edges.Contains(x)).Distinct();
        }

        public GraphRoute ToRoute(IEnumerable<int> trail) => new GraphRoute(ToEdges(trail.Reverse()));

        public IEnumerable<TableRelation> ToEdges(IEnumerable<int> trail)
            => trail
                .Select(x => Registry.Tables.GetNode(x))
                .PairWise((a, b) => FindByName(a.TableOrUdfName, b.TableOrUdfName));

    }

    public static class EdgeRepositoryEagerBuilder
    {
        public static IQueryable<TableRelation> BuildEntity(this IQueryable<TableRelation> query)
        {
            return query.Include(x => x.SourceTable).Include(x => x.TargetTable);
        }
    }
}
