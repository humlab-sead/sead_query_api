using System;
using System.Collections.Generic;
using System.Text;

namespace QuerySeadDomain {

    public class DijkstrasGraph<N>
    {
        public Dictionary<N, Dictionary<N, int>> vertices { get; set; } = new Dictionary<N, Dictionary<N, int>>();

        public DijkstrasGraph()
        {
        }

        public DijkstrasGraph(Dictionary<N, Dictionary<N, int>> weights)
        {
            this.vertices = weights;
        }

        public void add_vertex(N name, Dictionary<N, int> edges)
        {
            vertices[name] = edges;
        }

        public List<N> shortest_path(N start, N finish)
        {
            var previous = new Dictionary<N, N>();
            var distances = new Dictionary<N, int>();
            var nodes = new List<N>();

            List<N> path = null;

            foreach (var vertex in vertices) {
                if (vertex.Key.Equals(start)) {
                    distances[vertex.Key] = 0;
                } else {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0) {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest.Equals(finish)) {
                    path = new List<N>();
                    while (previous.ContainsKey(smallest)) {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue) {
                    break;
                }

                foreach (var neighbor in vertices[smallest]) {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key]) {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }

}
