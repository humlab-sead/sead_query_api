using System.Threading.Tasks;

namespace SeadQueryComposer.QueryComposer.Compilers;

/// <summary>
/// Binning strategy interface for range facets
/// </summary>
public interface IBucketCompiler
{
    /// <summary>
    /// The type of binning strategy
    /// </summary>
    string StrategyType { get; }

    /// <summary>
    /// Generates SQL for creating bins based on the strategy
    /// </summary>
    /// <param name="valueExpression">The SQL expression for the value to bin</param>
    /// <param name="minValue">Minimum value in the dataset</param>
    /// <param name="maxValue">Maximum value in the dataset</param>
    /// <param name="parameters">Strategy-specific parameters</param>
    /// <returns>SQL that returns bin_min, bin_max, bin_label</returns>
    string Compile(string valueExpression, decimal? minValue, decimal? maxValue, object parameters = null);
}
