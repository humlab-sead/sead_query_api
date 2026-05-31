using System.Threading.Tasks;

namespace SeadQueryComposer.QueryComposer.Compilers
{
    /// <summary>
    /// Configuration for range facet binning
    /// </summary>
    public class RangeBinningConfig
    {
        public string StrategyType { get; set; } = "fixed_bin_count";
        public object Parameters { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }

    /// <summary>
    /// Resolver for range facets (e.g., depth, age, dates).
    /// Handles range filtering and binning for content display.
    /// </summary>
    public class RangeFacetPredicateResolver : IContentCompiler
    {
        private readonly IBucketCompiler _binningStrategy;

        public RangeFacetPredicateResolver(IBucketCompiler binningStrategy = null)
        {
            _binningStrategy = binningStrategy ?? new FixedCountBucketCompiler();
        }

        public EFacetType FacetType => EFacetType.Range;

        public Task<string> ResolvePredicateAsync(Facet facet, FacetConfig2 facetConfig, string anchorName, object additionalPayload = null)
        {
            if (!facetConfig.HasPicks() || facetConfig.GetPickCount() < 2)
            {
                return Task.FromResult(string.Empty);
            }

            var picks = facetConfig.GetPickValues(sort: true);
            var minValue = picks[0];
            var maxValue = picks[picks.Count - 1];

            var template = GetTemplateForAnchor(facet, anchorName);

            // Replace range placeholders
            var resolvedSql = template.Replace("@min_value", minValue.ToString()).Replace("@max_value", maxValue.ToString());

            return Task.FromResult(resolvedSql);
        }

        public Task<string> ResolveContentQueryAsync(
            Facet facet,
            string filteredAnchorIdsQuery,
            string anchorName,
            object additionalPayload = null
        )
        {
            var binningConfig = additionalPayload as RangeBinningConfig ?? new RangeBinningConfig();

            // Get the value expression for this facet
            var valueExpression = GetValueExpression(facet);

            // Generate binning SQL
            var binningSQL = _binningStrategy.Compile(
                valueExpression,
                binningConfig.MinValue,
                binningConfig.MaxValue,
                binningConfig.Parameters
            );

            var sql =
                $@"
WITH filtered_anchor_ids AS (
    {filteredAnchorIdsQuery}
),
bins AS (
    {binningSQL}
),
data_with_bins AS (
    SELECT 
        fa.anchor_id,
        {valueExpression} AS value,
        bins.bin_min,
        bins.bin_max,
        bins.bin_label,
        bins.bin_number
    FROM filtered_anchor_ids fa";

            // Add joins to reach the target entity
            sql += BuildJoinsToTargetEntity(facet, anchorName);

            sql +=
                @"
    CROSS JOIN bins
    WHERE "
                + valueExpression
                + @" >= bins.bin_min 
      AND "
                + valueExpression
                + @" < bins.bin_max
)
SELECT
    CONCAT(bin_min, '-', bin_max) AS CategoryId,
    bin_label AS CategoryName,
    COUNT(DISTINCT anchor_id) AS ItemCount,
    bin_min,
    bin_max,
    bin_number
FROM data_with_bins
GROUP BY bin_min, bin_max, bin_label, bin_number
ORDER BY bin_number";

            return Task.FromResult(sql);
        }

        private string GetTemplateForAnchor(Facet facet, string anchorName)
        {
            // Simplified templates for range facets
            return facet.FacetCode switch
            {
                "sample_depth" =>
                    "SELECT T1.physical_sample_id AS anchor_id, T1.sample_id AS target_id FROM tbl_physical_samples AS T1 WHERE T1.depth_cm BETWEEN @min_value AND @max_value",
                "sample_age" =>
                    "SELECT T1.physical_sample_id AS anchor_id, T1.sample_id AS target_id FROM tbl_physical_samples AS T1 WHERE T1.age_years BETWEEN @min_value AND @max_value",
                _ => throw new System.NotSupportedException($"Range facet '{facet.FacetCode}' not supported in simplified implementation"),
            };
        }

        private string GetValueExpression(Facet facet)
        {
            // Return the column expression for the value to be binned
            return facet.FacetCode switch
            {
                "sample_depth" => "s.depth_cm",
                "sample_age" => "s.age_years",
                _ => facet.CategoryIdExpr, // Fallback to the category expression
            };
        }

        private string BuildJoinsToTargetEntity(Facet facet, string anchorName)
        {
            // Build joins to reach the target entity for range facets
            return facet.FacetCode switch
            {
                "sample_depth" or "sample_age" => "\n    INNER JOIN tbl_physical_samples s ON fa.anchor_id = s.physical_sample_id",
                _ => "",
            };
        }
    }
}
