using System.Linq;
using System.Threading.Tasks;

namespace SeadQueryComposer.QueryComposer.Compilers
{
    /// <summary>
    /// Configuration for GIS polygon facets
    /// </summary>
    public class GisPolygonConfig
    {
        public string WktPolygon { get; set; }
        public int? SpatialReferenceId { get; set; } = 4326; // Default to WGS84
        public string GeometryColumn { get; set; } = "geom";
    }

    /// <summary>
    /// Resolver for GIS polygon facets (e.g., geographic regions, custom drawn areas).
    /// Handles spatial containment filtering.
    /// </summary>
    public class GisPolygonContentCompiler : IContentCompiler
    {
        public EFacetType FacetType => EFacetType.GeoPolygon;

        public Task<string> ResolvePredicateAsync(Facet facet, FacetConfig2 facetConfig, string anchorName, object additionalPayload = null)
        {
            var gisConfig = additionalPayload as GisPolygonConfig;
            if (gisConfig?.WktPolygon == null)
            {
                return Task.FromResult(string.Empty);
            }

            var template = GetTemplateForAnchor(facet, anchorName);

            // Replace GIS placeholders
            var resolvedSql = template
                .Replace("@wkt_polygon", $"'{gisConfig.WktPolygon}'")
                .Replace("@srid", gisConfig.SpatialReferenceId.ToString());

            return Task.FromResult(resolvedSql);
        }

        public Task<string> ResolveContentQueryAsync(
            Facet facet,
            string filteredAnchorIdsQuery,
            string anchorName,
            object additionalPayload = null
        )
        {
            var sql =
                $@"
WITH filtered_anchor_ids AS (
    {filteredAnchorIdsQuery}
)
SELECT
    {facet.CategoryIdExpr} AS CategoryId,
    {facet.CategoryNameExpr} AS CategoryName,
    COUNT(DISTINCT fa.anchor_id) AS ItemCount
FROM
    filtered_anchor_ids fa";

            // Add the necessary joins to reach the target entity
            sql += BuildJoinsToTargetEntity(facet, anchorName);

            sql +=
                $@"
GROUP BY {facet.CategoryIdExpr}, {facet.CategoryNameExpr}
ORDER BY {facet.CategoryNameExpr}";

            return Task.FromResult(sql);
        }

        private string GetTemplateForAnchor(Facet facet, string anchorName)
        {
            // Simplified templates for GIS polygon facets
            return facet.FacetCode switch
            {
                "geographic_regions" => @"
SELECT T1.physical_sample_id AS anchor_id, T2.region_id AS target_id 
FROM tbl_physical_samples AS T1
INNER JOIN tbl_sites AS T2 ON T1.site_id = T2.site_id
WHERE ST_Within(T2.geom, ST_GeomFromText(@wkt_polygon, @srid))",

                "custom_area" => @"
SELECT T1.physical_sample_id AS anchor_id, T1.site_id AS target_id
FROM tbl_physical_samples AS T1
INNER JOIN tbl_sites AS T2 ON T1.site_id = T2.site_id
WHERE ST_Within(T2.geom, ST_GeomFromText(@wkt_polygon, @srid))",

                _ => throw new System.NotSupportedException(
                    $"GIS polygon facet '{facet.FacetCode}' not supported in simplified implementation"
                ),
            };
        }

        private string BuildJoinsToTargetEntity(Facet facet, string anchorName)
        {
            // Build joins for GIS facets
            return facet.FacetCode switch
            {
                "geographic_regions" => @"
    INNER JOIN tbl_sites s ON fa.anchor_id = s.physical_sample_id
    INNER JOIN tbl_geographic_regions gr ON s.region_id = gr.region_id",

                "custom_area" => @"
    INNER JOIN tbl_sites s ON fa.anchor_id = s.physical_sample_id",

                _ => "",
            };
        }
    }
}
