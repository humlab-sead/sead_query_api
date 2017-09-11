using QuerySeadDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;

namespace QuerySeadAPI.Services
{
    //using IContentServiceIndex = IIndex<EFacetType, IFacetContentService>;

    public interface ILoadFacetService {
        FacetContent Load(FacetsConfig2 facetsConfig);
    }

    public class LoadFacetService : AppServiceBase, ILoadFacetService {

        public IDeleteBogusPickService BogusPickService { get; private set; }
        public IIndex<EFacetType, IFacetContentService> ContentServices { get; private set; }
        //public IFacetContentServiceAggregate ContentServices { get; private set; }

        public LoadFacetService(IQueryBuilderSetting config, IUnitOfWork context, IQueryCache cache,
            IDeleteBogusPickService bogusService, IIndex<EFacetType, IFacetContentService> services) : base(config, context, cache)
        {
            BogusPickService = bogusService;
            ContentServices = services;
        }

        public FacetContent Load(FacetsConfig2 facetsConfig)
        {
            Cache.Store.Clear();
            facetsConfig.SetContext(Context);
            var cacheId = facetsConfig.GetCacheId();
            var facetContent = ContentCache.Get(cacheId);
            if (facetContent == null) {
                facetsConfig = BogusPickService.Delete(facetsConfig);
                facetContent = ContentServices[facetsConfig.TargetFacet.FacetTypeId].Load(facetsConfig);
                ConfigCache.Put(cacheId, facetsConfig);
                ContentCache.Put(cacheId, facetContent);
            }
            return facetContent;
        }
    }
}
