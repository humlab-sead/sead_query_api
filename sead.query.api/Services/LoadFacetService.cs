using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;

namespace SeadQueryAPI.Services
{
    public interface ILoadFacetService {
        FacetContent Load(FacetsConfig2 facetsConfig);
    }

    public class LoadFacetService : AppServiceBase, ILoadFacetService {

        public IDeleteBogusPickService BogusPickService { get; private set; }
        public IIndex<EFacetType, IFacetContentService> ContentServices { get; private set; }

        public LoadFacetService(IQueryBuilderSetting config, IUnitOfWork context, ICache cache,
            IDeleteBogusPickService bogusService, IIndex<EFacetType, IFacetContentService> services) : base(config, context, cache)
        {
            BogusPickService = bogusService;
            ContentServices = services;
        }

        public virtual FacetContent Load(FacetsConfig2 facetsConfig)
        {
            facetsConfig.SetContext(Context);
            facetsConfig = BogusPickService.Delete(facetsConfig);
            var facetContent = ContentServices[facetsConfig.TargetFacet.FacetTypeId].Load(facetsConfig);
            return facetContent;
        }
    }

    public class CachedLoadFacetService : LoadFacetService
    {

        public CachedLoadFacetService(IQueryBuilderSetting config, IUnitOfWork context, ICache cache,
            IDeleteBogusPickService bogusService, IIndex<EFacetType, IFacetContentService> services) : base(config, context, cache, bogusService, services)
        {
        }

        public override FacetContent Load(FacetsConfig2 facetsConfig)
        {
            var cacheId = facetsConfig.GetCacheId();
            var configCacheId = "config_{cacheId}";
            var contentCacheId = "content_{cacheId}";
            if (Cache.Exists(contentCacheId))
                return Cache.Get<FacetContent>(contentCacheId);
            var facetContent = base.Load(facetsConfig);
            Cache.Set<FacetsConfig2>(configCacheId, facetsConfig);
            Cache.Set<FacetContent>(contentCacheId, facetContent);
            return facetContent;
        }
    }
}
