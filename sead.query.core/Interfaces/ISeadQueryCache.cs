using System;

namespace SeadQueryCore
{
    public interface ISeadQueryCache
    {
        string KeyPrefix { get; set; }
        T Get<T>(string key);
        void Set<T>(string key, T value);
        void SetSliding<T>(string key, T value);
        void Set<T>(string key, T value, int duration);
        void SetSliding<T>(string key, T value, int duration);
        void Set<T>(string key, T value, DateTimeOffset expiration);
        bool Exists(string key);
        void Remove(string key);
        void Clear();
    }
}
