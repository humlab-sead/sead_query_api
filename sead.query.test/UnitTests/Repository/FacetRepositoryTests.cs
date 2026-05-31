using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;
using SeadQueryInfra;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("UsePostgresFixture")]
    public class FacetRepositoryTests : MockerWithFacetContext
    {
        public FacetRepositoryTests()
            : base() { }

        [Fact]
        public void Get_ByFacetCode_Success()
        {
            var repository = Registry.Facets;

            Facet facet = repository.GetByCode("species");

            Dictionary<string, object> expectedProperties = new()
            {
                { "FacetId", 25 },
                { "FacetCode", "species" },
                { "DisplayTitle", "Taxa" },
                { "FacetGroupId", 6 },
                { "FacetTypeId", EFacetType.Discrete },
                { "CategoryIdExpr", "tbl_taxa_tree_master.taxon_id" },
                {
                    "CategoryNameExpr",
                    "concat_ws(' ', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)"
                },
                { "SortExpr", "tbl_taxa_tree_genera.genus_name||' '||tbl_taxa_tree_master.species" },
                { "IsApplicable", true },
                { "IsDefault", false },
                { "AggregateType", "sum" },
                { "AggregateTitle", "sum of Abundance" },
                { "AggregateFacetId", 32 },
            };

            Asserter.EqualByProperty(expectedProperties, facet);

            Assert.NotNull(facet.FacetGroup);
            Assert.NotNull(facet.TargetTable);
            Assert.NotNull(facet.FacetType);
            Assert.NotNull(facet.Tables);

            Assert.True(facet.Tables.Count > 0);
        }

        [Fact]
        public void Get_FacetAnchor_Success()
        {
            var repository = Registry.Facets;

            Facet facet = repository.GetByCode("sites");
            Assert.NotNull(facet);
            Assert.NotNull(facet.FacetAnchors);
            Assert.NotEmpty(facet.FacetAnchors);

            var anchors = Registry.Anchors.GetAll();
            Assert.NotEmpty(anchors);

            Assert.All(facet.FacetAnchors, fa => anchors.Any(a => a.AnchorId == fa.AnchorId));
        }

        [Fact]
        public void GetByCode_RoutedSteps_HaveTableReferences()
        {
            var dbContext = (FacetContext)FacetContext;

            using var transaction = dbContext.Database.BeginTransaction();

            var suffix = dbContext.Tables.Max(table => table.TableId) + 1;
            var nextRouteId = dbContext.Routes.Select(route => route.RouteId).DefaultIfEmpty().Max() + 1;
            var nextRouteStepId = dbContext.RouteSteps.Select(step => step.RouteStepId).DefaultIfEmpty().Max() + 1;
            var nextFacetAnchorId = dbContext.FacetAnchors.Select(current => current.FacetAnchorId).DefaultIfEmpty().Max() + 1;
            var sourceTable = dbContext.Tables.OrderBy(table => table.TableId).First();
            var targetTable = dbContext.Tables.OrderBy(table => table.TableId).Skip(1).First();
            var facet = dbContext.Facets.Single(current => current.FacetCode == "sites");
            var anchor = dbContext.Anchors.OrderBy(current => current.AnchorId).First();

            var stepTable = new Table
            {
                TableId = suffix,
                TableOrUdfName = $"test_route_step_table_{suffix}",
                PrimaryKeyName = "test_id",
                IsUdf = false,
            };

            var route = new Route
            {
                RouteId = nextRouteId,
                Name = $"test-route-{suffix}",
                SourceTableId = sourceTable.TableId,
                TargetTableId = targetTable.TableId,
                Specification = "test specification",
                Steps = new List<RouteStep>
                {
                    new()
                    {
                        RouteStepId = nextRouteStepId,
                        SequenceId = 1,
                        RouteId = nextRouteId,
                        TableId = stepTable.TableId,
                        Table = stepTable,
                        KeyName = $"test-step-{suffix}",
                    },
                },
            };

            dbContext.Routes.Add(route);
            dbContext.FacetAnchors.Add(
                new FacetAnchor
                {
                    FacetAnchorId = nextFacetAnchorId,
                    FacetId = facet.FacetId,
                    AnchorId = anchor.AnchorId,
                    RouteId = route.RouteId,
                    Route = route,
                }
            );

            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            var repository = new FacetRepository(Registry);
            var reloadedFacet = repository.GetByCode("sites");
            var reloadedFacetAnchor = reloadedFacet.FacetAnchors.Single(current => current.RouteId == route.RouteId);
            var reloadedStep = Assert.Single(reloadedFacetAnchor.Route.Steps);

            Assert.NotNull(reloadedStep.Table);
            Assert.Equal(stepTable.TableOrUdfName, reloadedStep.Table.TableOrUdfName);
        }

        [Fact]
        public void FindThoseWithAlias_Success()
        {
            var repository = Registry.Facets;
            var anchhors = Registry.Facets;

            List<Facet> aliasFacets = repository.FindThoseWithAlias().ToList();
            Assert.True(aliasFacets.Count > 0);

            var facet = aliasFacets[0];

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
