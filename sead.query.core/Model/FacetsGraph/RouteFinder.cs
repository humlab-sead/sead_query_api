using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore;

using Route = List<TableRelation>;
using Graph = List<TableRelation>;
using Nodes = Dictionary<string, Table>;

public interface IDefaultGraphFactory
{
    Graph CreateGraph();
}

public class DefaultGraphFactory: IDefaultGraphFactory
{
    private IRepositoryRegistry Registry;
    public DefaultGraphFactory(IRepositoryRegistry registry)
    {
        Registry = registry;
    }

    public Graph CreateGraph()
    {
        return Registry.Relations.GetEdges();
    }
}

public class PathFinder : IPathFinder
{

    public Graph Graph { get; set; }
    public Nodes Nodes { get; private set; }

    public PathFinder(IDefaultGraphFactory factory)
    {
        Graph = factory.CreateGraph();
        Nodes = Graph.GetNodes();
    }

    public PathFinder(Graph edges)
    {
        Graph = edges;
        Nodes = edges.GetNodes();
    }

    public PathFinder(Graph edges, Nodes nodes) : this(edges)
    {
        Nodes = nodes;
    }

    public List<Route> Find(string start, List<string> targets, bool reduce = true)
    {
        var routes = targets.Where(z => z != start).Select(z => Find(start, z)).ToList();

        return reduce ? routes.ReduceEdges() : routes;
    }

    public Route Find(string source, string target)
    {
        var sourceNode = Nodes[source];
        var destinationNode = Nodes[target];
        var route = Find(sourceNode.TableId, destinationNode.TableId);
        return route;
    }

    public Route Find(int source, int target)
    {
        IEnumerable<int> trail = new DijkstrasGraph<int>(Graph.ToValueTuples()).FindShortestPath(source, target);

        if (trail == null)
            throw new ArgumentOutOfRangeException($"No route found between {source} and {target}");

        var route = ToEdges(trail);
        return route;
    }

    public Route ToRoute(IEnumerable<int> trail) => ToEdges(trail);

    public Route ToEdges(IEnumerable<int> trail)
        => trail.PairWise((a, b) => Graph.GetEdge(a, b)).ToList();

}
