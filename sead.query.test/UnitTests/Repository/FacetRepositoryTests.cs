using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class FacetRepositoryTests: DisposableFacetContextContainer
    {
        public FacetRepositoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private FacetContext CreateFacetContext()
        {
            var options = new DbContextOptionsBuilder<FacetContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var facetContext = new FacetContext(options);
            return facetContext;
        }

        [Fact(Skip ="Not implemented")]
        public void SaveChanges_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            using (var facetContext = CreateFacetContext()) {

                // Act
                var result = facetContext.SaveChanges();

                // Assert
                Assert.True(true);
            }
        }

        //[Fact]
        //public void Context_Should_Have_Values_For_All_Entity_Types()
        //{
        //    using (var context = JsonSeededFacetContextFactory.Create()) {

        //        foreach (Type type in ScaffoldUtility.GetModelTypes()) {
        //            var g = GetGenericMethodForType<FacetContext>("Set", type);
        //            var entities = (IEnumerable<object>)(g.Invoke(context, Array.Empty<object>()));
        //            Assert.True(entities.ToList().Count > 0);
        //        }
        //    }
        //}

        //private object GetGenericMethodForType<T>(string v, Type type)
        //{
        //    throw new NotImplementedException();
        //}

        [Fact]
        public void ShouldBeAbleToFetchFacetAndReferenceObjects()
        {
            var repository = Registry.Facets;

            Facet facet = repository.Get(25);

            Dictionary<string, object> expectedProperties = new Dictionary<string, object>() {
                { "FacetId", 25 },
                { "FacetCode", "species" },
                { "DisplayTitle", "Taxa" },
                { "FacetGroupId", 6 },
                { "FacetTypeId", EFacetType.Discrete },
                { "CategoryIdExpr", "tbl_taxa_tree_master.taxon_id" },
                { "CategoryNameExpr", "concat_ws(' ', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)" },
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

        [Fact]
        public void CanGetAliasFacets()
        {
            var repository = Registry.Facets;

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

        [Fact]
        public void FacetChildren_FacetHasASingleChild_ReturnsThatChild()
        {
            using (var connection = SqliteConnectionFactory.CreateAndOpen()) {
                var options = SqliteContextOptionsFactory.Create(Connection);
                using (var context = JsonSeededFacetContextFactory.Create(options, Fixture))
                using (var registry = new RepositoryRegistry(context))
                using (var container = TestDependencyService.CreateContainer(context, null))
                using (var scope = container.BeginLifetimeScope()) {

                    var parentGroup = FacetGroupFactory.Fake();
                    var childGroup = FacetGroupFactory.Fake();

                    var discreteType = FacetTypeFactory.Fake();

                    var facets = new List<Facet>()
                    {
                        FacetFactory.Fake("parent", discreteType, parentGroup, is_applicable: false),
                        FacetFactory.Fake("child 1", discreteType, childGroup),
                        FacetFactory.Fake("child 2", discreteType, childGroup)
                    };

                    var relations = new List<FacetChild>()
                    {
                        new FacetChild {
                            FacetCode = facets[0].FacetCode,
                            ChildFacetCode = facets[1].FacetCode,
                        }
                    };

                    context.FacetTypes.Add(discreteType);
                    context.FacetGroups.Add(parentGroup);
                    context.FacetGroups.Add(childGroup);
                    context.Facets.AddRange(facets);
                    context.FacetChildren.AddRange(relations);

                    context.SaveChanges();

                    var repository = new FacetRepository(context);

                    var parent = repository.GetByCode("parent");
                    Assert.NotNull(parent);

                    var children = repository.Children(parent.FacetCode);

                    Assert.NotNull(children);
                    Assert.Single(children);
                    Assert.Same(facets[1], children.FirstOrDefault());

                }
            }
        }
    }
}
