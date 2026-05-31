using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Moq;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.QueryComposer;
using Xunit;

namespace SQT.UnitTests.Services;

public class FacetUrlSqlProbeCommandTests
{
    [Fact]
    public void Run_WithFacetUrl_PrintsSqlAndMetadata()
    {
        var repository = new Mock<IFacetRepository>();
        var registry = new Mock<IRepositoryRegistry>();
        registry.SetupGet(x => x.Facets).Returns(repository.Object);

        var familyFacet = CreateFacet("family", EFacetType.Discrete, "Family");
        repository.Setup(x => x.GetByCode("family")).Returns(familyFacet);

        var factory = new FacetUrlFacetsConfigFactory(registry.Object);
    var pickSanitizer = new Mock<ISupportedRequestPickSanitizer>();
    pickSanitizer.Setup(service => service.Update(It.IsAny<FacetsConfig2>())).Returns<FacetsConfig2>(config => config);

        var composedFacetContentService = new Mock<IComposedFacetContentService>();
        composedFacetContentService.Setup(service => service.CanHandle(It.IsAny<FacetsConfig2>())).Returns(true);

        var facetContentService = new Mock<IFacetContentService>();
        facetContentService
            .Setup(service => service.Load(It.IsAny<FacetsConfig2>()))
            .Returns(
                (FacetsConfig2 config) =>
                    new FacetContent
                    {
                        FacetsConfig = config,
                        SqlQuery = "select * from facet_result",
                        IntervalInfo = new FacetContent.CategoryInfo { Query = "select * from facet_interval" },
                        Items = [new CategoryItem { Category = "A", Count = 1 }],
                        Distribution = new Dictionary<string, CategoryItem>(),
                        Picks = new Dictionary<string, FacetsConfig2.UserPickData>(),
                    }
            );

        var command = new FacetUrlSqlProbeCommand(
            factory,
            pickSanitizer.Object,
            facetContentService.Object,
            composedFacetContentService.Object
        );
        using var writer = new StringWriter();

        command.Run("family:family@1,2", writer);

        var output = writer.ToString();
        output.Should().Contain("=== FACET URL ===");
        output.Should().Contain("family:family@1,2");
        output.Should().Contain("=== EXECUTION PATH ===");
        output.Should().Contain("composed");
        output.Should().Contain("Code: family");
        output.Should().Contain("0: family | Type=Discrete | Picks=1,2 | Filter=(none)");
        output.Should().Contain("=== SQL ===");
        output.Should().Contain("select * from facet_result");
        output.Should().Contain("=== CATEGORY INFO SQL ===");
        output.Should().Contain("select * from facet_interval");
    }

    [Fact]
    public void Create_WithDomainQualifiedFacetUrl_PopulatesDomainAndTuplePicks()
    {
        var repository = new Mock<IFacetRepository>();
        var registry = new Mock<IRepositoryRegistry>();
        registry.SetupGet(x => x.Facets).Returns(repository.Object);

        var domainFacet = CreateFacet("pollen", EFacetType.Discrete, "Pollen");
        var targetFacet = CreateFacet("geochronology", EFacetType.Range, "Geochronology");
        repository.Setup(x => x.GetByCode("pollen")).Returns(domainFacet);
        repository.Setup(x => x.GetByCode("geochronology")).Returns(targetFacet);

        var factory = new FacetUrlFacetsConfigFactory(registry.Object);

        var config = factory.Create("pollen://geochronology:geochronology@(1.5,2.5)");

        config.DomainCode.Should().Be("pollen");
        config.DomainFacet.Should().BeSameAs(domainFacet);
        config.TargetCode.Should().Be("geochronology");
        config.TargetFacet.Should().BeSameAs(targetFacet);
        config.FacetConfigs.Should().HaveCount(1);
        config.FacetConfigs[0].Picks.Should().HaveCount(2);
        config.FacetConfigs[0].Picks[0].PickValue.Should().Be("1.5");
        config.FacetConfigs[0].Picks[1].PickValue.Should().Be("2.5");
    }

    private static Facet CreateFacet(string code, EFacetType facetType, string title)
    {
        return new Facet
        {
            FacetCode = code,
            DisplayTitle = title,
            FacetTypeId = facetType,
        };
    }
}
