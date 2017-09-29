using QuerySeadDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;

namespace QuerySeadAPI.Services
{
    public interface ILoadFacetService {
        FacetContent Load(FacetsConfig2 facetsConfig);
    }

    public class LoadFacetService : AppServiceBase, ILoadFacetService {

        public IDeleteBogusPickService BogusPickService { get; private set; }
        public IIndex<EFacetType, IFacetContentService> ContentServices { get; private set; }

        public LoadFacetService(IQueryBuilderSetting config, IUnitOfWork context, IQueryCache cache,
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

        public CachedLoadFacetService(IQueryBuilderSetting config, IUnitOfWork context, IQueryCache cache,
            IDeleteBogusPickService bogusService, IIndex<EFacetType, IFacetContentService> services) : base(config, context, cache, bogusService, services)
        {
        }

        public override FacetContent Load(FacetsConfig2 facetsConfig)
        {
            var cacheId = facetsConfig.GetCacheId();
            if (ContentCache.Exists(cacheId))
                return ContentCache.Get(cacheId);
            var facetContent = base.Load(facetsConfig);
            ConfigCache.Put(cacheId, facetsConfig);
            ContentCache.Put(cacheId, facetContent);
            return facetContent;
        }
    }
}
