using DataAccessPostgreSqlProvider;
using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class FacetContextTests : FacetTestBase, IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IQueryBuilderSetting> mockQueryBuilderSetting;

        public FacetContextTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockQueryBuilderSetting = this.mockRepository.Create<IQueryBuilderSetting>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetContext CreateFacetContext()
        {
            return new FacetContext(
                this.mockQueryBuilderSetting.Object);
        }

        [Fact(Skip ="Not implemented")]
        public void SaveChanges_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetContext = this.CreateFacetContext();

            // Act
            var result = facetContext.SaveChanges();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void Context_Should_Have_Values_For_All_Entity_Types()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                foreach (Type type in ScaffoldUtility.GetModelTypes()) {
                    var g = GetGenericMethodForType<FacetContext>("Set", type);
                    var entities = (IEnumerable<object>)g.Invoke(context, new object[] { });
                    Assert.True(entities.ToList().Count > 0);
                }
            }
        }

        [Fact]
        public void ShouldBeAbleToFetchFacetAndReferenceObjects()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var repository = new FacetRepository(context);

                Facet facet = repository.Get(25);

                Dictionary<string, object> expectedProperties = new Dictionary<string, object>() {
                    { "FacetId", 25 },
                    { "FacetCode", "species" },
                    { "DisplayTitle", "Taxa" },
                    { "FacetGroupId", 6 },
                    { "FacetTypeId", EFacetType.Discrete },
                    { "CategoryIdExpr", "tbl_taxa_tree_master.taxon_id" },
                    { "CategoryNameExpr", "concat_ws(' ', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)" },
                    { "IconIdExpr", "tbl_taxa_tree_master.taxon_id" },
                    { "SortExpr", "tbl_taxa_tree_genera.genus_name||' '||tbl_taxa_tree_master.species" },
                    { "IsApplicable", true },
                    { "IsDefault", false },
                    { "AggregateType", "sum" },
                    { "AggregateTitle", "sum of Abundance" },
                    { "AggregateFacetId", 32 }
                };

                Asserter.EqualByProperty(expectedProperties, facet);

                Assert.NotNull(facet.FacetGroup);
                Assert.NotNull(facet.TargetTable);
                Assert.NotNull(facet.FacetType);
                Assert.NotNull(facet.Tables);

                Assert.True(facet.Tables.Count > 0);

            }

        }

        [Fact]
        public void CanGetAliasFacets()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var repository = new FacetRepository(context);

                Facet facet = repository.Get(21);

                Assert.Equal("country", facet.FacetCode);
                Assert.Equal("Country", facet.DisplayTitle);
                Assert.NotNull(facet.FacetGroup);
                Assert.NotNull(facet.TargetTable);
                Assert.NotNull(facet.FacetType);
                Assert.True(facet.Tables.Count > 0);

                List<Facet> aliasFacets = repository.FindThoseWithAlias().ToList();
                Assert.Single(aliasFacets);
                Assert.Same(facet, aliasFacets[0]);
            }
        }
    }
}
