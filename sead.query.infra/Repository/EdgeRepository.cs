using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    using Route = List<TableRelation>;

    public class EdgeRepository(IRepositoryRegistry registry) : Repository<TableRelation, int>(registry), IEdgeRepository
    {

        public class EdgesLookup(Route edges)
        {
            public Dictionary<Tuple<string, string>, TableRelation> NameLookup { get; private set; } = edges.ToDictionary(z => z.Key);
            public Dictionary<Tuple<int, int>, TableRelation> IdLookup { get; private set; } = edges.ToDictionary(z => z.IdKey);
            public TableRelation GetEdge(string source, string target) => NameLookup[Tuple.Create(source, target)];
            public TableRelation GetEdge(int sourceId, int targetId) => IdLookup[Tuple.Create(sourceId, targetId)];
        }

        private Route __edges = null;
        public EdgesLookup __Lookup { get; private set; }


        public Route GetEdges(bool bidirectional = true)
        {
            if (__edges == null)
            {
                var edges = GetAll().ToList();

                if (bidirectional)
                {
                    edges.AddRange(edges.ReversedEdges());
                }
                var aliasNodes = Registry.FacetTables.FindThoseWithAlias();
                var aliasEdges = GetAliasedEdges(aliasNodes, edges);
                edges.AddRange(aliasEdges);

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

        public override Route GetAll()
        {
            return [.. Context.Set<TableRelation>().BuildEntity()];
        }

        // public TableRelation FindByName(string sourceName, string targetName)
        // {
        //     string[] names = [sourceName, targetName];
        //     return Context.Set<TableRelation>().BuildEntity()
        //         .Where(
        //             r => r.SourceName == sourceName && r.TargetName == targetName
        //         ).FirstOrDefault();
        // }

        private Route GetAliasedEdges(IEnumerable<FacetTable> aliases, Route edges)
        {
            var nodes = Registry.Tables.GetNodes();

            Route aliasEdges = [];

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
            return aliasEdges.Where(x => !edges.Contains(x)).Distinct().ToList();
        }

        // public Route ToRoute(IEnumerable<int> trail) => ToEdges(trail);

        // public Route ToEdges(IEnumerable<int> trail)
        //     => trail
        //         .Select(x => Registry.Tables.GetNode(x))
        //         .PairWise((a, b) => FindByName(a.TableOrUdfName, b.TableOrUdfName)).ToList();

    }

    public static class EdgeRepositoryEagerBuilder
    {
        public static IQueryable<TableRelation> BuildEntity(this IQueryable<TableRelation> query)
        {
            return query.Include(x => x.SourceTable).Include(x => x.TargetTable);
        }
    }
}
