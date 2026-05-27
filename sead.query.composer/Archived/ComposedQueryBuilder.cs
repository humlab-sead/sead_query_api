using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeadQueryComposer.QueryComposer
{
    /// <summary>
    /// Builds the composed query Q^{A^{id}}_{composed} from individual facet predicates
    /// </summary>
    public class ComposedQueryBuilder
    {
        private readonly IFacetPredicateRepository _predicateRepository;

        public ComposedQueryBuilder(IFacetPredicateRepository predicateRepository)
        {
            _predicateRepository = predicateRepository;
        }

        /// <summary>
        /// Generates the composed query Q^{A^{id}}_{composed} that returns anchor IDs
        /// filtered by all active facets using INTERSECT operations
        /// </summary>
        /// <param name="facetCodes">List of active facet codes</param>
        /// <param name="anchorName">The anchor entity name (e.g., "sample", "site")</param>
        /// <param name="facetConfigs">Configuration containing user picks for each facet</param>
        /// <returns>SQL query that returns anchor_id column</returns>
        public async Task<string> BuildComposedQueryAsync(IEnumerable<string> facetCodes, string anchorName, FacetsConfig2 facetConfigs)
        {
            var activeFacetCodes = facetCodes.ToList();

            if (!activeFacetCodes.Any())
            {
                throw new ArgumentException("At least one facet code must be provided", nameof(facetCodes));
            }

            var cteBuilder = new StringBuilder();
            var intersectBuilder = new StringBuilder();
            var parameters = new Dictionary<string, object>();
            var parameterIndex = 0;

            // Build CTEs for each active facet
            foreach (var facetCode in activeFacetCodes)
            {
                var predicate = await _predicateRepository.GetPredicateAsync(facetCode, anchorName);
                if (predicate == null)
                {
                    throw new InvalidOperationException($"No predicate found for facet '{facetCode}' with anchor '{anchorName}'");
                }

                var facetConfig = facetConfigs.FacetConfigs.FirstOrDefault(fc => fc.FacetCode == facetCode);
                if (facetConfig == null || !facetConfig.HasPicks())
                {
                    continue; // Skip facets without user selections
                }

                // Get the resolved SQL for this facet
                var resolvedSql = await ResolveFacetSqlAsync(predicate, facetConfig, ref parameterIndex);

                // Add CTE
                if (cteBuilder.Length > 0)
                {
                    cteBuilder.AppendLine(",");
                }
                cteBuilder.AppendLine($"{facetCode} (anchor_id, target_id) AS (");
                cteBuilder.AppendLine($"    {resolvedSql}");
                cteBuilder.Append(")");

                // Add to INTERSECT chain
                if (intersectBuilder.Length > 0)
                {
                    intersectBuilder.AppendLine("INTERSECT");
                }
                intersectBuilder.AppendLine($"SELECT anchor_id FROM {facetCode}");
            }

            if (cteBuilder.Length == 0)
            {
                throw new InvalidOperationException("No active facets with picks found");
            }

            // Combine CTEs and INTERSECT operations
            var finalQuery = new StringBuilder();
            finalQuery.AppendLine("WITH");
            finalQuery.AppendLine(cteBuilder.ToString());
            finalQuery.AppendLine(intersectBuilder.ToString());

            return finalQuery.ToString();
        }

        /// <summary>
        /// Resolves a facet's SQL template by substituting parameters based on facet type
        /// </summary>
        private async Task<string> ResolveFacetSqlAsync(FacetPredicate predicate, FacetConfig2 facetConfig, ref int parameterIndex)
        {
            // For now, implement basic parameter substitution
            // In a full implementation, this would delegate to facet-specific strategies
            var sql = predicate.SqlTemplate;

            if (facetConfig.HasPicks())
            {
                var picks = facetConfig.GetPickValues();
                var paramName = $"@p{parameterIndex++}";

                // Simple IN clause substitution for discrete facets
                var picksList = string.Join(", ", picks);
                sql = sql.Replace("@picks", $"({picksList})");
            }

            return sql;
        }

        /// <summary>
        /// Builds a query that excludes a specific target facet (used for facet content queries)
        /// </summary>
        public async Task<string> BuildComposedQueryExcludingFacetAsync(
            IEnumerable<string> facetCodes,
            string excludeFacetCode,
            string anchorName,
            FacetsConfig2 facetConfigs
        )
        {
            var filteredFacetCodes = facetCodes.Where(fc => fc != excludeFacetCode);

            if (!filteredFacetCodes.Any())
            {
                // If no filtering facets remain, return a query that selects all anchor IDs
                return $"SELECT {anchorName}_id AS anchor_id FROM tbl_{anchorName}s";
            }

            return await BuildComposedQueryAsync(filteredFacetCodes, anchorName, facetConfigs);
        }
    }
}
