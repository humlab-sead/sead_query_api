using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeadQueryCore;

namespace SeadQueryComposer.RouteCompiler;

public interface IRouteSqlCompiler
{
    /// <summary>
    /// Compiles a SQL query from the specified tables and columns.
    /// Given tables [S, B, ..., T] the following SQL is returned
    /// SELECT DISTINCT S.sourceColumn, T.targetColumn
    /// FROM S
    /// JOIN B ON ...
    /// ....
    /// JOIN T ON ...
    ///
    /// </summary>
    /// <param name="tables"></param>
    /// <returns></returns>
    string Compile(IReadOnlyList<string> tables);

    string Compile(IReadOnlyList<string> tables, string targetKeyColumn);

    string Compile(IReadOnlyList<string> tables, string sourceKeyColumn, string targetKeyColumn);

    string Compile(IReadOnlyList<string> tables, string sourceKeyColumn, string targetKeyColumn, IReadOnlyList<string> sourceCriteria);
}

public class RouteSqlCompiler : IRouteSqlCompiler
{
    private readonly IRouteRepository _repository;
    private readonly IRouteResolver _routeGraph;

    public RouteSqlCompiler(IRouteRepository repository, IRouteResolver routeGraph)
    {
        _repository = repository;
        _routeGraph = routeGraph;
    }

    public string Compile(IReadOnlyList<string> tables)
    {
        return Compile(tables, null, null);
    }

    public string Compile(IReadOnlyList<string> tables, string targetKeyColumn)
    {
        return Compile(tables, null, targetKeyColumn);
    }

    public string Compile(IReadOnlyList<string> tables, string sourceKeyColumn, string targetKeyColumn)
    {
        return Compile(tables, sourceKeyColumn, targetKeyColumn, []);
    }

    public string Compile(
        IReadOnlyList<string> tables,
        string sourceKeyColumn,
        string targetKeyColumn,
        IReadOnlyList<string> sourceCriteria
    )
    {
        ArgumentNullException.ThrowIfNull(tables);

        if (tables.Count < 1)
            throw new ArgumentException("At least one table is required to compile a route.");

        if (tables.Count == 1)
        {
            var route = tables[0];
            if (IsSqlQuery(route))
                return route; // Single SQL query

            // if (_routeGraph.Nodes.ContainsKey(route))
            //     throw new ArgumentException("At least two tables are required to compile a route.");
            throw new ArgumentException("At least two tables are required to compile a route.");
        }

        var resolvedRoute = _routeGraph.Resolve(tables);

        if (resolvedRoute == null || resolvedRoute.Count < 1)
            throw new InvalidOperationException($"Failed to resolve route {string.Join(" -> ", tables)}.");

        /*
            Build SQL join all TableRelation items in resolvedRoute
            Return columns is first tables primary key (source_id) and last tables primary key (target_id)
            Primary key name is given by <Table>.pk_name (i.e. _routeGraph.GetNode(table).primary_key_name)
            Format of SQL is this:
                select distinct A.pk_name as source_id, Z.pk_name as target_id
                from A
                join B on B.x = A.y -- Join condition
                join C on C.x = B.y
                ....
                join Z on Z.x = F.y
        */
        var sb = new StringBuilder();
        var sourceTable = resolvedRoute[0].SourceTable;
        var targetTable = resolvedRoute[^1].TargetTable;
        var targetAliasIndex = resolvedRoute.Count;
        var resolvedSourceKeyColumn = string.IsNullOrWhiteSpace(sourceKeyColumn) ? sourceTable.PrimaryKeyName : sourceKeyColumn;
        var resolvedTargetKeyColumn = string.IsNullOrWhiteSpace(targetKeyColumn) ? targetTable.PrimaryKeyName : targetKeyColumn;
        sb.AppendLine(
            $"select distinct X_0.{resolvedSourceKeyColumn} as source_id, X_{targetAliasIndex}.{resolvedTargetKeyColumn} as target_id"
        );
        sb.AppendLine($"from {sourceTable.Name} as X_0");
        foreach (var (edge, i) in resolvedRoute.Select((edge, i) => (edge, i)))
        {
            sb.AppendLine(
                $"join {edge.TargetTable.Name} as X_{i + 1} on X_{i + 1}.{edge.TargetColumnName} = X_{i}.{edge.SourceColumnName}"
            );
        }

        if (sourceCriteria?.Count > 0)
        {
            sb.AppendLine($"where {string.Join(" and ", sourceCriteria)}");
        }

        return sb.ToString();
    }

    public bool IsSqlQuery(string route)
    {
        return route.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
            && route.Contains("FROM", StringComparison.OrdinalIgnoreCase);
    }
}
