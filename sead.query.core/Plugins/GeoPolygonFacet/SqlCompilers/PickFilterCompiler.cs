using System;

namespace SeadQueryCore.Plugin.GeoPolygon;

public class GeoPolygonPickFilterCompiler : IGeoPolygonPickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (!config.HasPicks())
            return currentFacet.Criteria;

        var polygons = config.GetPickValueGroups();

        for (var i = 0; i < polygons.Count; i++)
        {
            var polygon = polygons[i];

            if (polygon.Count % 2 != 0 || polygon.Count < 6)
                throw new ArgumentException(polygons.Count == 1
                    ? $"Invalid polygon sizes {polygon.Count}"
                    : $"Invalid polygon sizes {polygon.Count} (polygon {i})");

            /* Close the ring if the client didn't */
            if (polygon[0] != polygon[^2] || polygon[1] != polygon[^1])
                polygon.AddRange([polygon[0], polygon[1]]);
        }

        var dotName = currentFacet.TargetTable.ResolvedAliasOrTableOrUdfName;
        return SqlCompileUtility.WithinAnyPolygonExpr($"{dotName}.latitude_dd", $"{dotName}.longitude_dd", polygons)
            .GlueIf(currentFacet.Criteria, " AND ");
    }
}
