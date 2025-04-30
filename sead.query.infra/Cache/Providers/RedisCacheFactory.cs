using CacheManager.Core;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class RedisCacheFactory
    {
        public ISeadQueryCache Create(string hostname, int port)
        {
            var cache = CacheFactory.Build(settings =>
            {
                settings
                .WithJsonSerializer()
                .WithRedisConfiguration("redis", config =>
                {
                    config.WithAllowAdmin()
                    .WithDatabase(0)
                    .WithConnectionTimeout(3000)
                    .WithEndpoint(hostname, port);
                })
                .WithMaxRetries(3)
                .WithRetryTimeout(100)
                .WithRedisBackplane("redis")
                .WithRedisCacheHandle("redis", true);
            });

            var manager = new SeadQueryCache(cache);
            return manager;
        }
    }
}
