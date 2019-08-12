using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using SeadQueryCore.Model;

namespace SeadQueryAPI.Services {

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
            ICache cache,
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
            ICache cache,
            IResultServiceIndex services,
            IDeleteBogusPickService bogusPickService) : base(config, context, cache, services, bogusPickService)
        {
        }

        public override ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var configCacheId = "config_" + facetsConfig.GetCacheId();
            var resultCacheId = "result_" + resultConfig.GetCacheId(facetsConfig);
            var viewType = Context.Results.GetViewType(resultConfig.ViewTypeId);
            var result = viewType.IsCachable ? Cache.Get<ResultContentSet>(resultCacheId) : null;
            if (result == null)
            {
                result = base.Load(facetsConfig, resultConfig);
                Cache.Set<FacetsConfig2>(configCacheId, facetsConfig);
                if (Context.Results.GetViewType(resultConfig.ViewTypeId).IsCachable)
                    Cache.Set<ResultContentSet>(resultCacheId, result);
            }
            return result;
        }
    }

}