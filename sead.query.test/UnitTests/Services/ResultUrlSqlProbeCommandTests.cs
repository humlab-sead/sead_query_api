using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Moq;
using SeadQueryAPI.DTO;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using Xunit;

namespace SQT.UnitTests.Services;

public class ResultUrlSqlProbeCommandTests
{
    [Fact]
    public void Run_WithFacetUrl_PrintsResultSqlAndHandoffMetadata()
    {
        var repository = new Mock<IFacetRepository>();
        var registry = new Mock<IRepositoryRegistry>();
        registry.SetupGet(x => x.Facets).Returns(repository.Object);

        var familyFacet = CreateFacet("family", EFacetType.Discrete, "Family");
        var resultFacet = CreateFacet("result_facet", EFacetType.Discrete, "Result facet");
        repository.Setup(x => x.GetByCode("family")).Returns(familyFacet);
        repository.Setup(x => x.GetByCode("result_facet")).Returns(resultFacet);

        var factory = new FacetUrlFacetsConfigFactory(registry.Object);
        var pickSanitizer = new Mock<ISupportedRequestPickSanitizer>();
        pickSanitizer.Setup(service => service.Update(It.IsAny<FacetsConfig2>())).Returns<FacetsConfig2>(config => config);

        var reconstituteService = new Mock<IResultConfigReconstituteService>();
        reconstituteService
            .Setup(service => service.Reconstitute(It.IsAny<ResultConfigDTO>()))
            .Returns(
                (ResultConfigDTO dto) =>
                    new ResultConfig
                    {
                        RequestId = dto.RequestId,
                        SessionId = dto.SessionId,
                        ViewTypeId = dto.ViewTypeId,
                        FacetCode = dto.FacetCode ?? "result_facet",
                        Facet = resultFacet,
                        SpecificationKeys = ["site_level"],
                        ViewType = new ResultViewType
                        {
                            ViewTypeId = dto.ViewTypeId,
                            ResultFacetCode = "result_facet",
                            SpecificationKey = "site_level",
                        },
                        Specifications = [],
                    }
            );

        var handoffBuilder = new Mock<IResultProjectionHandoffBuilder>();
        handoffBuilder
            .Setup(builder => builder.Build(It.IsAny<FacetsConfig2>(), It.IsAny<ResultConfig>()))
            .Returns(
                new ResultProjectionHandoff(
                    new QuerySetup { LeadingSql = "with composed_filter as (select 1)", Joins = ["inner join target_route on true"] },
                    []
                )
            );

        var loadResultService = new Mock<ILoadResultService>();
        loadResultService
            .Setup(service => service.Load(It.IsAny<FacetsConfig2>(), It.IsAny<ResultConfig>()))
            .Returns(new ResultContentSet { Query = "select * from result_sql" });

        var command = new ResultUrlSqlProbeCommand(
            factory,
            reconstituteService.Object,
            pickSanitizer.Object,
            loadResultService.Object,
            handoffBuilder.Object
        );

        using var writer = new StringWriter();
        command.Run("family:family@1,2", "map", "result_facet", writer);

        var output = writer.ToString();
        output.Should().Contain("=== FACET URL ===");
        output.Should().Contain("family:family@1,2");
        output.Should().Contain("=== EXECUTION PATH ===");
        output.Should().Contain("composed");
        output.Should().Contain("=== VIEW TYPE ===");
        output.Should().Contain("map");
        output.Should().Contain("=== RESULT FACET ===");
        output.Should().Contain("result_facet");
        output.Should().Contain("=== SPECIFICATION KEYS ===");
        output.Should().Contain("site_level");
        output.Should().Contain("=== HANDOFF LEADING SQL ===");
        output.Should().Contain("with composed_filter as (select 1)");
        output.Should().Contain("=== HANDOFF JOINS ===");
        output.Should().Contain("inner join target_route on true");
        output.Should().Contain("=== RESULT SQL ===");
        output.Should().Contain("select * from result_sql");
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
