using System;
using CacheManager.Core;
using Microsoft.Extensions.Caching.Memory;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class MemoryCacheFactory
    {
        public ISeadQueryCache Create()
        {
            var config = new CacheManager.Core.ConfigurationBuilder()
                //.WithSystemRuntimeCacheHandle()
                .WithDictionaryHandle()
                .Build();
            var cache = new CacheManager.Core.BaseCacheManager<object>(config);
            return new SeadQueryCache(cache);
        }
    }
}
