using SeadQueryCore;

namespace SeadQueryAPI.Services
{
    public interface ILoadFacetService
    {
        FacetContent Load(FacetsConfig2 facetsConfig);
    }

    public class LoadFacetService(ISetting config, IRepositoryRegistry context,
        ISupportedRequestPickSanitizer pickSanitizer, IFacetContentService contentService) : AppServiceBase(config, context), ILoadFacetService
    {
        public ISupportedRequestPickSanitizer PickSanitizer { get; private set; } = pickSanitizer;
        public IFacetContentService ContentService { get; private set; } = contentService;

        public virtual FacetContent Load(FacetsConfig2 facetsConfig)
        {
            facetsConfig = PickSanitizer.Update(facetsConfig);
            var facetContent = ContentService.Load(facetsConfig);
            return facetContent;
        }
    }

    public class LoadFacetWithCachingService(
        ISetting config,
        IRepositoryRegistry context,
        ISeadQueryCache cache,
        ISupportedRequestPickSanitizer pickSanitizer,
        IFacetContentService contentService) : LoadFacetService(config, context, pickSanitizer, contentService)
    {
        public ISeadQueryCache Cache { get; } = cache;

        public override FacetContent Load(FacetsConfig2 facetsConfig)
        {
            var cacheId = facetsConfig.GetCacheId();
            var configCacheId = $"config_{cacheId}";
            var contentCacheId = $"content_{cacheId}";
            if (Cache.Exists(contentCacheId))
                return Cache.Get<FacetContent>(contentCacheId);
            var facetContent = base.Load(facetsConfig);
            Cache.Set<FacetsConfig2>(configCacheId, facetsConfig);
            Cache.Set<FacetContent>(contentCacheId, facetContent);
            return facetContent;
        }
    }
}
