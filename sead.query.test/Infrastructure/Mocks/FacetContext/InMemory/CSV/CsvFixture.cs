using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace SQT.Scaffolding;

/// <summary>
/// Fixture interface for loading seed data into FacetContext.
/// </summary>
public interface IFacetContextDataFixture
{
    /// <summary>
    /// Loads CSV data for the given folder, schema, and table map.
    /// </summary>
    IReadOnlyDictionary<Type, IEnumerable<object>> Load(
        string folder,
        string schema,
        IReadOnlyDictionary<Type, string> tableMap
    );
}

/// <summary>
/// CSV-based implementation of <see cref="IFacetContextDataFixture"/>,
/// using CsvHelper to read header-matched records in parallel.
/// </summary>
public sealed class CsvFacetContextDataFixture : IFacetContextDataFixture
{
    private readonly MethodInfo _openDeserialize;

    /// <summary>
    /// Transforms CSV header text into the form used to match CLR property names.
    /// Default: strip underscores and lowercase.
    /// </summary>
    public Func<string, string> HeaderMatcher { get; set; } =
        header => header.Replace("_", string.Empty).ToLowerInvariant();

    /// <summary>
    /// Initializes a new instance and caches the generic Deserialize method.
    /// </summary>
    public CsvFacetContextDataFixture()
    {
        var mi =
            typeof(CsvFacetContextDataFixture).GetMethod(
                nameof(Deserialize),
                BindingFlags.Instance | BindingFlags.NonPublic
            )
            ?? throw new InvalidOperationException(
                "Internal error: cannot find Deserialize method."
            );
        _openDeserialize = mi.GetGenericMethodDefinition();
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IEnumerable<object>> Load(
        string folder,
        string schema,
        IReadOnlyDictionary<Type, string> tableMap
    )
    {
        ArgumentNullException.ThrowIfNull(folder);
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(tableMap);

        var result = new ConcurrentDictionary<Type, IEnumerable<object>>();

        // Load CSVs in parallel to speed up deserialization
        Parallel.ForEach(
            tableMap,
            kvp =>
            {
                var clrType = kvp.Key;
                var tableName = kvp.Value;
                var csvPath = ResolveCsvPath(folder, schema, tableName);
                if (csvPath == null)
                    return;

                var gm = _openDeserialize.MakeGenericMethod(clrType);
                var records = (IEnumerable<object>)gm.Invoke(this, new object[] { csvPath })!;
                result[clrType] = records;
            }
        );

        // Return as a regular dictionary
        return result.ToDictionary(k => k.Key, v => v.Value);
    }

    /// <summary>
    /// Checks candidate paths and returns the first that exists, or null.
    /// </summary>
    private static string ResolveCsvPath(string folder, string schema, string tableName)
    {
        var candidates = new[]
        {
            Path.Combine(folder, schema, tableName + ".csv"),
            Path.Combine(folder, $"{schema}.{tableName}.csv"),
            Path.Combine(folder, tableName + ".csv"),
        };
        return candidates.FirstOrDefault(File.Exists);
    }

    /// <summary>
    /// Generic CSV deserialization using CsvHelper, streaming records safely.
    /// </summary>
    private IEnumerable<T> Deserialize<T>(string filename)
        where T : class, new()
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException($"CSV fixture not found: {filename}");

        // Read all records into a list inside the using block to avoid deferred enumeration issues
        using var reader = new StreamReader(filename);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null,
            PrepareHeaderForMatch = args => HeaderMatcher(args.Header),
        };
        using var csv = new CsvReader(reader, config);

        try
        {
            var records = csv.GetRecords<T>().ToList();
            return records;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to parse CSV for {typeof(T).Name} from '{filename}'",
                ex
            );
        }
    }
}
