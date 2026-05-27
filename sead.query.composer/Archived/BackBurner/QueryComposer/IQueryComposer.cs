
using System.Threading.Tasks;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer
{
    /// <summary>
    /// Defines the contract for a service that composes facet predicates into executable SQL queries.
    /// </summary>
    public interface IQueryComposer
    {
        /// <summary>
        /// Composes a query to get the category counts for a single target facet,
        /// filtered by all other active facets.
        /// </summary>
        /// <param name="facetsConfig"></param>
        /// <param name="anchorKeyName">The name of the anchor to use (e.g., "sample").</param>
        /// <param name="targetIndex"></param>
        /// <returns>A SQL query that returns category IDs, names, and counts.</returns>
        Task<string> GetFacetContentQueryAsync(FacetsConfig2 facetsConfig, string anchorKeyName, int? targetIndex = 0);
    }
}
