using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;

namespace SeadQueryAPI.Services
{
    public interface ILoadFacetService
    {
        FacetContent Load(FacetsConfig2 facetsConfig);
    }

    public class LoadFacetService : AppServiceBase, ILoadFacetService
    {
        public IBogusPickService BogusPickService { get; private set; }
        public IFacetContentServiceLocator ContentServiceLocator { get; private set; }

        public LoadFacetService(ISetting config, IRepositoryRegistry context, ISeadQueryCache cache,
            IBogusPickService bogusService, IFacetContentServiceLocator contentServiceLocator) : base(config, context)
        {
            BogusPickService = bogusService;
            ContentServiceLocator = contentServiceLocator;
        }

        public virtual FacetContent Load(FacetsConfig2 facetsConfig)
        {
            facetsConfig = BogusPickService.Update(facetsConfig);
            var facetContent = ContentServiceLocator.Locate(facetsConfig.TargetFacet.FacetTypeId).Load(facetsConfig);
            return facetContent;
        }
    }

    public class CachedLoadFacetService : LoadFacetService
    {
        public CachedLoadFacetService(
            ISetting config,
            IRepositoryRegistry context,
            ISeadQueryCache cache,
            IBogusPickService bogusService,
            IFacetContentServiceLocator contentServiceLocator) : base(config, context, cache, bogusService, contentServiceLocator)
        {
            Cache = cache;
        }

        public ISeadQueryCache Cache { get; }

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
