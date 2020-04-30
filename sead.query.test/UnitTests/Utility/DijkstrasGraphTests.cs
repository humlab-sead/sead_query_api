using SeadQueryCore;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace SQT.Infrastructure
{
    public class DijkstrasGraphTests
    {
        private DijkstrasGraph<char> CreateDijkstrasGraph()
        {
            var g = new DijkstrasGraph<char>();
            return g;
        }

        public static IEnumerable<object[]> TestGraphData()
        {
            var g = new DijkstrasGraph<char>();
            g.add_vertex('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } });
            g.add_vertex('B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } });
            g.add_vertex('C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } });
            g.add_vertex('D', new Dictionary<char, int>() { { 'F', 8 } });
            g.add_vertex('E', new Dictionary<char, int>() { { 'H', 1 } });
            g.add_vertex('F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } });
            g.add_vertex('G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 9 } });
            g.add_vertex('H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } });
            return new[] {
                new object[] { g, 'A', 'H', 3 }
            };
        }

        [Theory]
        [MemberData(nameof(TestGraphData))]
        public void TestDijkstras(SeadQueryCore.DijkstrasGraph<char> g, char start, char stop, int expected)
        {
            List<char> route = g.shortest_path(start, stop);
            Assert.Equal<int>(expected, route.Count);
            route.ForEach(x => Debug.Write(x));
        }

        [Fact]
        public void add_vertex_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dijkstrasGraph = this.CreateDijkstrasGraph();

            // Act
            var source = 'A';
            var neighbours = new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } };
            dijkstrasGraph.add_vertex(source, neighbours);

            // Assert
            var storedNeighbours = dijkstrasGraph.Vertices.GetValueOrDefault(source);

            Assert.NotNull(storedNeighbours);
            Assert.Equal(7, storedNeighbours.GetValueOrDefault('B'));
            Assert.Equal(8, storedNeighbours.GetValueOrDefault('C'));
        }

        [Theory]
        [MemberData(nameof(TestGraphData))]
        public void shortest_path_StateUnderTest_ExpectedBehavior(DijkstrasGraph<char> g, char start, char stop, int expected)
        {
            // Act
            var result = g.shortest_path(start, stop);

            // Assert
            Assert.Equal<int>(expected, result.Count);
        }
    }
}
