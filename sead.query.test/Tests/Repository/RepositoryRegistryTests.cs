using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class RepositoryRegistryTests : IDisposable
    {
        private IFacetContext mockFacetContext;

        public RepositoryRegistryTests()
        {
            this.mockFacetContext = ScaffoldUtility.JsonSeededFacetContext();
        }

        public void Dispose()
        {
        }

        private RepositoryRegistry CreateRepositoryRegistry()
        {
            return new RepositoryRegistry(
                this.mockFacetContext);
        }

        [Fact]
        public void Commit_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var repositoryRegistry = this.CreateRepositoryRegistry();

            // Act
            var result = repositoryRegistry.Commit();

            // Assert
            Assert.True(false);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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
