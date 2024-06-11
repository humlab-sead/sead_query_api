using Moq;
using SeadQueryCore;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SQT.Model
{
    public class GraphRouteTests
    {
        public IEnumerable<TableRelation> Edges { get; private set; }

        public GraphRouteTests()
        {
            Edges = FakeGraphFactory.CreateSimpleGraph();
        }

        private GraphRoute CreateGraphRoute()
        {
            return new GraphRoute(
                [
                    Edges.GetEdge("A", "B"),
                    Edges.GetEdge("B", "F"),
                    Edges.GetEdge("F", "H")
                ]
            );
        }

        [Fact]
        public void Contains_WhenAdded_EdgeExists()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            TableRelation item = Edges.GetEdge("A", "B");

            // Act
            var result = graphRoute.Contains(item);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public void Contains_WhenNotAdded_EdgeDoesNotExists()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            TableRelation item = Edges.GetEdge("C", "F");

            // Act
            var result = graphRoute.Contains(item);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void ReduceBy_ExistingItem_ItemRemoved()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            List<GraphRoute> routes = new() {
                new GraphRoute(
                    new List<TableRelation>() {
                        Edges.GetEdge("F", "H")
                    }
                )
            };

            // Act
            var result = graphRoute.ReduceBy(routes);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.True(graphRoute.Contains(Edges.GetEdge("A", "B")));
            Assert.True(graphRoute.Contains(Edges.GetEdge("B", "F")));
        }

        [Fact]
        public void ToString_OfAnyGraph_ReturnsCsvString()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            const string expected = "A;B;7\nB;F;2\nF;H;3";
            // Act
            var result = graphRoute.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Trail_OfAnyGraph_ReturnListOfNodesNamesInTrail()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            var expected = "A-B-F-H".Split('-');

            // Act
            var result = graphRoute.Trail();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
