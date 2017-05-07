using QuerySeadDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;

namespace QuerySeadAPI.Services
{
    public class AppServiceBase {

        public IFacetSetting Config { get; set; }
        public IUnitOfWork Context { get; set; }
        public IQueryCache Cache { get; set; }

        public CacheService<FacetsConfig2> ConfigCache { get; private set; }
        public CacheService<FacetContent> ContentCache { get; private set; }
        public CacheService<FacetResult> ResultCache { get; private set; }

        public AppServiceBase(IQueryBuilderSetting config, IUnitOfWork context, IQueryCache cache)
        {
            Config = config.Facet;
            Context = context;
            Cache = cache;

            ConfigCache = new FacetConfigCache(cache);
            ContentCache = new FacetContentCache(cache);
            ResultCache = new FaceResultCache(cache);
        }

    }
}
