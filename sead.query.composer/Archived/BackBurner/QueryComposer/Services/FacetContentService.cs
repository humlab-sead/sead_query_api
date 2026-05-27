using System.Linq;
using System.Threading.Tasks;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Services;

/// <summary>
/// A new, refactored service for loading facet content using the modern IQueryComposer.
/// </summary>
public class FacetContentService : IFacetContentService
{
    private readonly IFacetRepository _facetRepository;
    private readonly IQueryComposer _queryComposer;
    private readonly ITypedQueryProxy _queryProxy;

    public FacetContentService(IRepositoryRegistry repositoryRegistry, IQueryComposer queryComposer, ITypedQueryProxy queryProxy)
    {
        _facetRepository = repositoryRegistry.Facets;
        _queryComposer = queryComposer;
        _queryProxy = queryProxy;
    }

    public async Task<FacetContent> LoadAsync(FacetsConfig2 facetsConfig)
    {
        // For now, we assume the anchor is always "sample".
        // This could be made dynamic or part of the request DTO in the future.
        const string anchorKeyName = "sample";

        // 1. Get the SQL from the new QueryComposer. The composer encapsulates all complexity.
        var sqlQuery = await _queryComposer.GetFacetContentQueryAsync(_facetRepository, anchorKeyName);

        // 2. Execute the query. We assume the composer generates SQL that returns
        //    a consistent format that can be mapped to CategoryItem.
        var categoryItems = await _queryProxy.QueryAsync<CategoryItem>(sqlQuery);

        // 3. Collect user picks (this logic can be reused from the old model).
        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

        // 4. Assemble the FacetContent object. Note how much simpler this is.
        //    The complex 'Distribution' and 'IntervalInfo' are streamlined.
        var facetContent = new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items = categoryItems.ToList(),
            Distribution = categoryItems.ToDictionary(z => z.Category ?? "(null)"),
            SqlQuery = sqlQuery,
            Picks = userPicks ?? [],
            IntervalInfo = new FacetContent.CategoryInfo(), // Simplified in the new model
        };

        return facetContent;
    }
}
