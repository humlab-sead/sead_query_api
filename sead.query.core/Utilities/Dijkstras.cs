using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class NoRouteFoundException : ArgumentOutOfRangeException
    {
        public NoRouteFoundException(string message)
            : base(message) { }
    }

    public class EmptyGraphException : ArgumentOutOfRangeException
    {
        public EmptyGraphException(string message)
            : base(message) { }
    }

    public static class GraphHelper
    {
        public static Dictionary<N, Dictionary<N, int>> ToWeightGraph<N>(IEnumerable<Tuple<N, N, int>> edges)
        {
            return ToWeightGraph(edges.Select(x => (x.Item1, x.Item2, x.Item3)));
        }

        public static Dictionary<N, Dictionary<N, int>> ToWeightGraph<N>(IEnumerable<(N, N, int)> edges)
        {
            return edges
                .GroupBy(p => p.Item1, (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.Item2, x => x.Item3)))
                .ToDictionary(x => x.SourceId, y => y.TargetWeights);
        }
    }

    public class DijkstrasGraph<N>
    {
        public Dictionary<N, Dictionary<N, int>> EdgeDict { get; set; } = [];

        public DijkstrasGraph() { }

        public DijkstrasGraph(Dictionary<N, Dictionary<N, int>> weights)
        {
            EdgeDict = weights;
        }

        public DijkstrasGraph(IEnumerable<(N, N, int)> edges)
        {
            EdgeDict = GraphHelper.ToWeightGraph(edges);
        }

        public DijkstrasGraph(IEnumerable<Tuple<N, N, int>> edges)
        {
            EdgeDict = GraphHelper.ToWeightGraph(edges);
        }

        public void add_vertex(N name, Dictionary<N, int> edges)
        {
            EdgeDict[name] = edges;
        }

        public List<N> FindShortestPath(N start, N finish, bool onNotFoundThrow = true, bool reverse = true)
        {
            var previous = new Dictionary<N, N>();
            var distances = new Dictionary<N, int>();
            var nodes = new List<N>();

            if (EdgeDict.Count == 0)
                throw new EmptyGraphException("Graph is empty");

            List<N> path = null;

            var allNodes = EdgeDict.Keys.Union(EdgeDict.Values.SelectMany(edges => edges.Keys)).Distinct();

            foreach (var vertex in allNodes)
            {
                if (vertex.Equals(start))
                {
                    distances[vertex] = 0;
                }
                else
                {
                    distances[vertex] = int.MaxValue;
                }

                nodes.Add(vertex);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest.Equals(finish))
                {
                    path = [];
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }
                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                if (!EdgeDict.TryGetValue(smallest, out var adjacentNodes))
                {
                    continue;
                }

                foreach (var neighbor in adjacentNodes)
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            if (path == null)
            {
                if (onNotFoundThrow)
                    throw new NoRouteFoundException($"No route found between {start} and {finish}");
                return null;
            }

            path.Add(start);

            if (reverse)
                path.Reverse();

            return path;
        }

        public List<List<N>> FindShortestPaths(N start, List<N> destinations, bool onNotFoundThrow = true, bool reverse = true)
        {
            return destinations.Select(d => FindShortestPath(start, d, onNotFoundThrow, reverse)).ToList();
        }
    }
}
