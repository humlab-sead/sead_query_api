using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Mocks;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class RepositoryRegistryTests
    {
        private RepositoryRegistry CreateRepositoryRegistry()
        {
            var mockContext = JsonSeededFacetContextFactory.Create();
            return new RepositoryRegistry(mockContext);
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

        [Fact(Skip = "Not implemented")]
        public void QueryRow_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var repositoryRegistry = this.CreateRepositoryRegistry();
            string sql = null;

            // Act
            var result = repositoryRegistry.QueryRow<Facet>(
                sql,
                null);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void Query_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var repositoryRegistry = this.CreateRepositoryRegistry();
            string sql = null;

            // Act
            var result = repositoryRegistry.Query(
                sql);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void QueryRows_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var repositoryRegistry = this.CreateRepositoryRegistry();
            string sql = null;

            // Act
            var result = repositoryRegistry.QueryRows<Facet>(
                sql,
                null);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void QueryKeyValues2_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var repositoryRegistry = this.CreateRepositoryRegistry();
            string sql = null;
            int keyIndex = 0;
            int valueIndex1 = 0;
            int valueIndex2 = 0;

            // Act
            var result = repositoryRegistry.QueryKeyValues2<int,string>(
                sql,
                keyIndex,
                valueIndex1,
                valueIndex2);

            // Assert
            Assert.True(false);
        }
    }
}
