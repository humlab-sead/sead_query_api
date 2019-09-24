using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class GraphEdgeTests : FacetTestBase
    {
        private GraphEdge CreateGraphEdge()
        {
            return new GraphEdge() {
                EdgeId = 1,
                SourceNode = new GraphNode() { NodeId = 1, TableName = "A" },
                TargetNode = new GraphNode() { NodeId = 2, TableName = "B" },
                Weight = 6,
                SourceKeyName = "a_b",
                TargetKeyName = "a_b"
            };
        }

        [Fact]
        public void Clone_OfAnyEdge_HasSameState()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();

            // Act
            var result = graphEdge.Clone();

            // Assert
            Asserter.EqualByProperty( graphEdge, result);
        }

        [Fact]
        public void Reverse_OfAnyGraph_SwitchesNodes()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();

            // Act
            var result = graphEdge.Reverse();

            // Assert
            Assert.Equal(result.SourceName, graphEdge.TargetName);
            Assert.Equal(result.SourceNode, graphEdge.TargetNode);
            Assert.Equal(result.TargetName, graphEdge.SourceName);
            Assert.Equal(result.TargetNode, graphEdge.SourceNode);
            Assert.Equal(result.SourceKeyName, graphEdge.TargetKeyName);
            Assert.Equal(result.TargetKeyName, graphEdge.SourceKeyName);
            Assert.Equal(result.Weight, graphEdge.Weight);
        }

        [Fact]
        public void Alias_OfAnyGraph_ReplacesNodeWithSameId()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();
            GraphNode node = graphEdge.TargetNode;
            GraphNode alias = new GraphNode { NodeId = node.NodeId, TableName = "ALIAS" };

            // Act
            var result = graphEdge.Alias(node, alias);

            // Assert
            Assert.Equal("ALIAS", result.TargetNode.TableName);
        }

        [Fact]
        public void Equals_OfIdenticalEdges_IsTrue()
        {
            // Arrange
            var graphEdge1 = this.CreateGraphEdge();
            var graphEdge2 = this.CreateGraphEdge();

            // Act
            var result = graphEdge1.Equals(graphEdge1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ToStringPair_OfAnyEdge_ReturnsPair()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();

            // Act
            var result = graphEdge.ToStringPair();

            // Assert
            Assert.Equal($"{graphEdge.SourceName}/{graphEdge.TargetName}", result);
        }

        public static List<object[]> TestData = new List<object[]>() {
             new object[] {
                typeof(GraphEdge),
                5,
                new Dictionary<string, object>() {
                    { "EdgeId", 5 },
                    { "SourceNodeId", 44 },
                    { "TargetNodeId", 142 },
                    { "Weight", 20 },
                    { "SourceKeyName", "modification_type_id" },
                    { "TargetKeyName", "modification_type_id" }
                }
            }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void Find_FromRepository_IsComplete(Type type, object id, Dictionary<string, object> expected)
        {
            // Arrange
            using (var context = ScaffoldUtility.DefaultFacetContext()) {
                // Act
                var entity = context.Find(type, new object[] { id });
                // Assert
                Assert.NotNull(entity);
                Asserter.EqualByDictionary(type, expected, entity);
            }
        }
    }
}
