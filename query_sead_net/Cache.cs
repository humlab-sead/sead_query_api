using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using QuerySeadDomain;
using QuerySeadDomain.Model;

namespace QuerySeadAPI
{
    public interface IQueryCache {
        ICacheManager<object> Store { get; set; }
    }

    public class QueryCacheFactory {

        public ICacheManager<object> Create()
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
            return cache;
        }
    }

    public class CacheService<T> {

        public string Prefix;
        private ICacheManager<object> Store { get; set; }

        public CacheService(IQueryCache cache, string prefix)
        {
            Store = cache.Store;
            Prefix = prefix;
        }

        public void Put(string key, T entity) => Store.Put(Prefix + key, entity);
        public bool Exists(string key)        => Store.Exists(Prefix + key);
        public T    Get(string key)           => Store.Exists(Prefix + key) ? Store.Get<T>(Prefix + key) : default(T);
        public void Clear() => Store.Clear();
    }

    public class FacetConfigCache : CacheService<FacetsConfig2> {
        public FacetConfigCache(IQueryCache cache) : base(cache, "config_") { }
    }

    public class FacetContentCache : CacheService<FacetContent> {
        public FacetContentCache(IQueryCache cache) : base(cache, "content_") { }
    }
    
    public class FaceResultCache : CacheService<ResultContentSet> {
        public FaceResultCache(IQueryCache cache) : base(cache, "result_") { }
    }
}
