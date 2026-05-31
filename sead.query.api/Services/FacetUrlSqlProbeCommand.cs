using System;
using System.Globalization;
using System.IO;
using System.Linq;
using SeadQueryCore;
using SeadQueryCore.QueryComposer;

namespace SeadQueryAPI.Services;

public sealed class FacetUrlSqlProbeCommand
{
    private readonly FacetUrlFacetsConfigFactory _facetsConfigFactory;
    private readonly ISupportedRequestPickSanitizer _pickSanitizer;
    private readonly IFacetContentService _facetContentService;
    private readonly IComposedFacetContentService _composedFacetContentService;

    public FacetUrlSqlProbeCommand(
        FacetUrlFacetsConfigFactory facetsConfigFactory,
        ISupportedRequestPickSanitizer pickSanitizer,
        IFacetContentService facetContentService,
        IComposedFacetContentService composedFacetContentService
    )
    {
        _facetsConfigFactory = facetsConfigFactory ?? throw new ArgumentNullException(nameof(facetsConfigFactory));
        _pickSanitizer = pickSanitizer ?? throw new ArgumentNullException(nameof(pickSanitizer));
        _facetContentService = facetContentService ?? throw new ArgumentNullException(nameof(facetContentService));
        _composedFacetContentService = composedFacetContentService ?? throw new ArgumentNullException(nameof(composedFacetContentService));
    }

    public void Run(string facetUrl, TextWriter writer = null)
    {
        if (string.IsNullOrWhiteSpace(facetUrl))
        {
            throw new ArgumentException("Facet URL must be non-empty.", nameof(facetUrl));
        }

        writer ??= Console.Out;

        var facetsConfig = _facetsConfigFactory.Create(facetUrl);
    var normalizedConfig = _pickSanitizer.Update(facetsConfig);
        var usesComposedPath = _composedFacetContentService.CanHandle(normalizedConfig);
        var facetContent = _facetContentService.Load(normalizedConfig);

        WriteSection(writer, "FACET URL", facetUrl);
        WriteSection(writer, "EXECUTION PATH", usesComposedPath ? "composed" : "legacy");
        WriteSection(writer, "DOMAIN", normalizedConfig.HasDomainCode() ? normalizedConfig.DomainCode : "(none)");

        writer.WriteLine("=== TARGET FACET ===");
        writer.WriteLine($"Code: {normalizedConfig.TargetFacet.FacetCode}");
        writer.WriteLine($"Type: {normalizedConfig.TargetFacet.FacetTypeId}");
        writer.WriteLine($"Title: {normalizedConfig.TargetFacet.DisplayTitle ?? string.Empty}");
        writer.WriteLine();

        writer.WriteLine("=== FACET CONFIGS ===");
        foreach (var config in normalizedConfig.FacetConfigs.OrderBy(config => config.Position))
        {
            writer.WriteLine(
                $"{config.Position}: {config.FacetCode} | Type={config.Facet?.FacetTypeId} | Picks={FormatPicks(config)} | Filter={FormatTextFilter(config)}"
            );
        }

        writer.WriteLine();
        WriteSection(writer, "ITEM COUNT", facetContent.ItemCount.ToString(CultureInfo.InvariantCulture));
        WriteSection(writer, "SQL", facetContent.SqlQuery ?? string.Empty);

        var categoryInfoQuery = facetContent.IntervalInfo?.Query;
        if (
            !string.IsNullOrWhiteSpace(categoryInfoQuery)
            && !string.Equals(categoryInfoQuery, facetContent.SqlQuery, StringComparison.Ordinal)
        )
        {
            WriteSection(writer, "CATEGORY INFO SQL", categoryInfoQuery);
        }
    }

    private static void WriteSection(TextWriter writer, string title, string content)
    {
        writer.WriteLine($"=== {title} ===");
        writer.WriteLine(content);
        writer.WriteLine();
    }

    private static string FormatPicks(FacetConfig2 config)
    {
        return config.Picks.Count == 0 ? "(none)" : string.Join(",", config.Picks.Select(pick => pick.PickValue));
    }

    private static string FormatTextFilter(FacetConfig2 config)
    {
        return string.IsNullOrWhiteSpace(config.TextFilter) ? "(none)" : config.TextFilter;
    }
}
