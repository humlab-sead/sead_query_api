using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeadQueryAPI;
using SeadQueryCore;
using SeadQueryInfra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryTest.Cache {
    [TestClass]
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

        [TestMethod]
        public void CanCacheSimpleValue()
        {
            const int expectedValue = 1234;
            var testKey = "a test key";
            var cache = GetCache();
            cache.Set(testKey, expectedValue);
            var retrievedValue = cache.Get<int>(testKey);
            Assert.AreEqual(expectedValue, retrievedValue);
        }

        [TestMethod]
        public void CanCacheComplexValue()
        {
            var expectedValue = new { A = 1, B = 2 };
            var testKey = "a test key";
            var cache = GetCache();
            cache.Set(testKey, expectedValue);
            var retrievedValue = cache.Get<object>(testKey);
            Assert.AreEqual(expectedValue, retrievedValue);
        }

        [TestMethod]
        public void CanCacheComplexValue2()
        {
            var cache = GetCache();

            var expectedValue = new { A = 1, B = 2 };
            var testKey = "a test key";
            cache.Set(testKey, expectedValue);
            dynamic retrievedValue = cache.Get<object>(testKey);
            Assert.AreEqual(expectedValue.A, retrievedValue.A);
            Assert.AreEqual(expectedValue.B, retrievedValue.B);
        }

        [TestMethod]
        public void CanResolveCacheService()
        {
            var options = Startup.Options;
            var container = new TestDependencyService().Register(null, options);
            using (var scope = container.BeginLifetimeScope()) {
                // Assert.IsNotNull(scope.Resolve<ICacheManager<object>>());
                Assert.IsNotNull(scope.Resolve<ISeadQueryCache>());
            }
        }


    }
}
