using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using SeadQueryCore;
using SeadQueryCore.QueryComposer.RouteCompiler;
using SqlKata;
using SqlKata.Compilers;

namespace SeadQueryComposer.QueryComposer.Archived;

public class QueryComposer(IIndex<EFacetType, IFacetPredicateResolver> resolvers) : IQueryComposer
{
    private readonly IIndex<EFacetType, IFacetPredicateResolver> _facetResolvers = resolvers;

    public async Task<string> GetFacetContentQueryAsync(FacetsConfig2 facetsConfig, string anchorKeyName, int? targetIndex = 0)
    {
        // TODO: Implement the logic to build the facet content query using CTEs.
        // This will involve:
        // 1. Identifying the filtering facets.
        // 2. Building a CTE for each filtering facet from its PredicateTemplate.
        // 3. Combining the CTEs to get a final set of Anchor IDs.
        // 4. Joining the Anchor IDs to the target facet's tables to get counts.

        // Noteworthy: derived from QuerySetupBuilder.Build method.
        // Noteworthy: targetFacet argument removed since result set is handled by result service in this design
        // Noteworthy: facetCodes argument removed since facet aggregation is defined by target facet in this design.
        // Noteworthy: new argument targetIndex is used to determine the target facet based on its index in the configuration.
        //             with this improvement, the same facet can be used multiple times

        if (targetIndex != null)
        {
            targetIndex = facetsConfig.IndexOf(facetsConfig.TargetConfig);
        }

        var involvedConfigs = facetsConfig.Take((int)targetIndex + 1);
        if (facetsConfig.HasDomainCode())
            involvedConfigs.Insert(0, facetsConfig.CreateDomainConfig());

        Dictionary<string, string> CTEs = [];
        for (var i = 0; i < involvedConfigs.Count; i++)
        {
            var facetConfig = involvedConfigs[i];
            var facet = facetConfig.Facet;
            var resolver = _facetResolvers[facet.FacetType.FacetTypeId];
            if (!facetConfig.HasConstraints())
            {
                continue;
            }
            if (facet.GetTemplate(anchorKeyName) == null)
            {
                throw new ArgumentException($"Facet '{facet.FacetCode}' does not have a template for anchor key '{anchorKeyName}'.");
            }
            var sql = await resolver.ResolveSqlAsync(
                facet.FacetCode,
                facetConfig.Facet.TargetTable.TableOrUdfName,
                facetConfig.Facet.TargetTable.ResolvedTableOrUdfCall,
                facetConfig.PickValues,
                facetConfig.GetConfiguration(),
                anchorTable,
                anchorKeyName
            );
            CTEs.Add($"{anchorKeyName}_{i}", sql);
        }

        var query = AnchorIdQueryBuilder.BuildQuery(CTEs, anchorKeyName);
        return await Task.FromResult(query);
    }
}

public class AnchorIdQueryBuilder
{
    /// <summary>
    /// Constructs a SQL query using Common Table Expressions (CTEs) for each facet.
    /// It finds the final set of anchor_ids that exist in ALL facet results.
    /// </summary>
    /// <param name="facetCteSources">A dictionary where the Key is the desired CTE name (alias)
    /// and the Value is the SQL query for that facet, which must return a single column named 'anchor_id'.</param>
    /// <param name="anchorKeyName"></param>
    /// <returns>A complete, runnable SQL query string.</returns>
    public static string BuildQuery(Dictionary<string, string> facetCteSources, string anchorKeyName)
    {
        if (facetCteSources == null || facetCteSources.Count == 0)
        {
            return $"SELECT {anchorKeyName} FROM (VALUES (null)) WHERE 1=0;";
        }

        var query = new Query();

        // 1. Add each facet source as a CTE using WithRaw
        foreach (var facet in facetCteSources)
        {
            // WithRaw is perfect for when you already have the SQL for the CTE body
            query.WithRaw(facet.Key, facet.Value);
        }

        // 2. The main body of the query can now reference the CTEs.
        // We'll use Intersect.
        var firstFacetAlias = facetCteSources.Keys.First();
        var otherFacetAliases = facetCteSources.Keys.Skip(1);

        // Start the query body by selecting from the first CTE
        query.From(firstFacetAlias).Select(anchorKeyName);

        // Chain INTERSECT for all other CTEs
        foreach (var alias in otherFacetAliases)
        {
            var intersectQuery = new Query(alias).Select(anchorKeyName);
            query.Intersect(intersectQuery);
        }

        // 3. Compile the Query object to a raw SQL string for your database
        var compiler = new PostgresCompiler();
        var sqlResult = compiler.Compile(query);

        return sqlResult.Sql + ";";
    }
}
