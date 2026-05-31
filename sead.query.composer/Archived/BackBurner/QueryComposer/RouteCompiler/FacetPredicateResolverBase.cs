using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeadQueryCore;
using SqlKata;
using SqlKata.Compilers;

namespace SeadQueryComposer.QueryComposer.RouteCompiler;

public abstract class FacetPredicateResolverBase<TUserInput> : IFacetPredicateResolver
{
    protected readonly IRouteParser RouteParser;
    protected readonly IRepositoryRegistry RepositoryRegistry;
    protected readonly PostgresCompiler SqlCompiler;

    protected FacetPredicateResolverBase(IRouteParser routeParser, IRepositoryRegistry repositoryRegistry)
    {
        RouteParser = routeParser;
        RepositoryRegistry = repositoryRegistry;
        SqlCompiler = new PostgresCompiler();
    }

    public async Task<string> ResolveSqlAsync(FacetConfig2 facetConfig, AnchorTemplate anchorTemplate, string anchorTable, string anchorId)
    {
        //var typedUserInput = ValidateAndCastUserInput(userInput);

        return await ResolveSqlInternalAsync(facetConfig, anchorTemplate, anchorTable, anchorId);
    }

    protected abstract Task<string> ResolveSqlInternalAsync(
        FacetConfig2 facetConfig,
        AnchorTemplate anchorTemplate,
        string anchorTable,
        string anchorId
    );

    protected virtual TUserInput ValidateAndCastUserInput(object userInput)
    {
        if (userInput is not TUserInput typedInput)
            throw new ArgumentException($"User input must be of type {typeof(TUserInput).Name}", nameof(userInput));

        return typedInput;
    }

    protected async Task<string> BuildSqlFromAnchorTemplateAsync(
        string targetTable,
        string targetId,
        AnchorTemplate anchorTemplate,
        string anchorTable,
        string anchorId
    )
    {
        // Priority: Explicit SQL > Route > Auto-discovery
        if (!string.IsNullOrEmpty(anchorTemplate.ExplicitSql))
        {
            return anchorTemplate.ExplicitSql;
        }

        if (anchorTemplate.Route.Any())
        {
            return await BuildSqlFromRouteAsync(
                targetTable,
                targetId,
                anchorTemplate.Route,
                anchorTable,
                anchorId,
                anchorTemplate.RequiresDistinct
            );
        }

        // Fallback: Auto-discovery using existing TableRelation system
        // var autoPath = await RelationRepository.FindShortestPathAsync(targetTable, anchorTable);
        // return await BuildSqlFromTableChainAsync(targetTable, targetId, autoPath, anchorTable, anchorId, anchorTemplate.RequiresDistinct);
        throw new NotImplementedException("Auto-discovery path is not implemented yet.");
    }

    protected async Task<string> BuildSqlFromRouteAsync(
        string targetTable,
        string targetId,
        List<string> route,
        string anchorTable,
        string anchorId,
        bool requiresDistinct
    )
    {
        var resolvedTableChain = await RouteParser.ResolveRouteAsync(route);
        return await BuildSqlFromTableChainAsync(targetTable, targetId, resolvedTableChain, anchorTable, anchorId, requiresDistinct);
    }

    protected async Task<string> BuildSqlFromTableChainAsync(
        string targetTable,
        string targetId,
        List<string> tableChain,
        string anchorTable,
        string anchorId,
        bool requiresDistinct
    )
    {
        var query = new Query();
        var targetAlias = GetTableAlias(targetTable);
        var anchorAlias = GetTableAlias(anchorTable);

        // Build the main query
        query = query.From($"{targetTable} as {targetAlias}");

        // Add target_id and anchor_id selections
        query = query.Select($"{targetAlias}.{targetId} as target_id");

        if (tableChain.Any())
        {
            // Add joins for the table chain
            string previousTable = targetTable;
            string previousAlias = targetAlias;

            foreach (var table in tableChain)
            {
                var alias = GetTableAlias(table);
                var joinCondition = await GetJoinConditionAsync(previousTable, table);

                query = query.Join($"{table} as {alias}", joinCondition.FromColumn, joinCondition.ToColumn);

                previousTable = table;
                previousAlias = alias;
            }

            // The anchor_id comes from the last table in the chain
            var finalAlias = GetTableAlias(tableChain.Last());
            query = query.Select($"{finalAlias}.{anchorId} as anchor_id");
        }
        else
        {
            // Identity case - target and anchor are the same
            query = query.Select($"{targetAlias}.{anchorId} as anchor_id");
        }

        if (requiresDistinct)
        {
            query = query.Distinct();
        }

        // Add criteria placeholder - this will be replaced by the specific resolver
        var sql = SqlCompiler.Compile(query).ToString();
        return sql + Environment.NewLine + "@criteria";
    }

    protected virtual string GetTableAlias(string tableName)
    {
        // Create short alias from table name
        if (tableName.StartsWith("tbl_"))
        {
            var withoutPrefix = tableName[4..];
            var parts = withoutPrefix.Split('_');
            return parts.Length > 1
                ? string.Join("", parts.Select(p => p.FirstOrDefault())).ToLower()
                : withoutPrefix[..Math.Min(3, withoutPrefix.Length)].ToLower();
        }

        return tableName[..Math.Min(3, tableName.Length)].ToLower();
    }

    protected async Task<JoinCondition> GetJoinConditionAsync(string fromTable, string toTable)
    {
        var relation = await RelationRepository.GetRelationAsync(fromTable, toTable);
        if (relation != null)
        {
            var fromAlias = GetTableAlias(fromTable);
            var toAlias = GetTableAlias(toTable);
            return new JoinCondition($"{fromAlias}.{relation.SourceKey}", $"{toAlias}.{relation.ForeignKey}");
        }

        // Fallback to convention-based join
        var commonKey = await InferCommonKeyAsync(fromTable, toTable);
        var fromAliasDefault = GetTableAlias(fromTable);
        var toAliasDefault = GetTableAlias(toTable);
        return new JoinCondition($"{fromAliasDefault}.{commonKey}", $"{toAliasDefault}.{commonKey}");
    }

    protected virtual async Task<string> InferCommonKeyAsync(string fromTable, string toTable)
    {
        // Convention-based inference - this should be implemented based on your database schema
        // For now, return a common pattern
        await Task.CompletedTask;

        // Try to infer from table names
        if (fromTable.Contains("site") && toTable.Contains("site"))
            return "site_id";
        if (fromTable.Contains("sample") && toTable.Contains("sample"))
            return "sample_group_id";

        return "id";
    }

    protected record JoinCondition(string FromColumn, string ToColumn);
}
