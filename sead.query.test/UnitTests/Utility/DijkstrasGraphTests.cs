using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using SQT.Mocks;

namespace SQT.Infrastructure
{
    public class DijkstrasGraphTests
    {

        public static IEnumerable<object[]> TestGraphData()
        {
            var g1 = new DijkstrasGraph<char>(FakeGraphFactory.EdgesAsValueTuples());
            var g2 = new DijkstrasGraph<char>(FakeGraphFactory.EdgesAsDictionary());
            return [
                [g1, 'A', 'H', new List<char> {'A', 'B', 'F', 'H'}, true, true],
                [g1, 'E', 'H', new List<char> {'E', 'H'}, true, true],
                [g2, 'A', 'H', new List<char> {'A', 'B', 'F', 'H'}, true, true],
                [g2, 'E', 'H', new List<char> {'E', 'H'}, true, true],
                [g2, 'A', 'A', new List<char> {'A'}, true, true],
                [g1, 'A', 'Z', null, true, false],
                [g1, 'A', 'H', new List<char> {'H', 'F', 'B', 'A'}, false, true],
                [g1, 'A', 'A', new List<char> {'A'}, false, true],

            ];
        }

        [Theory]
        [MemberData(nameof(TestGraphData))]
        public void TestDijkstrasWithVariousSetups(DijkstrasGraph<char> g, char start, char stop, List<char> expected, bool reverse, bool onNotFoundThrow)
        {
            List<char> route = g.FindShortestPath(start, stop, reverse: reverse, onNotFoundThrow: onNotFoundThrow);

            if (route is null)
                Assert.True(expected is null);
            else
                Assert.True(route.SequenceEqual(expected));
        }


        [Fact]
        public void TestDijkstrasNoRouterThrowsException()
        {
            var g = new DijkstrasGraph<char>(FakeGraphFactory.EdgesAsValueTuples());
            var exception = Assert.Throws<NoRouteFoundException>(() => g.FindShortestPath('A', 'Z'));
        }

        [Fact]
        public void TestDijkstrasEmptyGraphThrowsException()
        {
            var g = new DijkstrasGraph<char>();
            var exception = Assert.Throws<EmptyGraphException>(() => g.FindShortestPath('A', 'B'));
        }


        [Fact]
        public void TestDijkstrasFindShortestPaths()
        {
            var g = new DijkstrasGraph<char>(FakeGraphFactory.EdgesAsValueTuples());
            var routes = g.FindShortestPaths('A', ['A', 'H'], true, true);

            var expected = new List<List<char>>() {
                new List<char> {'A'},
                new List<char> {'A', 'B', 'F', 'H'}
            };
            Assert.True(routes.Count == expected.Count);
            routes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void TestAddVertex()
        {
            var dijkstrasGraph = new DijkstrasGraph<char>();

            const char source = 'A';
            var neighbours = new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } };
            dijkstrasGraph.add_vertex(source, neighbours);

            var storedNeighbours = dijkstrasGraph.EdgeDict.GetValueOrDefault(source);

            Assert.NotNull(storedNeighbours);
            Assert.Equal(7, storedNeighbours.GetValueOrDefault('B'));
            Assert.Equal(8, storedNeighbours.GetValueOrDefault('C'));
        }

    }
}
