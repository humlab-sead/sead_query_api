using System;
using System.Threading.Tasks;
using SeadQueryCore.QueryComposer.Models;
using SqlKata;

namespace SeadQueryComposer.Plugins.DiscretePlugin;

public class RangeFacetPredicateResolver : FacetPredicateResolverBase<RangeFacetUserInput>
{
    public RangeFacetPredicateResolver(IRouteParser routeParser, IRepositoryRegistry registry)
        : base(routeParser, registry) { }

    protected override async Task<string> ResolveSqlInternalAsync(
        FacetConfig2 facetConfig,
        AnchorTemplate anchorTemplate,
        string anchorTable,
        string anchorColumn
    )
    {
        var targetTable = facetConfig.Facet.TargetTable.TableOrUdfName;
        var targetColumn = facetConfig.Facet.CategoryIdExpr;
        var baseSql = await BuildSqlFromAnchorTemplateAsync(targetTable, targetColumn, anchorTemplate, anchorTable, anchorColumn);

        // If no range is provided, return the base SQL without additional criteria
        if (!userInput.HasRange)
        {
            return baseSql;
        }

        // Replace @criteria with the actual WHERE clause
        var whereClause = BuildRangeWhereClause(targetTable, targetColumn, userInput);

        return baseSql.Replace("@criteria", whereClause);
    }

    private string BuildRangeWhereClause(string targetTable, string rangeColumn, RangeFacetUserInput userInput)
    {
        var targetAlias = GetTableAlias(targetTable);
        var whereQuery = new Query();

        if (userInput.MinValue.HasValue)
        {
            whereQuery = whereQuery.Where($"{targetAlias}.{rangeColumn}", ">=", userInput.MinValue.Value);
        }

        if (userInput.MaxValue.HasValue)
        {
            whereQuery = whereQuery.Where($"{targetAlias}.{rangeColumn}", "<=", userInput.MaxValue.Value);
        }

        var compiledWhere = SqlCompiler.Compile(whereQuery);
        var whereClause = compiledWhere.ToString();
        var whereIndex = whereClause.IndexOf("WHERE ", StringComparison.OrdinalIgnoreCase);

        return whereIndex >= 0 ? whereClause[whereIndex..] : "";
    }
}
