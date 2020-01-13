using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class GraphRouteTests : FacetTestBase
    {
        public IFacetsGraph FacetsGraph { get; private set; }

        public GraphRouteTests()
        {
            FacetsGraph = MockFacetGraphGenerator.CreateSimpleGraph();

        }

        private GraphRoute CreateGraphRoute()
        {
            GraphRoute testRoute = new GraphRoute(
                new List<TableRelation>() {
                    FacetsGraph.GetEdge("A", "B"),
                    FacetsGraph.GetEdge("B", "F"),
                    FacetsGraph.GetEdge("F", "H")
                }
            );
            return testRoute;
        }

        [Fact]
        public void Contains_WhenAdded_EdgeExists()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            TableRelation item = FacetsGraph.GetEdge("A", "B");

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
            TableRelation item = FacetsGraph.GetEdge("C", "F");

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
            List<GraphRoute> routes = new List<GraphRoute>() {
                new GraphRoute(
                    new List<TableRelation>() {
                        FacetsGraph.GetEdge("F", "H")
                    }
                )
            };

            // Act
            var result = graphRoute.ReduceBy(
                routes);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.True(graphRoute.Contains(FacetsGraph.GetEdge("A", "B")));
            Assert.True(graphRoute.Contains(FacetsGraph.GetEdge("B", "F")));
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
            Assert.Equal(expected, result );
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
