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
}
