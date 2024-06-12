using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore;

using Route = List<TableRelation>;

public class RouteFinder : IRouteFinder
{
    public IRepositoryRegistry Registry { get; private set; }
    public Route Edges { get; set; }

    public RouteFinder(IRepositoryRegistry registry, Route edges = null)
    {
        Registry = registry;
        Edges = edges ?? Registry.Relations.GetEdges();
    }

    public List<Route> Find(string start, List<string> destinations, bool reduce = true)
    {

        var routes = destinations.Where(z => z != start).Select(z => Find(start, z)).ToList();

        if (reduce)
        {
            return routes.ReduceEdges();
        }

        return routes;
    }

    public Route Find(string source, string destination)
    {
        var sourceNode = Registry.Tables.GetNode(source);
        var destinationNode = Registry.Tables.GetNode(destination);
        var route = Find(sourceNode.TableId, destinationNode.TableId);
        return route;
    }

    public Route Find(int source, int destination)
    {
        IEnumerable<int> trail = new DijkstrasGraph<int>(Edges.ToValueTuples()).FindShortestPath(source, destination);

        if (trail == null)
            throw new ArgumentOutOfRangeException($"No route found between {source} and {destination}");

        var route = ToEdges(trail);
        return route;
    }

    public List<int> Find2(int source, int destination)
    {
        return new DijkstrasGraph<int>(Edges.ToValueTuples()).FindShortestPath(source, destination);
    }

    public Route ToRoute(IEnumerable<int> trail) => ToEdges(trail);

    public Route ToEdges(IEnumerable<int> trail)
        => trail.PairWise((a, b) => Edges.Find(a, b)).ToList();
            // .Select(x => Registry.Tables.GetNode(x))

}
