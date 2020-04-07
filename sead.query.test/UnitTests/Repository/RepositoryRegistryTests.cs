using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class RepositoryRegistryTests : DisposableFacetContextContainer
    {
        public RepositoryRegistryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private RepositoryRegistry CreateRepositoryRegistry()
        {
            return new RepositoryRegistry(Context);
        }

        [Fact(Skip="Not implemented")]
        public void Commit_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var repositoryRegistry = this.CreateRepositoryRegistry();

            // Act
            var result = repositoryRegistry.Commit();

            // Assert
            Assert.True(false);
        }

    }
}
