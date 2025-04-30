using CacheManager.Core;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class MemoryCacheFactory
    {
        public ISeadQueryCache Create()
        {
            var config = new CacheConfigurationBuilder()
                //.WithSystemRuntimeCacheHandle()
                .WithDictionaryHandle()
                .Build();
            var cache = new BaseCacheManager<object>(config);
            return new SeadQueryCache(cache);
        }
    }
}
