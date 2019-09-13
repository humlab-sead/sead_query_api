using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.JoinCompilers
{
    public class EdgeSqlCompilerTests : IDisposable
    {
        public EdgeSqlCompilerTests()
        {

        }

        public void Dispose()
        {
        }

        private EdgeSqlCompiler CreateEdgeSqlCompiler()
        {
            return new EdgeSqlCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var edgeSqlCompiler = this.CreateEdgeSqlCompiler();

            var graphMock = new Mock<IFacetsGraph>();
            graphMock.Setup(g => g.ResolveTargetName(It.IsAny<string>())).Returns("tbl_site_locations");
            graphMock.Setup(g => g.ResolveAliasName(It.IsAny<string>())).Returns(default(string));

            GraphEdge edge = new GraphEdge() {
                EdgeId = -2151,
                SourceNodeId = 46,
                TargetNodeId = 113,
                Weight = 5,
                SourceKeyName = "location_id",
                TargetKeyName = "location_id",
                SourceNode = new GraphNode() { NodeId = 46, TableName = "countries"},
                TargetNode = new GraphNode() { NodeId = 113, TableName = "tbl_site_locations"}
            };

            bool innerJoin = false;

            // Act
            var result = edgeSqlCompiler.Compile(
                graphMock.Object,
                edge,
                innerJoin);

            // Assert
            var expected = " LEFT JOIN tbl_site_locations  ON tbl_site_locations.\"location_id\" = countries.\"location_id\" ";
            Assert.Equal(expected, result);
        }
    }
}
