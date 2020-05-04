using System.Collections.Generic;
using Moq;
using SeadQueryCore;
using System;
using Xunit;
using System.Text.RegularExpressions;

namespace SQT.SqlCompilers
{
    public class JoinSqlCompilerTests
    {
        private JoinSqlCompiler CreateEdgeSqlCompiler()
        {
            return new JoinSqlCompiler();
        }
        private static Mock<IFacetsGraph> MockFacetGraph()
        {
            var facetGraphMock = new Mock<IFacetsGraph>();
            facetGraphMock.Setup(x => x.AliasedFacetTables).Returns(new List<FacetTable>());
            return facetGraphMock;
        }

        [Fact]
        public void Compile_WithSingleEdge_ReturnSingleJoin()
        {
            // Arrange

            TableRelation edge = new TableRelation() {
                TableRelationId = -2151,
                SourceTableId = 46,
                TargetTableId = 113,
                Weight = 5,
                SourceColumName = "location_id",
                TargetColumnName = "location_id",
                SourceTable = new Table() { TableId = 46, TableOrUdfName = "countries" },
                TargetTable = new Table() { TableId = 113, TableOrUdfName = "tbl_site_locations" }
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
            var edgeSqlCompiler = new JoinSqlCompiler();
            var result = edgeSqlCompiler.Compile(edge, facetTable, false);

            // Assert
            var expected = "left join tbl_site_locations on tbl_site_locations.\"location_id\" = countries.\"location_id\"";
            Assert.Equal(expected, result.ToLower().Trim());
        }

        [Fact]
        public void Compile_WithSingleEdgeWithWithoutAliasAndNoUdf_ReturnSingleJoinWithNoAlias()
        {
            // Arrange

            TableRelation edge = new TableRelation() {
                TableRelationId = -2151,
                Weight = 5,
                SourceColumName = "a",
                TargetColumnName = "a",
                SourceTable = new Table() { TableId = 1, TableOrUdfName = "A"},
                TargetTable = new Table() { TableId = 2, TableOrUdfName = "B"}
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
            var edgeSqlCompiler = new JoinSqlCompiler();
            var result = edgeSqlCompiler.Compile(edge, facetTable, false);

            // Assert
            var expected = @"\s*left\s+join\s+b\s+on\s+b\.""a""\s+=\s+a\.""a""\s*";
            Assert.Matches(expected, result.ToLower());
        }
    }
}
