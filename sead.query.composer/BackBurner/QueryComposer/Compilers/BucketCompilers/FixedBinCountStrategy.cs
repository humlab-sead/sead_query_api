namespace SeadQueryComposer.QueryComposer.Compilers;

/// <summary>
/// Fixed number of bins strategy
/// </summary>
public class FixedCountBucketCompiler : IBucketCompiler
{
    public string StrategyType => "fixed_bin_count";

    public string Compile(string valueExpression, decimal? minValue, decimal? maxValue, object parameters = null)
    {
        var bucketCount = parameters as int? ?? 10; // Default to 10 bins

        return $@"
SELECT 
    bin_min,
    bin_max,
    CONCAT(ROUND(bin_min, 2), ' - ', ROUND(bin_max, 2)) AS bin_label,
    bin_number
FROM (
    SELECT 
        ({minValue} + (bin_number * ({maxValue} - {minValue}) / {bucketCount})) AS bin_min,
        ({minValue} + ((bin_number + 1) * ({maxValue} - {minValue}) / {bucketCount})) AS bin_max,
        bin_number
    FROM generate_series(0, {bucketCount - 1}) AS bin_number
) bins";
    }
}
