using System;

namespace SeadQueryCore.QueryBuilder;

public class GeoPolygonPickFilterCompiler : IPickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (!config.HasPicks())
            return currentFacet.Criteria;

        var polygon = config.GetPickValues(false);

        if (polygon.Count % 2 != 0 || polygon.Count < 6)
            throw new ArgumentException($"Invalid polygon sizes {polygon.Count}");

        if (polygon[0] != polygon[^2] || polygon[1] != polygon[^1])
            polygon.AddRange([polygon[0], polygon[1]]);

        return SqlCompileUtility.WithinPolygonExpr(currentFacet.CategoryIdExpr, polygon)
            .GlueIf(currentFacet.Criteria, " AND ");

    }

}
