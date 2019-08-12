using System;
using System.Configuration;

namespace SeadQueryAPI
{
    public abstract class CacheProviderBase<TCache> : ICache
    {
        public int CacheDuration { get; set; }
        protected readonly TCache Cache;
        public string KeyPrefix { get; set; } = "";

        public CacheProviderBase(string keyPrefix="", int defaultCacheDurationMinutes = 60 * 24 * 30)
        {
            CacheDuration = defaultCacheDurationMinutes;
            KeyPrefix = keyPrefix;
            Cache = InitCache();
        }

        protected abstract TCache InitCache();

        public abstract T Get<T>(string key);

        public virtual void Set<T>(string key, T value)
        {
            Set<T>(key, value, CacheDuration);
        }

        public virtual void SetSliding<T>(string key, T value)
        {
            SetSliding<T>(key, value, CacheDuration);
        }

        public abstract void Set<T>(string key, T value, int duration);

        public abstract void SetSliding<T>(string key, T value, int duration);

        public abstract void Set<T>(string key, T value, DateTimeOffset expiration);

        public abstract bool Exists(string key);

        public abstract void Remove(string key);

        public abstract void Clear();
    }

    public class NullCacheProvider : CacheProviderBase<object>
    {
        public override bool Exists(string key) => false;

        public override T Get<T>(string key) => default(T);

        public override void Remove(string key) { }

        public override void Clear() { }

        public override void Set<T>(string key, T value, int duration) { }

        public override void Set<T>(string key, T value, DateTimeOffset expiration) { }

        public override void SetSliding<T>(string key, T value, int duration) { }

        protected override object InitCache() => null;

    }
}
