using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra;

/// <summary>
/// Resolves facet template SQL and template keys at runtime by querying the
/// <c>facet.facet_template</c> database table. Results are cached in-memory
/// to avoid repeated lookups for the same facet and anchor combinations.
/// </summary>
public sealed class FacetTemplateRuntimeResolver : IFacetTemplateRuntimeResolver
{
    private readonly IFacetContext _context;
    private readonly Dictionary<int, FacetTemplateRuntimeSnapshot> _templateSnapshotCache = [];
    private readonly Dictionary<(int FacetId, int AnchorId), string> _anchorSqlCache = [];
    private readonly Dictionary<int, string> _templateKeyCache = [];
    private bool? _tableExists;

    /// <summary>
    /// Initializes a new instance of the <see cref="FacetTemplateRuntimeResolver"/> class.
    /// </summary>
    /// <param name="context">The facet context used for database access.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <c>null</c>.</exception>
    public FacetTemplateRuntimeResolver(IFacetContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public FacetTemplateRuntimeSnapshot GetTemplateSnapshot(Facet facet)
    {
        ArgumentNullException.ThrowIfNull(facet);

        if (!FacetTemplateTableExists())
        {
            return FacetTemplateRuntimeSnapshot.Empty;
        }

        if (_templateSnapshotCache.TryGetValue(facet.FacetId, out var cachedSnapshot))
        {
            return cachedSnapshot;
        }

        var snapshot = LoadTemplateSnapshot(facet);
        _templateSnapshotCache[facet.FacetId] = snapshot;
        return snapshot;
    }

    /// <inheritdoc />
    public string GetAnchorSql(Facet facet, string anchorTable)
    {
        ArgumentNullException.ThrowIfNull(facet);

        if (string.IsNullOrWhiteSpace(anchorTable))
        {
            return string.Empty;
        }

        var snapshot = GetTemplateSnapshot(facet);
        if (snapshot.AnchorSqlByTable.TryGetValue(anchorTable, out var cachedSql))
        {
            return cachedSql;
        }

        if (!FacetTemplateTableExists())
        {
            return string.Empty;
        }

        var anchorId = ResolveAnchorId(facet, anchorTable);
        if (!anchorId.HasValue)
        {
            return string.Empty;
        }

        if (_anchorSqlCache.TryGetValue((facet.FacetId, anchorId.Value), out var resolvedSql))
        {
            return resolvedSql;
        }

        const string sql = """
            select sql_text
            from facet.facet_template
            where facet_id = @facet_id
              and anchor_id = @anchor_id
              and template_role = 'anchor_sql'
            limit 1
            """;

        resolvedSql = ExecuteScalar(
            sql,
            new Dictionary<string, object> { ["@facet_id"] = facet.FacetId, ["@anchor_id"] = anchorId.Value }
        );

        _anchorSqlCache[(facet.FacetId, anchorId.Value)] = resolvedSql;
        return resolvedSql;
    }

    /// <inheritdoc />
    public string GetTemplateKey(Facet facet)
    {
        ArgumentNullException.ThrowIfNull(facet);

        var snapshot = GetTemplateSnapshot(facet);
        if (!string.IsNullOrWhiteSpace(snapshot.TemplateKey))
        {
            return snapshot.TemplateKey;
        }

        if (!FacetTemplateTableExists())
        {
            return string.Empty;
        }

        if (_templateKeyCache.TryGetValue(facet.FacetId, out var cachedTemplateKey))
        {
            return cachedTemplateKey;
        }

        const string sql = """
            select template_key
            from facet.facet_template
            where facet_id = @facet_id
              and anchor_id is null
              and template_role = 'template_key'
            limit 1
            """;

        var templateKey = ExecuteScalar(sql, new Dictionary<string, object> { ["@facet_id"] = facet.FacetId });
        _templateKeyCache[facet.FacetId] = templateKey;
        return templateKey;
    }

    /// <inheritdoc />
    public bool HasAnchorSql(Facet facet, string anchorTable)
    {
        return !string.IsNullOrWhiteSpace(GetAnchorSql(facet, anchorTable));
    }

    private bool FacetTemplateTableExists()
    {
        if (_tableExists.HasValue)
        {
            return _tableExists.Value;
        }

        const string sql = "select to_regclass('facet.facet_template')::text";
        _tableExists = !string.IsNullOrWhiteSpace(ExecuteScalar(sql, new Dictionary<string, object>()));
        return _tableExists.Value;
    }

    private static int? ResolveAnchorId(Facet facet, string anchorTable)
    {
        foreach (var facetAnchor in facet.FacetAnchors ?? [])
        {
            var candidateTable = facetAnchor.Anchor?.Table?.TableOrUdfName;
            if (string.Equals(candidateTable, anchorTable, StringComparison.OrdinalIgnoreCase))
            {
                return facetAnchor.AnchorId;
            }
        }

        return null;
    }

    private FacetTemplateRuntimeSnapshot LoadTemplateSnapshot(Facet facet)
    {
        var templateKey = LoadTemplateKey(facet.FacetId);
        var baseSql = LoadBaseTemplateSql(facet.FacetId);
        var anchorSqlByTable = LoadAnchorSqlByTable(facet.FacetId);
        return new FacetTemplateRuntimeSnapshot(
            templateKey,
            baseSql.SqlText,
            baseSql.TemplateContract,
            baseSql.BaseAnchor,
            anchorSqlByTable
        );
    }

    private string LoadTemplateKey(int facetId)
    {
        const string sql = """
            select template_key
            from facet.facet_template
            where facet_id = @facet_id
              and anchor_id is null
              and template_role = 'template_key'
            limit 1
            """;

        return ExecuteScalar(sql, new Dictionary<string, object> { ["@facet_id"] = facetId });
    }

    private TemplateRow LoadBaseTemplateSql(int facetId)
    {
        const string sql = """
            select sql_text, template_contract, base_anchor
            from facet.facet_template
            where facet_id = @facet_id
              and anchor_id is null
              and template_role = 'base_sql'
            limit 1
            """;

        var dbContext =
            _context as DbContext ?? throw new InvalidOperationException("Template resolution requires a DbContext-backed facet context.");

        using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;

        var facetIdParameter = command.CreateParameter();
        facetIdParameter.ParameterName = "@facet_id";
        facetIdParameter.Value = facetId;
        command.Parameters.Add(facetIdParameter);

        if (command.Connection?.State != System.Data.ConnectionState.Open)
        {
            command.Connection?.Open();
        }

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return TemplateRow.Empty;
        }

        return new TemplateRow(
            reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
            reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
            reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
        );
    }

