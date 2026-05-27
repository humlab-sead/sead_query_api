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
    string Compile(List<string> tables);
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

    public string Compile(List<string> tables)
    {
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
        sb.AppendLine(
            $"select distinct X_0.{sourceTable.PrimaryKeyName} as source_id, X_{resolvedRoute.Count - 1}.{targetTable.PrimaryKeyName} as target_id"
        );
        sb.AppendLine($"from {sourceTable.Name} as X_0");
        foreach (var (edge, i) in resolvedRoute.Select((edge, i) => (edge, i)))
        {
            sb.AppendLine(
                $"join {edge.TargetTable.Name} as X_{i + 1} on X_{i + 1}.{edge.TargetColumnName} = X_{i}.{edge.SourceColumnName}"
            );
        }
        return sb.ToString();
    }

    public bool IsSqlQuery(string route)
    {
        return route.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
            && route.Contains("FROM", StringComparison.OrdinalIgnoreCase);
    }
}
