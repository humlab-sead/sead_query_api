using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using SeadQueryCore.QueryComposer.Models;
using Xunit;

namespace SeadQuery.Tests.QueryComposer.RouteCompiler;

public class DiscreteFacetPredicateResolverIntegrationTests
{
    private readonly IRouteParser _fakeRouteParser;
    private readonly ITableRelationRepository _fakeRelationRepository;
    private readonly DiscreteFacetPredicateResolver _resolver;

    public DiscreteFacetPredicateResolverIntegrationTests()
    {
        _fakeRouteParser = FakeItEasy.Fake<IRouteParser>();
        _fakeRelationRepository = A.Fake<ITableRelationRepository>();
        _resolver = new DiscreteFacetPredicateResolver(_fakeRouteParser, _fakeRelationRepository);
    }

    [Fact]
    public async Task ResolveSqlAsync_WithComplexRoute_GeneratesCompleteValidSql()
    {
        // Arrange
        var userInput = new DiscreteFacetUserInput
        {
            Picks = new List<object> { 101, 102, 103 },
        };
        var configuration = new AnchorTemplate
        {
            Route = new List<string> { "tbl_sample_groups", "tbl_physical_samples" },
            RequiresDistinct = true,
        };

        A.CallTo(() => _fakeRouteParser.ResolveRouteAsync(A<List<string>>._))
            .Returns(new List<string> { "tbl_sample_groups", "tbl_physical_samples" });

        A.CallTo(() => _fakeRelationRepository.GetRelationAsync("tbl_sites", "tbl_sample_groups"))
            .Returns(new TableRelation { SourceKey = "site_id", ForeignKey = "site_id" });

        A.CallTo(() => _fakeRelationRepository.GetRelationAsync("tbl_sample_groups", "tbl_physical_samples"))
            .Returns(new TableRelation { SourceKey = "sample_group_id", ForeignKey = "sample_group_id" });

        // Act
        var result = await _resolver.ResolveSqlAsync(
            "site_facet",
            "tbl_sites",
            "site_id",
            userInput,
            configuration,
            "tbl_physical_samples",
            "physical_sample_id"
        );

        // Assert
        Assert.Contains("SELECT DISTINCT", result);
        Assert.Contains("s.site_id as target_id", result);
        Assert.Contains("ps.physical_sample_id as anchor_id", result);
        Assert.Contains("FROM tbl_sites as s", result);
        Assert.Contains("JOIN tbl_sample_groups as sg", result);
        Assert.Contains("JOIN tbl_physical_samples as ps", result);
        Assert.Contains("WHERE s.site_id IN", result);
        Assert.Contains("101", result);
        Assert.Contains("102", result);
        Assert.Contains("103", result);
    }

    [Fact]
    public async Task ResolveSqlAsync_WithFacetConfig2Input_ConvertsAndProcessesCorrectly()
    {
        // Arrange
        var facetConfig = new FacetConfig2
        {
            FacetCode = "site_facet",
            Picks = new List<FacetConfigPick>
            {
                new() { PickValue = 101 },
                new() { PickValue = 102 },
            },
        };
        var configuration = new AnchorTemplate { Route = new List<string>() };

        // Act
        var result = await _resolver.ResolveSqlAsync(
            "site_facet",
            "tbl_sites",
            "site_id",
            facetConfig,
            configuration,
            "tbl_sites",
            "site_id"
        );

        // Assert
        Assert.Contains("SELECT", result);
        Assert.Contains("WHERE s.site_id IN", result);
        Assert.Contains("101", result);
        Assert.Contains("102", result);
    }
}