    private IReadOnlyDictionary<string, string> LoadAnchorSqlByTable(int facetId)
    {
        var dbContext =
            _context as DbContext ?? throw new InvalidOperationException("Template resolution requires a DbContext-backed facet context.");

        const string sql = """
            select ft.anchor_id, tbl.table_or_udf_name, ft.sql_text
            from facet.facet_template ft
            join facet.anchor a on a.anchor_id = ft.anchor_id
            join facet."table" tbl on tbl.table_id = a.table_id
            where ft.facet_id = @facet_id
              and ft.template_role = 'anchor_sql'
            """;

        using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;

        var facetIdParameter = command.CreateParameter();
        facetIdParameter.ParameterName = "@facet_id";
        facetIdParameter.Value = facetId;
        command.Parameters.Add(facetIdParameter);

        if (command.Connection?.State != System.Data.ConnectionState.Open)
        {
            command.Connection?.Open();
        }

        var anchorSqlByTable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (reader.IsDBNull(1) || reader.IsDBNull(2))
            {
                continue;
            }

            anchorSqlByTable[reader.GetString(1)] = reader.GetString(2);
        }

        return anchorSqlByTable;
    }

    private string ExecuteScalar(string sql, IReadOnlyDictionary<string, object> parameters)
    {
        var dbContext =
            _context as DbContext ?? throw new InvalidOperationException("Template resolution requires a DbContext-backed facet context.");

        using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;

        foreach (var parameter in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = parameter.Key;
            dbParameter.Value = parameter.Value;
            command.Parameters.Add(dbParameter);
        }

        if (command.Connection?.State != System.Data.ConnectionState.Open)
        {
            command.Connection?.Open();
        }

        var value = command.ExecuteScalar();
        return value == null || value == DBNull.Value ? string.Empty : value.ToString() ?? string.Empty;
    }

    private sealed record TemplateRow(string SqlText, string TemplateContract, string BaseAnchor)
    {
        public static TemplateRow Empty { get; } = new(string.Empty, string.Empty, string.Empty);
    }
}
