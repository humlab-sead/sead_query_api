using Autofac.Features.Indexed;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;

namespace SeadQueryAPI.Services
{
    public class LoadResultService : AppServiceBase, ILoadResultService
    {
        public IResultService ResultService { get; private set; }
        private readonly IBogusPickService BogusPickService;

        public LoadResultService(
            ISetting config,
            IRepositoryRegistry context,
#pragma warning disable IDE0060, RCS1163
            ISeadQueryCache cache,
#pragma warning restore IDE0060, RCS1163
            IResultService service,
            IBogusPickService bogusPickService) : base(config, context)
        {
            ResultService = service;
            BogusPickService = bogusPickService;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            BogusPickService.Update(facetsConfig);
            return ResultService.Load(facetsConfig, resultConfig);
        }
    }

    public class LoadResultWithCachingService : LoadResultService
    {
        public LoadResultWithCachingService(
            ISetting config,
            IRepositoryRegistry context,
            ISeadQueryCache cache,
            IResultService service,
            IBogusPickService bogusPickService) : base(config, context, cache, service, bogusPickService)
        {
            Cache = cache;
        }

        public ISeadQueryCache Cache { get; }

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
