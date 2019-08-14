using CacheManager.Core;
using System;

namespace SeadQueryInfra
{
    public class RedisCacheManagerFactory
    {
        public string Hostname { get; set; } = "localhost";
        public int Port { get; set; } = 6379;

        public ICacheManagerConfiguration CreateConfig()
        {
            return ConfigurationBuilder.BuildConfiguration(settings =>
            {
                settings
                .WithJsonSerializer()
                .WithRedisConfiguration("redis", config =>
                {
                    config.WithAllowAdmin()
                    .WithDatabase(0)
                    .WithEndpoint(Hostname, Port);
                })
                .WithMaxRetries(3)
                .WithRetryTimeout(100)
                .WithRedisBackplane("redis")
                .WithRedisCacheHandle("redis", true);
            });
        }

        public ICacheManager<object> CreateManager()
        {
            return CacheFactory.FromConfiguration<object>(CreateConfig());
        }
    }

    public class RedisCacheProvider : CacheManagerProvider
    {
        protected override ICacheManager<object> InitCache()
        {
            return new RedisCacheManagerFactory().CreateManager();
        }
    }

}
