using System.Threading.Tasks;

namespace SeadQueryComposer.QueryComposer.Compilers;

/// <summary>
/// Resolver for discrete facets (e.g., sample groups, taxa, countries).
/// Handles IN clause filtering with user-selected picks.
/// </summary>
public class DiscreteContentCompiler : IContentCompiler
{
    public EFacetType FacetType => EFacetType.Discrete;

    public Task<string> ResolvePredicateAsync(Facet facet, FacetConfig2 facetConfig, string anchorName, object additionalPayload = null)
    {
        if (!facetConfig.HasPicks())
        {
            return Task.FromResult(string.Empty);
        }

        var pickValues = facetConfig.GetPickValues();
        var picksList = string.Join(", ", pickValues);

        var template = facet.GetTemplate(anchorName);

        // Replace the @picks placeholder with actual values
        var resolvedSql = template?.SqlTemplate.Replace("@picks", $"({picksList})");

        return Task.FromResult(resolvedSql);
    }

    public Task<string> ResolveContentQueryAsync(
        Facet facet,
        string filteredAnchorIdsQuery,
        string anchorName,
        object additionalPayload = null
    )
    {
        var sql =
            $@"
WITH filtered_anchor_ids AS (
    {filteredAnchorIdsQuery}
)
SELECT
    {facet.CategoryIdExpr} AS CategoryId,
    {facet.CategoryNameExpr} AS CategoryName,
    COUNT(DISTINCT fa.anchor_id) AS ItemCount
FROM
    filtered_anchor_ids fa";

        // Add the necessary joins to reach the target entity
        sql += BuildJoinsToTargetEntity(facet, anchorName);

        sql +=
            $@"
GROUP BY {facet.CategoryIdExpr}, {facet.CategoryNameExpr}
ORDER BY {facet.CategoryNameExpr}";

        return Task.FromResult(sql);
    }
}
