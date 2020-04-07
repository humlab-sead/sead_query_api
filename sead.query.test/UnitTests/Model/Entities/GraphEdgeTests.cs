using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class GraphEdgeTests
    {
        private TableRelation CreateGraphEdge()
        {
            return new TableRelation() {
                TableRelationId = 1,
                SourceTable = new Table() { TableId = 1, TableOrUdfName = "A" },
                TargetTable = new Table() { TableId = 2, TableOrUdfName = "B" },
                Weight = 6,
                SourceColumName = "a_b",
                TargetColumnName = "a_b"
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
            Assert.Equal(result.SourceTable, graphEdge.TargetTable);
            Assert.Equal(result.TargetName, graphEdge.SourceName);
            Assert.Equal(result.TargetTable, graphEdge.SourceTable);
            Assert.Equal(result.SourceColumName, graphEdge.TargetColumnName);
            Assert.Equal(result.TargetColumnName, graphEdge.SourceColumName);
            Assert.Equal(result.Weight, graphEdge.Weight);
        }

        [Fact]
        public void Alias_OfAnyGraph_ReplacesNodeWithSameId()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();
            Table node = graphEdge.TargetTable;
            Table alias = new Table { TableId = node.TableId, TableOrUdfName = "ALIAS" };

            // Act
            var result = graphEdge.Alias(node, alias);

            // Assert
            Assert.Equal("ALIAS", result.TargetTable.TableOrUdfName);
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

    }
}
