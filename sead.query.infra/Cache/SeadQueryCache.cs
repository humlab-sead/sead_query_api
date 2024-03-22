using System;
using CacheManager.Core;
using Microsoft.Extensions.Caching.Memory;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class SeadQueryCache : ISeadQueryCache
    {
        // const int defaultCacheDurationMinutes = 60 * 24 * 30;

        public int CacheDuration { get; set; }
        protected readonly ICacheManager<object> Cache;
        public string KeyPrefix { get; set; } = "";

        public SeadQueryCache(ICacheManager<object> cache, string keyPrefix = "", int defaultCacheDurationMinutes = 60 * 24 * 30)
        {
            CacheDuration = defaultCacheDurationMinutes;
            KeyPrefix = keyPrefix;
            Cache = cache;
        }

        protected virtual ICacheManager<object> InitCache()
        {
            return CacheFactory.FromConfiguration<object>("cache", null);
        }

        public virtual T Get<T>(string key)
        {
            return Cache.Get<T>(KeyPrefix + key);
        }

        public virtual void Set<T>(string key, T value, int duration)
        {
            Cache.Put(KeyPrefix + key, value);
            Cache.Expire(KeyPrefix + key, DateTime.Now.AddMinutes(duration));
        }

        public virtual void SetSliding<T>(string key, T value, int duration)
        {
            Cache.Put(KeyPrefix + key, value);
            Cache.Expire(KeyPrefix + key, new TimeSpan(0, duration, 0));
        }

        public virtual void Set<T>(string key, T value, DateTimeOffset expiration)
        {
            Cache.Put(KeyPrefix + key, value);
            Cache.Expire(KeyPrefix + key, expiration);
        }

        public virtual void Set<T>(string key, T value)
        {
            Set<T>(key, value, CacheDuration);
        }

        public virtual void SetSliding<T>(string key, T value)
        {
            SetSliding<T>(key, value, CacheDuration);
        }

        public virtual bool Exists(string key)
        {
            return Cache.Exists(KeyPrefix + key);
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(KeyPrefix + key);
        }

        public virtual void Clear()
        {
            Cache.Clear();
        }
    }
}
