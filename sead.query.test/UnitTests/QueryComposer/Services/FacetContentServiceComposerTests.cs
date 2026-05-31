using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryComposer;
using Xunit;

namespace SQT.UnitTests.QueryComposer.Services;

public class FacetContentServiceComposerTests
{
    [Fact]
    public void Load_WhenComposedServiceCanHandle_DelegatesToComposedService()
    {
        var facetsConfig = new FacetsConfig2
        {
            TargetCode = "country",
            TargetFacet = new Facet { FacetCode = "country" },
            FacetConfigs = [],
        };
        var expected = new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items =
            [
                new CategoryItem
                {
                    Category = "SE",
                    Count = 1,
                    Name = "SE",
                    Extent = [1],
                },
            ],
            Distribution = new()
            {
                ["SE"] = new CategoryItem
                {
                    Category = "SE",
                    Count = 1,
                    Name = "SE",
                    Extent = [1],
                },
            },
            Picks = [],
            IntervalInfo = new FacetContent.CategoryInfo(),
            SqlQuery = "select 1",
        };

        var composedService = new Mock<IComposedFacetContentService>();
        composedService.Setup(service => service.CanHandle(facetsConfig)).Returns(true);
        composedService.Setup(service => service.Load(facetsConfig)).Returns(expected);

        var categoryCountService = new Mock<ICategoryCountService>(MockBehavior.Strict);
        var service = new FacetContentService(
            Mock.Of<IFacetSetting>(),
            Mock.Of<IRepositoryRegistry>(),
            Mock.Of<IQuerySetupBuilder>(),
            Mock.Of<ITypedQueryProxy>(),
            categoryCountService.Object,
            composedService.Object
        );

        var result = service.Load(facetsConfig);

        result.Should().BeSameAs(expected);
        composedService.Verify(service => service.Load(facetsConfig), Times.Once);
        categoryCountService.Verify(
            service => service.Load(It.IsAny<string>(), It.IsAny<FacetsConfig2>(), EFacetType.Unknown),
            Times.Never
        );
    }

    [Fact]
    public void Load_WhenComposedServiceCannotHandle_UsesLegacyCategoryCountService()
    {
        var facetsConfig = new FacetsConfig2
        {
            TargetCode = "country",
            TargetFacet = new Facet { FacetCode = "country" },
            FacetConfigs = [],
        };
        var outerCategoryCounts = new List<CategoryItem>
        {
            new()
            {
                Category = "SE",
                Count = 2,
                Name = "SE",
                Extent = [2],
            },
        };
        var categoryCountData = new CategoryCountService.CategoryCountData
        {
            OuterCategoryCounts = outerCategoryCounts,
            CategoryCounts = new() { ["SE"] = outerCategoryCounts[0] },
            CategoryInfo = new FacetContent.CategoryInfo { Count = 1, Query = "select legacy" },
            SqlQuery = "select legacy",
        };

        var composedService = new Mock<IComposedFacetContentService>(MockBehavior.Strict);
        composedService.Setup(service => service.CanHandle(facetsConfig)).Returns(false);

        var categoryCountService = new Mock<ICategoryCountService>(MockBehavior.Strict);
        categoryCountService
            .Setup(service => service.Load(facetsConfig.TargetCode, facetsConfig, EFacetType.Unknown))
            .Returns(categoryCountData);

        var service = new FacetContentService(
            Mock.Of<IFacetSetting>(),
            Mock.Of<IRepositoryRegistry>(),
            Mock.Of<IQuerySetupBuilder>(),
            Mock.Of<ITypedQueryProxy>(),
            categoryCountService.Object,
            composedService.Object
        );

        var result = service.Load(facetsConfig);

        result.Items.Should().BeEquivalentTo(outerCategoryCounts);
        result.Distribution.Should().BeEquivalentTo(categoryCountData.CategoryCounts);
        result.IntervalInfo.Should().BeSameAs(categoryCountData.CategoryInfo);
        result.SqlQuery.Should().Be(categoryCountData.SqlQuery);
        composedService.Verify(service => service.CanHandle(facetsConfig), Times.Once);
        composedService.Verify(service => service.Load(It.IsAny<FacetsConfig2>()), Times.Never);
        categoryCountService.Verify(service => service.Load(facetsConfig.TargetCode, facetsConfig, EFacetType.Unknown), Times.Once);
    }
}
