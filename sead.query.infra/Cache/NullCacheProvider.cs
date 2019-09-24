using System;
using System.Configuration;
using SeadQueryCore;

namespace SeadQueryInfra
{
   
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
