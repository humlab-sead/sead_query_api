namespace SeadQueryComposer.QueryComposer.Compilers;

/// <summary>
/// Fixed bin size strategy
/// </summary>
public class FixedSizeBucketCompiler : IBucketCompiler
{
    public string StrategyType => "fixed_bin_size";

    public string Compile(string valueExpression, decimal? minValue, decimal? maxValue, object parameters = null)
    {
        var binSize = parameters as decimal? ?? 1.0m; // Default bin size

        return $@"
SELECT 
    bin_min,
    bin_min + {binSize} AS bin_max,
    CONCAT(ROUND(bin_min, 2), ' - ', ROUND(bin_min + {binSize}, 2)) AS bin_label,
    bin_number
FROM (
    SELECT 
        ({minValue} + (bin_number * {binSize})) AS bin_min,
        bin_number
    FROM generate_series(0, FLOOR(({maxValue} - {minValue}) / {binSize})::int) AS bin_number
) bins
WHERE bin_min <= {maxValue}";
    }
}
