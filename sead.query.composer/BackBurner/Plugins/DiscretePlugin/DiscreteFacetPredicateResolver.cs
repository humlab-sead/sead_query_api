using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeadQueryCore.QueryComposer.Models;
using SqlKata;
using SqlKata.Compilers;

namespace SeadQueryComposer.Plugins.DiscretePlugin;

public class DiscreteFacetPredicateResolver(IRouteParser routeParser, IRepositoryRegistry registry)
    : FacetPredicateResolverBase<DiscreteFacetUserInput>(routeParser, registry)
{
    protected override async Task<string> ResolveSqlInternalAsync(
        string facetCode,
        string targetTable,
        string targetId,
        DiscreteFacetUserInput userInput,
        AnchorTemplate anchorTemplate,
        string anchorTable,
        string anchorId
    )
    {
        var baseSql = await BuildSqlFromAnchorTemplateAsync(targetTable, targetId, anchorTemplate, anchorTable, anchorId);

        // If no picks are provided, return the base SQL without additional criteria
        if (!userInput.HasPicks)
        {
            return baseSql;
        }

        // Replace @criteria with the actual WHERE clause
        var whereClause = BuildWhereClause(targetTable, targetId, userInput);
        return baseSql.Replace("@criteria", whereClause);
    }

    protected override DiscreteFacetUserInput ValidateAndCastUserInput(object userInput)
    {
        return userInput switch
        {
            DiscreteFacetUserInput discreteInput => discreteInput,
            FacetConfig2 facetConfig => new DiscreteFacetUserInput
            {
                Picks = facetConfig.GetPickValues()?.Cast<object>().ToList() ?? [],
                Operator = "in",
            },
            List<object> picks => new DiscreteFacetUserInput { Picks = picks, Operator = "in" },
            _ => throw new ArgumentException(
                $"User input must be of type {typeof(DiscreteFacetUserInput).Name}, {typeof(FacetConfig2).Name}, or List<object>",
                nameof(userInput)
            ),
        };
    }

    private string BuildWhereClause(string targetTable, string targetId, DiscreteFacetUserInput userInput)
    {
        var targetAlias = GetTableAlias(targetTable);
        var whereQuery = new Query().Where($"{targetAlias}.{targetId}", userInput.Operator, userInput.Picks);

        var compiledWhere = SqlCompiler.Compile(whereQuery);

        // Extract just the WHERE clause from the compiled query
        var whereClause = compiledWhere.ToString();
        var whereIndex = whereClause.IndexOf("WHERE ", StringComparison.OrdinalIgnoreCase);

        return whereIndex >= 0
            ? whereClause[whereIndex..]
            : $"WHERE {targetAlias}.{targetId} IN ({string.Join(", ", userInput.Picks.Select(p => $"'{p}'"))})";
    }
}
