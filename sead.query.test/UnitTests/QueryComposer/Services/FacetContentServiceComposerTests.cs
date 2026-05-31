using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SeadQueryCore;
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

        var service = new FacetContentService(composedService.Object);

        var result = service.Load(facetsConfig);

        result.Should().BeSameAs(expected);
        composedService.Verify(service => service.CanHandle(It.IsAny<FacetsConfig2>()), Times.Never);
        composedService.Verify(service => service.Load(facetsConfig), Times.Once);
    }

    [Fact]
    public void Load_WhenComposedServiceThrows_PropagatesComposerOnlyFailure()
    {
        var facetsConfig = new FacetsConfig2
        {
            TargetCode = "country",
            TargetFacet = new Facet { FacetCode = "country" },
            FacetConfigs = [],
        };
        var composedService = new Mock<IComposedFacetContentService>(MockBehavior.Strict);
        composedService
            .Setup(service => service.Load(facetsConfig))
            .Throws(new InvalidOperationException("Unsupported facet-content requests no longer fall back to the legacy runtime."));

        var service = new FacetContentService(composedService.Object);

        var act = () => service.Load(facetsConfig);

        act.Should().Throw<InvalidOperationException>().WithMessage("*no longer fall back to the legacy runtime*");
        composedService.Verify(service => service.CanHandle(It.IsAny<FacetsConfig2>()), Times.Never);
        composedService.Verify(service => service.Load(facetsConfig), Times.Once);
    }
}
