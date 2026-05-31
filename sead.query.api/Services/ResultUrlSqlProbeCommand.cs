using System;
using System.IO;
using System.Linq;
using SeadQueryAPI.DTO;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;

namespace SeadQueryAPI.Services;

public sealed class ResultUrlSqlProbeCommand
{
    private readonly FacetUrlFacetsConfigFactory _facetsConfigFactory;
    private readonly IResultConfigReconstituteService _resultConfigReconstituteService;
    private readonly ISupportedRequestPickSanitizer _pickSanitizer;
    private readonly ILoadResultService _loadResultService;
    private readonly IResultProjectionHandoffBuilder _resultProjectionHandoffBuilder;

    public ResultUrlSqlProbeCommand(
        FacetUrlFacetsConfigFactory facetsConfigFactory,
        IResultConfigReconstituteService resultConfigReconstituteService,
        ISupportedRequestPickSanitizer pickSanitizer,
        ILoadResultService loadResultService,
        IResultProjectionHandoffBuilder resultProjectionHandoffBuilder
    )
    {
        _facetsConfigFactory = facetsConfigFactory ?? throw new ArgumentNullException(nameof(facetsConfigFactory));
        _resultConfigReconstituteService =
            resultConfigReconstituteService ?? throw new ArgumentNullException(nameof(resultConfigReconstituteService));
        _pickSanitizer = pickSanitizer ?? throw new ArgumentNullException(nameof(pickSanitizer));
        _loadResultService = loadResultService ?? throw new ArgumentNullException(nameof(loadResultService));
        _resultProjectionHandoffBuilder =
            resultProjectionHandoffBuilder ?? throw new ArgumentNullException(nameof(resultProjectionHandoffBuilder));
    }

    public void Run(string facetUrl, string viewTypeId, string resultCode = null, TextWriter writer = null)
    {
        if (string.IsNullOrWhiteSpace(facetUrl))
        {
            throw new ArgumentException("Facet URL must be non-empty.", nameof(facetUrl));
        }

        if (string.IsNullOrWhiteSpace(viewTypeId))
        {
            throw new ArgumentException("View type must be non-empty.", nameof(viewTypeId));
        }

        writer ??= Console.Out;

        var facetsConfig = _facetsConfigFactory.Create(facetUrl);
        var normalizedConfig = _pickSanitizer.Update(facetsConfig);
        var resultConfig = _resultConfigReconstituteService.Reconstitute(
            new ResultConfigDTO
            {
                RequestId = "result-url-sql",
                SessionId = "result-url-sql",
                ViewTypeId = viewTypeId,
                FacetCode = resultCode,
            }
        );

        var handoff = _resultProjectionHandoffBuilder.Build(normalizedConfig, resultConfig);
        var result = _loadResultService.Load(normalizedConfig, resultConfig);
        var executionPath = ResolveExecutionPath(handoff);

        WriteSection(writer, "FACET URL", facetUrl);
        WriteSection(writer, "EXECUTION PATH", executionPath);
        WriteSection(writer, "VIEW TYPE", resultConfig.ViewTypeId);
        WriteSection(writer, "RESULT FACET", resultConfig.FacetCode);
        WriteSection(writer, "SPECIFICATION KEYS", string.Join(",", resultConfig.SpecificationKeys));

        writer.WriteLine("=== TARGET FACET ===");
        writer.WriteLine($"Code: {normalizedConfig.TargetFacet.FacetCode}");
        writer.WriteLine($"Type: {normalizedConfig.TargetFacet.FacetTypeId}");
        writer.WriteLine($"Title: {normalizedConfig.TargetFacet.DisplayTitle ?? string.Empty}");
        writer.WriteLine();

        WriteSection(writer, "HANDOFF LEADING SQL", handoff.QuerySetup.LeadingSql ?? string.Empty);

        writer.WriteLine("=== HANDOFF JOINS ===");
        if (handoff.QuerySetup.Joins?.Count > 0)
        {
            foreach (var join in handoff.QuerySetup.Joins)
            {
                writer.WriteLine(join);
            }
        }
        else
        {
            writer.WriteLine("(none)");
        }

        writer.WriteLine();
        WriteSection(writer, "RESULT SQL", result.Query ?? string.Empty);
    }

    private static string ResolveExecutionPath(ResultProjectionHandoff handoff)
    {
        var leadingSql = handoff.QuerySetup.LeadingSql ?? string.Empty;
        return
            leadingSql.Contains("composed_filter", StringComparison.OrdinalIgnoreCase)
            || leadingSql.Contains("target_route", StringComparison.OrdinalIgnoreCase)
            ? "composed"
            : "legacy";
    }

    private static void WriteSection(TextWriter writer, string title, string content)
    {
        writer.WriteLine($"=== {title} ===");
        writer.WriteLine(content);
        writer.WriteLine();
    }
}
