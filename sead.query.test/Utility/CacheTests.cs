using Autofac;
using Xunit;
using SeadQueryCore;
using SeadQueryInfra;

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
            return new SimpleMemoryCacheProvider();
        }

        [Fact]
        public void CanCacheSimpleValue()
        {
            const int expectedValue = 1234;
            var testKey = "a test key";
            var cache = GetCache();
            cache.Set(testKey, expectedValue);
            var retrievedValue = cache.Get<int>(testKey);
            Assert.Equal(expectedValue, retrievedValue);
        }

        [Fact]
        public void CanCacheComplexValue()
        {
            var expectedValue = new { A = 1, B = 2 };
            var testKey = "a test key";
            var cache = GetCache();
            cache.Set(testKey, expectedValue);
            var retrievedValue = cache.Get<object>(testKey);
            Assert.Equal(expectedValue, retrievedValue);
        }

        [Fact]
        public void CanCacheComplexValue2()
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
        public void CanResolveCacheService()
        {
            var options = Startup.Options;
            var container = new TestDependencyService().Register(null, options);
            using (var scope = container.BeginLifetimeScope()) {
                // Assert.IsNotNull(scope.Resolve<ICacheManager<object>>());
                Assert.NotNull(scope.Resolve<ISeadQueryCache>());
            }
        }


    }
}
