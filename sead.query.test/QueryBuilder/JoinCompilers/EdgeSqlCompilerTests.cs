using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.JoinCompilers
{
    public class EdgeSqlCompilerTests
    {
        private EdgeSqlCompiler CreateEdgeSqlCompiler()
        {
            return new EdgeSqlCompiler();
        }

        [Fact]
        public void Compile_WithSingleEdge_ReturnSingleJoin()
        {
            // Arrange
            var edgeSqlCompiler = this.CreateEdgeSqlCompiler();

            TableRelation edge = new TableRelation() {
                TableRelationId = -2151,
                SourceTableId = 46,
                TargetTableId = 113,
                Weight = 5,
                SourceColumName = "location_id",
                TargetColumnName = "location_id",
                SourceTable = new Table() { TableId = 46, TableOrUdfName = "countries"},
                TargetTable = new Table() { TableId = 113, TableOrUdfName = "tbl_site_locations"}
            };

            FacetTable facetTable = new FacetTable {
                FacetTableId = 1,
                FacetId = 1,
                SequenceId = 1,
                TableId = edge.TargetTableId,
                Table = edge.TargetTable,
                UdfCallArguments = null,
                Alias = ""
            };

            // Act
            var result = edgeSqlCompiler.Compile(edge, facetTable, false);

            // Assert
            var expected = "left join tbl_site_locations on tbl_site_locations.\"location_id\" = countries.\"location_id\"";
            Assert.Equal(expected, result.ToLower().Trim());
        }

        [Fact]
        public void Compile_WithSingleEdgeWithWithoutAliasAndNoUdf_ReturnSingleJoinWithNoAlias()
        {
            // Arrange
            var edgeSqlCompiler = this.CreateEdgeSqlCompiler();

            TableRelation edge = new TableRelation() {
                TableRelationId = -2151,
                SourceTableId = 1,
                TargetTableId = 2,
                Weight = 5,
                SourceColumName = "site_id",
                TargetColumnName = "site_id",
                SourceTable = new Table() { TableId = 1, TableOrUdfName = "tbl_sites"},
                TargetTable = new Table() { TableId = 2, TableOrUdfName = "tbl_site_locations"}
            };

            FacetTable facetTable = new FacetTable {
                FacetTableId = 1,
                FacetId = 1,
                SequenceId = 1,
                TableId = edge.TargetTableId,
                Table = edge.TargetTable,
                UdfCallArguments = null,
                Alias = ""
            };

            // Act
            var result = edgeSqlCompiler.Compile(edge, facetTable, false);

            // Assert
            var expected = "left join tbl_site_locations on tbl_site_locations.\"site_id\" = tbl_sites.\"site_id\"";
            Assert.Equal(expected, result.ToLower().Trim());
        }
    }
}
