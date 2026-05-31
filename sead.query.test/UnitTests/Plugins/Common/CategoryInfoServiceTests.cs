using FluentAssertions;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Plugin.Common;
using SeadQueryCore.QueryBuilder;
using Xunit;

namespace SQT.Plugins.Common;

public class CategoryInfoServiceTests
{
    [Fact]
    public void GetCategoryInfo_UsesSupportedRequestQuerySetupFactory()
    {
        var targetFacet = new Facet
        {
            FacetCode = "sites",
            DisplayTitle = "Sites",
            FacetTypeId = EFacetType.Discrete,
        };
        var facetsConfig = new FacetsConfig2
        {
            TargetCode = targetFacet.FacetCode,
            TargetFacet = targetFacet,
            FacetConfigs = [new FacetConfig2(targetFacet, 0, string.Empty, [])],
        };
        var querySetup = new QuerySetup { Facet = facetsConfig.TargetFacet };

        var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
        querySetupFactory.Setup(factory => factory.Create(facetsConfig, facetsConfig.TargetFacet, null, null)).Returns(querySetup);

        var compiler = new Mock<ICategoryInfoSqlCompiler>();
        compiler
            .Setup(x => x.Compile(querySetup, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter()))
            .Returns("select * from category_info");

        var service = new TestCategoryInfoService(querySetupFactory.Object, compiler.Object);

        var result = service.GetCategoryInfo(facetsConfig, facetsConfig.TargetCode);

        result.Query.Should().Be("select * from category_info");
        result.Count.Should().Be(1);
        querySetupFactory.Verify(factory => factory.Create(facetsConfig, facetsConfig.TargetFacet, null, null), Times.Once);
        compiler.Verify(x => x.Compile(querySetup, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter()), Times.Once);
    }

    private sealed class TestCategoryInfoService(ISupportedRequestQuerySetupFactory factory, ICategoryInfoSqlCompiler compiler)
        : CategoryInfoService(factory, compiler) { }
}
