using QuerySeadDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using QuerySeadDomain.Model;

namespace QuerySeadAPI.Services {

    using IResultServiceIndex = IIndex<string, IResultService>;

    public interface ILoadResultService {
        ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig);
    }

    public class LoadResultService : AppServiceBase, ILoadResultService {

        public IResultServiceIndex ResultServices { get; private set; }
        private IDeleteBogusPickService BogusPickService;

        public LoadResultService(
            IQueryBuilderSetting config,
            IUnitOfWork context,
            IQueryCache cache,
            IResultServiceIndex services,
            IDeleteBogusPickService bogusPickService) : base(config, context, cache) {
            ResultServices = services;
            BogusPickService = bogusPickService;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            // BogusPickService.Delete(facetsConfig);
            return ResultServices[resultConfig.ViewTypeId].Load(facetsConfig, resultConfig);
        }

    }

    public class CachedLoadResultService : LoadResultService
    {

        public CachedLoadResultService(
            IQueryBuilderSetting config,
            IUnitOfWork context,
            IQueryCache cache,
            IResultServiceIndex services,
            IDeleteBogusPickService bogusPickService) : base(config, context, cache, services, bogusPickService)
        {
        }

        public override ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var cacheId = facetsConfig.GetCacheId();
            var resultCacheId = resultConfig.GetCacheId(facetsConfig);
            var viewType = Context.Results.GetViewType(resultConfig.ViewTypeId);
            var resultContent = viewType.IsCachable ? ResultCache.Get(resultCacheId) : null;
            if (resultContent == null)
            {
                resultContent = base.Load(facetsConfig, resultConfig);
                ConfigCache.Put(cacheId, facetsConfig);
                if (Context.Results.GetViewType(resultConfig.ViewTypeId).IsCachable)
                    ResultCache.Put(resultCacheId, resultContent);
            }
            return resultContent;
        }
    }

}