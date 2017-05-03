using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;

namespace QuerySeadAPI
{
    public interface IQueryCache : ICacheManager<object> { }

    public class QueryCacheFactory {

        public IQueryCache Create()
        {
            var cacheConfig = ConfigurationBuilder.BuildConfiguration(settings => {
                settings
                    .WithJsonSerializer()
                    .WithRedisConfiguration("redis", config => {
                        config.WithAllowAdmin()
                        .WithDatabase(0)
                        .WithEndpoint("localhost", 6379);
                    })
                    .WithMaxRetries(3)
                    .WithRetryTimeout(100)
                    .WithRedisBackplane("redis")
                    .WithRedisCacheHandle("redis", true);
            });
            ICacheManager<object> cache = CacheFactory.FromConfiguration<object>(cacheConfig);
            return (IQueryCache)cache;
        }
    }
}
