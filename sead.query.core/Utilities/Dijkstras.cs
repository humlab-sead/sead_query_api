using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{

    public class DijkstrasGraph<N>
    {
        public Dictionary<N, Dictionary<N, int>> EdgeDict { get; set; } = [];

        public DijkstrasGraph()
        {
        }

        public DijkstrasGraph(Dictionary<N, Dictionary<N, int>> weights)
        {
            EdgeDict = weights;
        }

        public DijkstrasGraph(IEnumerable<Tuple<N, N, int>> edges)
        {
            EdgeDict = ToWeightGraph(edges);
        }

        public void add_vertex(N name, Dictionary<N, int> edges)
        {
            EdgeDict[name] = edges;
        }

        public Dictionary<N, Dictionary<N, int>> ToWeightGraph(IEnumerable<Tuple<N, N, int>> edges)
        {
            return edges.GroupBy(p => p.Item1, (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.Item2, x => x.Item3)))
                .ToDictionary(x => x.SourceId, y => y.TargetWeights);
            // var graph = new Dictionary<N, Dictionary<N, int>>();
            // foreach (var edge in edges) {
            //     if (!graph.ContainsKey(edge.Item1)) {
            //         graph[edge.Item1] = [];
            //     }
            //     graph[edge.Item1][edge.Item2] = edge.Item3;
            // }
            // return graph;
        }

        public Dictionary<N, Dictionary<N, int>> ToWeightGraph(IEnumerable<TableRelation> edges)
        {
            return ToWeightGraph((IEnumerable<Tuple<N, N, int>>)edges.ToTuples());
        }

        public List<N> FindShortestPath(N start, N finish)
        {
            var previous = new Dictionary<N, N>();
            var distances = new Dictionary<N, int>();
            var nodes = new List<N>();

            List<N> path = null;

            foreach (var vertex in EdgeDict)
            {
                if (vertex.Key.Equals(start))
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
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

                foreach (var neighbor in EdgeDict[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }
}
