using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore;

using WeightDictionary = Dictionary<int, Dictionary<int, int>>;

public class RouteFinder(IRepositoryRegistry registry) : IRouteFinder
{
    public IRepositoryRegistry Registry { get; private set; } = registry;

    public WeightDictionary ToWeightGraph()
    {
        return Registry.Relations.GetEdges()
            .GroupBy(p => p.SourceId, (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.TargetId, x => x.Weight)))
                .ToDictionary(x => x.SourceId, y => y.TargetWeights);
    }

    public List<GraphRoute> Find(string start_table, List<string> destination_tables, bool reduce = true)
    {
        // Make sure that start table doesn't exists in list of destination tables...
        destination_tables = destination_tables.Where(z => z != start_table).ToList();

        var routes = destination_tables.Select(z => Find(start_table, z)).ToList();

        if (reduce)
        {
            return GraphUtility.Reduce(routes);
        }

        return routes;
    }

    public GraphRoute Find(string source, string destination)
    {
        return Find(
            Registry.Tables.GetNode(source).TableId,
            Registry.Tables.GetNode(destination).TableId
        );
    }

    public GraphRoute Find(int startId, int destId)
    {
        IEnumerable<int> trail = new DijkstrasGraph<int>(ToWeightGraph())
            .shortest_path(startId, destId);

        if (trail == null)
            throw new ArgumentOutOfRangeException($"No route found between {startId} and {destId}");

        return Registry.Relations.ToRoute(trail.Concat([startId]));
    }

    public FacetTable GetAliasTable(string aliasName) => Registry.FacetTables.GetByAlias(aliasName);
}
