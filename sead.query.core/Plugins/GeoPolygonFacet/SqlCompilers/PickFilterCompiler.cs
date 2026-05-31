using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SeadQueryCore.Plugin.GeoPolygon;

public class GeoPolygonPickFilterCompiler : IGeoPolygonPickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (!config.HasPicks())
            return currentFacet.Criteria;

        var polygon = GetPolygonValues(config);

        if (polygon.Count % 2 != 0 || polygon.Count < 6)
            throw new ArgumentException($"Invalid polygon sizes {polygon.Count}");

        if (polygon[0] != polygon[^2] || polygon[1] != polygon[^1])
            polygon.AddRange([polygon[0], polygon[1]]);

        var dotName = currentFacet.TargetTable.ResolvedAliasOrTableOrUdfName;
        return SqlCompileUtility
            .WithinPolygonExpr($"{dotName}.latitude_dd", $"{dotName}.longitude_dd", polygon)
            .GlueIf(currentFacet.Criteria, " AND ");
    }

    private static List<decimal> GetPolygonValues(FacetConfig2 config)
    {
        var cultureInfo = new CultureInfo("en-US");

        return config
            .Picks.SelectMany(pick => pick.PickValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(value => decimal.Parse(value, NumberStyles.Any, cultureInfo))
            .ToList();
    }
}
