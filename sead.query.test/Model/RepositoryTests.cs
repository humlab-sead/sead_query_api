using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Model
{

    public class RepositoryTests : FacetTestBase
    {

        [Fact]
        public void ShouldBeAbleToFetchAllResultFields()
        {
            // Arrange
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var sut = new ResultRepository(context);

                var expected = context.Set<ResultField>().ToList().Count;

                Assert.Equal(expected, sut.GetAllFields().ToList().Count);
            }
        }

        [Fact]
        public void ShouldBeAbleToFetchAllResultAggregates()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var sut = new ResultRepository(context);
                List<ResultAggregate> items = sut.GetAll().ToList();

                Assert.Equal(4, items.Count);
                foreach (ResultAggregate value in items) {
                    Assert.NotNull(value.Fields);
                    Assert.True(value.Fields.Count > 0);
                    value.Fields.ForEach(z => Assert.NotNull(z.ResultField));
                    value.Fields.ForEach(z => Assert.NotNull(z.FieldType));
                }
            }
        }

        [Fact]
        public void ShouldBeAbleToFetchAllResultTypes()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var repository = new ResultRepository(context);

                List<ResultFieldType> fieldTypes = repository.GetAllFieldTypes().ToList();

                Assert.True(fieldTypes.Count > 0);

                var expected = new List<string>() { "single_item", "text_agg_item", "count_item", "link_item", "sort_item", "link_item_filtered" };

                Assert.True(expected.All(x => fieldTypes.Exists(w => w.FieldTypeId == x)));
            }
        }


        [Fact]
        public void ShouldBeAbleToFetchFacetAndReferenceObjects()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var repository = new FacetRepository(context);

                Facet facet = repository.Get(25);

                Dictionary <string, object> expectedProperties = new Dictionary<string, object>() {
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

                AssertPropertiesEquals(expectedProperties, facet);

                Assert.NotNull(facet.FacetGroup);
                Assert.NotNull(facet.TargetNode);
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
                Assert.NotNull(facet.TargetNode);
                Assert.NotNull(facet.FacetType);
                Assert.True(facet.Tables.Count > 0);

                List<Facet> aliasFacets = repository.FindThoseWithAlias().ToList();
                Assert.Single(aliasFacets);
                Assert.Same(facet, aliasFacets[0]);
            }
        }
    }
}
