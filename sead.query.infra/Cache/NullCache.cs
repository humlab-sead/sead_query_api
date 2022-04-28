using System;
using System.Configuration;
using CacheManager.Core;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class NullCache : ISeadQueryCache
    {
        public string KeyPrefix { get => ""; set { } }

        public void Clear() { }
        public bool Exists(string key) => false;

        public T Get<T>(string key) => default;

        public void Remove(string key) { }
        public void Set<T>(string key, T value) { }
        public void Set<T>(string key, T value, int duration) { }
        public void Set<T>(string key, T value, DateTimeOffset expiration) { }
        public void SetSliding<T>(string key, T value) { }
        public void SetSliding<T>(string key, T value, int duration) { }
    }
}
