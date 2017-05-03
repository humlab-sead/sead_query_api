using CacheManager.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryFacetTest
{
    [TestClass]
    public class CacheTests {

        public ICacheManager<T> GetManager<T>()
        {
            var cache = CacheFactory.Build<T>("cacheName", settings => settings
                .WithJsonSerializer()
                .WithRedisConfiguration("redis", config => {
                        config.WithAllowAdmin()
                        .WithDatabase(0)
                        .WithEndpoint("localhost", 6379);
                    })
                .WithMaxRetries(1000)
                .WithRetryTimeout(100)
                //.WithRedisCacheHandle("redis", false)
                //.WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(30))
            );
            return cache;
        }

        public ICacheManager<object> GetManager2()
        {
            var cacheConfig = ConfigurationBuilder.BuildConfiguration(settings => {
                settings
                    .WithJsonSerializer()
                    .WithRedisConfiguration("redis", config => {
                        config.WithAllowAdmin()
                        .WithDatabase(0)
                        .WithEndpoint("localhost", 6379);
                    })
                    .WithMaxRetries(1000)
                    .WithRetryTimeout(100)
                    .WithRedisBackplane("redis")
                    .WithRedisCacheHandle("redis", true);
            });
            ICacheManager<object> cache = CacheFactory.FromConfiguration<object>(cacheConfig);
            return cache;
        }

        [TestMethod]
        public void CanCacheSimpleValue()
        {
            var expectedValue = 1234;
            var testKey = "a test key";
            var cache = GetManager<int>();
            cache.Put(testKey, expectedValue);
            var retrievedValue = cache.Get(testKey);
            Assert.AreEqual(expectedValue, retrievedValue);
        }

        [TestMethod]
        public void CanCacheComplexValue()
        {
            var expectedValue = new { A = 1, B = 2 };
            var testKey = "a test key";
            var cache = GetManager2();
            cache.Put(testKey, expectedValue);
            var retrievedValue = cache.Get(testKey);
            Assert.AreEqual(expectedValue, retrievedValue);
        }

        [TestMethod]
        public void CanCacheComplexValue2()
        {
            var cacheConfig = ConfigurationBuilder.BuildConfiguration(settings =>
            {
                settings
                    .WithJsonSerializer()
                    .WithRedisConfiguration("redis", config => {
                        config.WithAllowAdmin()
                        .WithDatabase(0)
                        .WithEndpoint("localhost", 6379);
                    })
                    .WithMaxRetries(1000)
                    .WithRetryTimeout(100)
                    .WithRedisBackplane("redis")
                    .WithRedisCacheHandle("redis", true);
            });
            ICacheManager<object> cache = CacheFactory.FromConfiguration<object>(cacheConfig);

            var expectedValue = new { A = 1, B = 2 };
            var testKey = "a test key";
            cache.Put(testKey, expectedValue);
            dynamic retrievedValue = cache.Get(testKey);
            Assert.AreEqual(expectedValue.A, retrievedValue.A);
            Assert.AreEqual(expectedValue.B, retrievedValue.B);
        }
    }
}
