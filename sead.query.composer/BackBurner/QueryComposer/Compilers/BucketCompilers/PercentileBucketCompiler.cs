namespace SeadQueryComposer.QueryComposer.Compilers;

/// <summary>
/// Dynamic binning based on data distribution (percentiles)
/// </summary>
public class PercentileBucketCompiler : IBucketCompiler
{
    public string StrategyType => "percentile";

    public string Compile(string valueExpression, decimal? minValue, decimal? maxValue, object parameters = null)
    {
        var percentiles = parameters as decimal[] ?? new decimal[] { 0.25m, 0.5m, 0.75m }; // Quartiles by default

        var percentileList = string.Join(", ", percentiles);

        return $@"
WITH percentile_values AS (
    SELECT 
        percentile_cont(ARRAY[{percentileList}]) WITHIN GROUP (ORDER BY {valueExpression}) AS percentiles
    FROM source_data
)
SELECT 
    CASE 
        WHEN bin_number = 0 THEN {minValue}
        ELSE percentiles[bin_number]
    END AS bin_min,
    CASE 
        WHEN bin_number = array_length(percentiles, 1) THEN {maxValue}
        ELSE percentiles[bin_number + 1]
    END AS bin_max,
    bin_number
FROM percentile_values
CROSS JOIN generate_series(0, array_length(percentiles, 1)) AS bin_number";
    }
}
