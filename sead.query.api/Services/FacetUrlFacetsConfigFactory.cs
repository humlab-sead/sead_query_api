using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SeadQueryCore;

namespace SeadQueryAPI.Services;

public sealed class FacetUrlFacetsConfigFactory
{
    private static readonly Regex RangeTupleRegex = new(@"^\(([-+]?\d+(?:\.\d+)?),([-+]?\d+(?:\.\d+)?)\)$", RegexOptions.Compiled);
    private readonly IRepositoryRegistry _registry;

    public FacetUrlFacetsConfigFactory(IRepositoryRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    public FacetsConfig2 Create(string facetUrl)
    {
        if (string.IsNullOrWhiteSpace(facetUrl))
        {
            throw new ArgumentException("Facet URL must be non-empty.", nameof(facetUrl));
        }

        var parsed = Parse(facetUrl);
        var position = 0;

        var facetConfigs = parsed.FacetCodes.Select(entry => CreateFacetConfig(GetFacet(entry.Key), position++, entry.Value)).ToList();

        return new FacetsConfig2
        {
            DomainCode = parsed.Domain,
            DomainFacet = parsed.Domain.Length > 0 ? GetFacet(parsed.Domain) : null,
            TargetCode = parsed.TargetCode,
            TargetFacet = GetFacet(parsed.TargetCode),
            RequestId = "facet-url-sql",
            RequestType = "populate",
            FacetConfigs = facetConfigs,
        };
    }

    private Facet GetFacet(string facetCode)
    {
        var facet = _registry.Facets.GetByCode(facetCode);
        if (facet is null)
        {
            throw new InvalidOperationException($"Could not resolve facet '{facetCode}' from the current repository registry.");
        }

        return facet;
    }

    private static FacetConfig2 CreateFacetConfig(Facet facet, int position, IReadOnlyList<FacetConfigPick> picks)
    {
        return new FacetConfig2(facet, position, string.Empty, picks?.ToList() ?? []);
    }

    private static ParsedFacetUrl Parse(string facetUrl)
    {
        var domain = string.Empty;
        var uri = facetUrl;
        var domainParts = facetUrl.Split("://", StringSplitOptions.None);
        if (domainParts.Length > 1)
        {
            domain = domainParts[0];
            uri = domainParts[1];
        }

        var parts = uri.Split(':', 2);
        var targetParts = parts[0].Split('@', 2);
        var targetCode = targetParts[0];
        var facetConfigCodes = parts.Length > 1 ? parts[1] : targetCode;

        var facetCodes = facetConfigCodes
            .Split('/')
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Select(part => part.Split('@', 2))
            .Select(parts => new
            {
                FacetCode = parts[0].Trim(),
                Picks = parts.Length > 1 ? ParsePicks(parts[1]) : new List<FacetConfigPick>(),
            })
            .ToDictionary(entry => entry.FacetCode, entry => entry.Picks);

        return new ParsedFacetUrl(domain, targetCode, facetCodes);
    }

    private static List<FacetConfigPick> ParsePicks(string data)
    {
        var match = RangeTupleRegex.Match(data);
        if (match.Success)
        {
            return
            [
                new FacetConfigPick(decimal.Parse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture)),
                new FacetConfigPick(decimal.Parse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture)),
            ];
        }

        return data.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(value => new FacetConfigPick(value.Trim())).ToList();
    }

    private sealed record ParsedFacetUrl(string Domain, string TargetCode, Dictionary<string, List<FacetConfigPick>> FacetCodes);
}
