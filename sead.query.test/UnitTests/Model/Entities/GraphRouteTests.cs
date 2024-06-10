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
        public IRouteFinder RouteFinder { get; private set; }

        public GraphRouteTests()
        {
            RouteFinder = FakeFacetGraphFactory.CreateSimpleFinder();
        }

        private GraphRoute CreateGraphRoute()
        {
            return new GraphRoute(
                new List<TableRelation>() {
                    RouteFinder.RelationLookup.GetRelation("A", "B"),
                    RouteFinder.RelationLookup.GetRelation("B", "F"),
                    RouteFinder.RelationLookup.GetRelation("F", "H")
                }
            );
        }

        [Fact]
        public void Contains_WhenAdded_EdgeExists()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            TableRelation item = RouteFinder.RelationLookup.GetRelation("A", "B");

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
            TableRelation item = RouteFinder.RelationLookup.GetRelation("C", "F");

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
                        RouteFinder.RelationLookup.GetRelation("F", "H")
                    }
                )
            };

            // Act
            var result = graphRoute.ReduceBy(routes);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.True(graphRoute.Contains(RouteFinder.RelationLookup.GetRelation("A", "B")));
            Assert.True(graphRoute.Contains(RouteFinder.RelationLookup.GetRelation("B", "F")));
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
