using System;
using CacheManager.Core;
using Microsoft.Extensions.Caching.Memory;

namespace SeadQueryInfra
{

    public class SimpleMemoryCacheProvider : CacheManagerProvider
    {
        protected override ICacheManager<object> InitCache()
        {
            var config = new CacheManager.Core.ConfigurationBuilder()
                //.WithSystemRuntimeCacheHandle()
                .WithDictionaryHandle()
                .Build();
            var cache = new CacheManager.Core.BaseCacheManager<object>(config);
            return cache;
        }
    }
}
