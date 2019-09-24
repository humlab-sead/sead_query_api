using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure.Scaffolds;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetConfig2Tests
    {
 
        [Fact]
        public void HasPicks_WhenRangeFacetHasPicks_IsTrue()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2 {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateLowerUpper(3M, 52M)
            };
            // Act
            var result = facetConfig2.HasPicks();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasPicks_WhenFacetHasNoPicks_IsFalse()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2 {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = new List<FacetConfigPick>()
            };
            // Act
            var result = facetConfig2.HasPicks();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPicks_WhenDiscreteFacetHasPicks_IsTrue()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2 {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateDiscrete(new List<int>() {  1, 2, 3 })
            };
            // Act
            var result = facetConfig2.HasPicks();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ClearPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2 {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateDiscrete(new List<int>() { 1, 2, 3 })
            };

            // Act
            facetConfig2.ClearPicks();

            // Assert
            Assert.False(facetConfig2.HasPicks());
        }

        [Fact]
        public void GetPickValues_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2 {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateDiscrete(new List<int>() { 1, 2, 3 })
            };
            bool sort = false;

            // Act
            var result = facetConfig2.GetPickValues(sort);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void GetJoinTables_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facet = new Facet() {
                FacetId = 1,
                FacetCode = "result_facet",
                DisplayTitle = "Analysis entities",
                CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                CategoryNameExpr = "tbl_physical_samples.sample_name||\' \'||tbl_datasets.dataset_name",
                AggregateType = "count",
                AggregateTitle = "Number of dummies",
                AggregateFacetId = 0,
                SortExpr = "tbl_dummy.dummy_name",
                FacetType = new FacetType {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new Mock<FacetGroup>().Object,
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 1,
                        FacetId = 1,
                        SequenceId = 1,
                        ObjectName = "tbl_analysis_entities",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 38,
                        FacetId = 1,
                        SequenceId = 2,
                        ObjectName = "tbl_physical_samples",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 53,
                        FacetId = 1,
                        SequenceId = 3,
                        ObjectName = "tbl_datasets",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> { }
            };
            var facetConfig2 = new FacetConfig2 {
                FacetCode = "result_facet",
                Facet = facet,
                Position = 0,
                Picks = FacetConfigPick.CreateDiscrete(new List<int>() { 1, 2, 3 })
            };
            // Act
            var result = facetConfig2.GetJoinTables();

            // Assert
            var expected = new List<string>() { "tbl_physical_samples", "tbl_datasets" };
            Assert.Equal(expected, result);
        }
    }
}
