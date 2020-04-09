﻿using Autofac;
using Xunit;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;

namespace SeadQueryTest.Cache {

    public class CacheTests
    {

        protected virtual ISeadQueryCache GetCache()
        {
            //try {
            //    return new RedisCacheProvider();
            //} catch (InvalidOperationException) {
            //    Console.WriteLine("Failed to connect to Redis!");
            //}
            return new MemoryCacheFactory().Create();
        }

        [Theory]
        [InlineData("a test key", 1234)]
        [InlineData("a test key", "a test value")]
        [InlineData("a test key", true)]
        public void Set_WhenSimple_IsCached(string testKey, object expectedValue)
        {
            var cache = GetCache();
            cache.Set(testKey, expectedValue);
            var retrievedValue = cache.Get<object>(testKey);
            Assert.Equal(expectedValue, retrievedValue);
        }

        [Fact]
        public void Set_WhenComplexValue_IsCached()
        {
            var cache = GetCache();

            var expectedValue = new { A = 1, B = 2 };
            var testKey = "a test key";
            cache.Set(testKey, expectedValue);
            dynamic retrievedValue = cache.Get<object>(testKey);
            Assert.Equal(expectedValue.A, retrievedValue.A);
            Assert.Equal(expectedValue.B, retrievedValue.B);
        }

        [Fact]
        public void Resolve_CanResolveCacheService()
        {
            var options = Startup.Options;
            using (var container = TestDependencyService.CreateContainer(null, options))
            using (var scope = container.BeginLifetimeScope()) {
                // Assert.IsNotNull(scope.Resolve<ICacheManager<object>>());
                Assert.NotNull(scope.Resolve<ISeadQueryCache>());
            }
        }


    }
}
