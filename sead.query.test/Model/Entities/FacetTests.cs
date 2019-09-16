using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        public FacetTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private Facet CreateFacet()
        {
            return new Facet();
        }

        [Fact]
        public void Facet_FetchedFromRepository_ShouldBeComplete()
        {
            // Arrange
            using (var context = Infrastructure.Scaffolding.ScaffoldUtility.DefaultFacetContext()) {

                // Act
                var facet = context.Facets.Find(1);

                // Assert
                Assert.NotNull(facet);

                var expected = new Facet() {
                    FacetId = 1,
                    FacetCode = "result_facet",
                    DisplayTitle = "Analysis entities",
                    FacetGroupId = 999,
                    FacetTypeId = (EFacetType)1,
                    CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                    CategoryNameExpr = "tbl_physical_samples.sample_name||' '||tbl_datasets.dataset_name",
                    IconIdExpr = "tbl_analysis_entities.analysis_entity_id",
                    SortExpr = "tbl_datasets.dataset_name",
                    IsApplicable = false,
                    IsDefault = false,
                    AggregateType = "count",
                    AggregateTitle = "Number of samples",
                    AggregateFacetId = 0
                };

                Assert.Equal(expected.FacetId, facet.FacetId);
                Assert.Equal(expected.FacetCode, facet.FacetCode);
                Assert.Equal(expected.DisplayTitle, facet.DisplayTitle);
                Assert.Equal(expected.FacetGroupId, facet.FacetGroupId);
                Assert.Equal(expected.FacetTypeId, facet.FacetTypeId);
                Assert.Equal(expected.CategoryIdExpr, facet.CategoryIdExpr);
                Assert.Equal(expected.CategoryNameExpr, facet.CategoryNameExpr);
                Assert.Equal(expected.IconIdExpr, facet.IconIdExpr);
                Assert.Equal(expected.SortExpr, facet.SortExpr);
                Assert.Equal(expected.IsApplicable, facet.IsApplicable);
                Assert.Equal(expected.IsDefault, facet.IsDefault);
                Assert.Equal(expected.FacetCode, facet.FacetCode);
                Assert.Equal(expected.AggregateType, facet.AggregateType);
                Assert.Equal(expected.AggregateTitle, facet.AggregateTitle);
                Assert.Equal(expected.AggregateFacetId, facet.AggregateFacetId);

                Assert.NotNull(facet.Clauses);
                Assert.NotNull(facet.Tables);
                Assert.NotNull(facet.ExtraTables);
            }
        }


        [Fact]
        public void IsAffectedBy_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facet = this.CreateFacet();
            List<string> facetCodes = null;
            Facet targetFacet = null;

            // Act
            var result = facet.IsAffectedBy(
                facetCodes,
                targetFacet);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void CanGetFacetsWithAssociatedObjects()
        {
            var settings = new MockOptionBuilder().Build().Value;
            var context = new Mock<IFacetContext>().Object;

            var repositoryMock = new Mock<IFacetRepository>();

            repositoryMock.Setup(
                r => r.GetAll()
            ).Returns(
                new List<Facet>() { new Facet() }
            );

            var repository = repositoryMock.Object;

            List<Facet> facets = repository.GetAll().ToList();

            Assert.Equal(37, facets.Count);

            //FacetDefinition facet = repository.Get(14);

            //Assert.AreEqual(facet.FacetCode, "places");
            //Assert.AreEqual(facet.DisplayTitle, "Places");
            //Assert.IsNotNull(facet.FacetGroup);
            //Assert.IsNotNull(facet.TargetNode);
            //Assert.IsNotNull(facet.FacetType);
            //Assert.IsNotNull(facet.Tables);
            //Assert.IsTrue(facet.Tables.Count > 0);
            //Assert.AreEqual(facet.CategoryIdExpr, "tbl_locations.location_id");
            //Assert.AreEqual(facet.CategoryNameExpr, "tbl_locations.location_name");
            //Assert.AreEqual(facet.AggregateType, "count");
        }

        //[TestMethod]
        //public void CanGetAliasFacets()
        //{
        //    var repository = new FacetRepository(context);
        //    List<FacetDefinition> facets = repository.GetAll().ToList();
        //    FacetDefinition facet = repository.Get(21);
        //    Assert.AreEqual("country", facet.FacetCode);
        //    Assert.AreEqual("Country", facet.DisplayTitle);
        //    Assert.IsNotNull(facet.FacetGroup);
        //    Assert.IsNotNull(facet.TargetNode);
        //    Assert.IsNotNull(facet.FacetType);
        //    Assert.IsNotNull(facet.Tables);
        //    Assert.IsTrue(facet.Tables.Count > 0);

        //    List<FacetDefinition> aliasFacets = repository.FindThoseWithAlias().ToList();
        //    Assert.AreEqual(1, aliasFacets.Count);
        //    Assert.AreSame(facet, aliasFacets[0]);
        //}

        //[TestMethod]
        //public void CanGetFacetsFromStore()
        //{
        //    var facets = context.Facets
        //        .Include(x => x.FacetGroup)
        //        .Include(x => x.FacetType)
        //        .Include(x => x.Tables)
        //        .Include(x => x.Clauses)
        //        .ToList();
        //    Assert.IsTrue(facets.Count > 0);
        //}

    }
}
