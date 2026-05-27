using System.Threading.Tasks;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Compilers
{
    /// <summary>
    /// Strategy interface for resolving facet-specific SQL predicates.
    /// Each facet type implements its own logic for handling user input and generating SQL.
    /// </summary>
    public interface IContentCompiler
    {
        /// <summary>
        /// The facet type this resolver handles
        /// </summary>
        EFacetType FacetType { get; }

        /// <summary>
        /// Resolves a facet's predicate SQL based on user input and facet-specific configuration.
        /// Returns both anchor_id and target_id columns.
        /// </summary>
        /// <param name="facet">The facet definition</param>
        /// <param name="facetConfig">User selections and configuration</param>
        /// <param name="anchorName">The anchor entity name (e.g., "sample", "site")</param>
        /// <param name="additionalPayload">Facet-type-specific configuration (e.g., binning strategy)</param>
        /// <returns>Complete SQL query that returns (anchor_id, target_id) columns</returns>
        Task<string> ResolvePredicateAsync(Facet facet, FacetConfig2 facetConfig, string anchorName, object additionalPayload = null);

        /// <summary>
        /// Resolves the content query for displaying facet categories and counts.
        /// This is used for populating the UI with available options.
        /// </summary>
        /// <param name="facet">The target facet for content</param>
        /// <param name="filteredAnchorIdsQuery">CTE query containing filtered anchor IDs</param>
        /// <param name="anchorName">The anchor entity name</param>
        /// <param name="additionalPayload">Facet-type-specific configuration</param>
        /// <returns>SQL query that returns CategoryId, CategoryName, and ItemCount</returns>
        Task<string> ResolveContentQueryAsync(
            Facet facet,
            string filteredAnchorIdsQuery,
            string anchorName,
            object additionalPayload = null
        );
    }
}
