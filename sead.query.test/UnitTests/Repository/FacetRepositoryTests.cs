using SeadQueryCore;
using SeadQueryInfra;
using SQT.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    public class FacetRepositoryTests: DisposableFacetContextContainer
    {
        public FacetRepositoryTests(JsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Get_ByFacetCode_Success()
        {
            var repository = Registry.Facets;

            Facet facet = repository.GetByCode("species");

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
        public void FindThoseWithAlias_Success()
        {
            var repository = Registry.Facets;

            List<Facet> aliasFacets = repository.FindThoseWithAlias().ToList();
            Assert.True(aliasFacets.Count > 0);

            var facet = aliasFacets.First();

            Assert.NotNull(facet);
            Assert.True(facet.Tables.Exists(z => z.HasAlias));

        }

        //    [Fact]
        //    public void Children_FacetHasASingleChild_ReturnsThatChild()
        //    {
        //        var context = FacetContext;
        //        var facetTypeRepository = new Repository<FacetType, EFacetType>(context);
        //        var parentGroup = FacetGroupFactory.Fake(27218);
        //        var childGroup = FacetGroupFactory.Fake(175);

        //        var discreteType = facetTypeRepository.Get(EFacetType.Discrete);

        //        var facets = new List<Facet>()
        //        {
        //            FacetFactory.Fake("parent", discreteType, parentGroup, is_applicable: false),
        //            FacetFactory.Fake("child 1", discreteType, childGroup),
        //            FacetFactory.Fake("child 2", discreteType, childGroup)
        //        };

        //        var relations = new List<FacetChild>()
        //        {
        //            new FacetChild {
        //                FacetCode = facets[0].FacetCode,
        //                ChildFacetCode = facets[1].FacetCode,
        //            }
        //        };

        //        context.FacetGroups.Add(parentGroup);
        //        context.FacetGroups.Add(childGroup);
        //        context.Facets.AddRange(facets);
        //        context.FacetChildren.AddRange(relations);

        //        context.SaveChanges();

        //        var repository = new FacetRepository(context);

        //        var parent = repository.GetByCode("parent");
        //        Assert.NotNull(parent);

        //        var children = repository.Children(parent.FacetCode);

        //        Assert.NotNull(children);
        //        Assert.Single(children);
        //        Assert.Same(facets[1], children.FirstOrDefault());
        //    }
    }
}
