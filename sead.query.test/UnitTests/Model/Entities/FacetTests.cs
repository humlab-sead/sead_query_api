using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.Model
{
    [Collection("JsonSeededFacetContext")]
    public class FacetTests : DisposableFacetContextContainer
    {

        public FacetTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private Facet CreateFacet(string facetCode)
        {
            return Registry.Facets.GetByCode(facetCode);
        }

        [Fact]
        public void Find_FromRepository_IsComplete()
        {
            // FIXME: Remove (dummy test)
            // Arrange
            var facet = CreateFacet("result_facet");
            var expected = new Facet() {
                FacetId = 1,
                FacetCode = "result_facet",
                DisplayTitle = "Analysis entities",
                FacetGroupId = 999,
                FacetTypeId = (EFacetType)1,
                CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                CategoryNameExpr = "tbl_physical_samples.sample_name||' '||tbl_datasets.dataset_name",
                SortExpr = "tbl_datasets.dataset_name",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 0
            };

            // Assert
            Asserter.EqualByProperty(facet, expected);
        }

        [Fact]
        public void QueryCriteria_WhenFacetHasNoClause_IsEmpty()
        {
            // Arrange
            var facet = CreateFacet("sites");
            facet.Clauses = new List<FacetClause>();

            // Act
            var result = facet.Criteria;

            Assert.Equal("", result);
        }

        [Fact]
        public void QueryCriteria_WhenFacetHasOneClause_IsSameAsClause()
        {
            // Arrange
            var facet = CreateFacet("sites");
            var mockClause = new Mock<FacetClause>();
            mockClause.Setup(x => x.Clause).Returns("A = 1");
            facet.Clauses = new List<FacetClause>() { mockClause.Object };

            // Act
            var result = facet.Criteria;

            Assert.Equal("A = 1", result);
        }

        [Fact]
        public void QueryCriteria_WhenFacetHasTwoClause_IsNotEmpty()
        {
            // Arrange
            var facet = CreateFacet("sites");

            var mockClause1 = new Mock<FacetClause>();
            mockClause1.Setup(x => x.Clause).Returns("A = 1");

            var mockClause2 = new Mock<FacetClause>();
            mockClause2.Setup(x => x.Clause).Returns("B = 2");

            facet.Clauses = new List<FacetClause>() { mockClause1.Object, mockClause2.Object };

            // Act
            var result = facet.Criteria;

            Assert.Equal("A = 1 AND B = 2", result);
        }


        [Fact]
        public void AliasName_WhenFacetHasNoAlias_IsEmpty()
        {
            // Arrange
            var facet = CreateFacet("sites");

            // Act
            var result = facet.Criteria;

            Assert.Equal("", result);
        }

        [Fact]
        public void AliasName_WhenFacetHasAlias_IsNotEmpty()
        {
            var facet = CreateFacet("country");
            var result = facet.TargetTable.Alias;
            Assert.Equal("countries", result);
            Assert.True(facet.TargetTable.HasAlias);
        }

        [Fact]
        public void ExtraTables_WhenFacetHasOnlyOneTable_IsEmpty()
        {
            // Arrange
            var facet = CreateFacet("sites");
            var mockTable = new Mock<FacetTable>();
            mockTable.Setup(x => x.SequenceId).Returns(1);
            facet.Tables = new List<FacetTable>() { mockTable.Object };

            // Act
            var result = facet.Tables.Where(z => z.SequenceId > 1);

            Assert.Empty(result);
        }

        [Fact]
        public void ExtraTables_WhenFacetHasTwoTable_IsSingle()
        {
            // Arrange
            var facet = CreateFacet("sites");
            var mockTable1 = new Mock<FacetTable>();
            mockTable1.Setup(x => x.SequenceId).Returns(1);
            var mockTable2 = new Mock<FacetTable>();
            mockTable2.Setup(x => x.SequenceId).Returns(2);
            facet.Tables = new List<FacetTable>() {
                mockTable1.Object, mockTable2.Object
            };

            // Act
            var result = facet.Tables.Where(z => z.SequenceId > 1);

            Assert.Single(result);
        }

    }
}
