using Autofac.Features.Indexed;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;

namespace SeadQueryAPI.Services
{
    public class LoadResultService : AppServiceBase, ILoadResultService
    {
        public IResultService ResultService { get; private set; }
        private readonly ISupportedRequestPickSanitizer _pickSanitizer;

        public LoadResultService(
            ISetting config,
            IRepositoryRegistry context,
#pragma warning disable IDE0060, RCS1163
            ISeadQueryCache cache,
#pragma warning restore IDE0060, RCS1163
            IResultService service,
            ISupportedRequestPickSanitizer pickSanitizer) : base(config, context)
        {
            ResultService = service;
            _pickSanitizer = pickSanitizer;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            _pickSanitizer.Update(facetsConfig);
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
            ISupportedRequestPickSanitizer pickSanitizer) : base(config, context, cache, service, pickSanitizer)
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
