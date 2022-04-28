using Autofac;
using Xunit;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using Moq;

namespace SQT.Infrastructure
{
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
            const string testKey = "a test key";
            cache.Set(testKey, expectedValue);
            dynamic retrievedValue = cache.Get<object>(testKey);
            Assert.Equal(expectedValue.A, retrievedValue.A);
            Assert.Equal(expectedValue.B, retrievedValue.B);
        }

        [Fact]
        public void Resolve_CanResolveCacheService()
        {
            var settingsMock = new Mock<ISetting>();
            settingsMock.Setup(x => x.Facet).Returns(new FacetSetting());
            settingsMock.Setup(x => x.Store).Returns(new StoreSetting());
            using (var container = DependencyService.CreateContainer(null, ScaffoldUtility.JsonDataFolder(), settingsMock.Object))
            using (var scope = container.BeginLifetimeScope())
            {
                Assert.NotNull(scope.Resolve<ISeadQueryCache>());
            }
        }
    }
}
