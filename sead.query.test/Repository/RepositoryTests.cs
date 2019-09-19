using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Linq.Expressions;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class RepositoryTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<IFacetContext> mockFacetContext;

        public RepositoryTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockFacetContext =

            mockFacetContext = MockFacetContext();
        }

        private Mock<IFacetContext> MockFacetContext()
        {
            Mock<IFacetContext> mockFacetContext = mockRepository.Create<IFacetContext>();
            IFacetContext testContext = ScaffoldUtility.DefaultFacetContext();
            mockFacetContext.Setup(
                z => z.Facets
            ).Returns(
                testContext.Facets
            );
            return mockFacetContext;
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        //private SeadQueryInfra.Repository<T, K> CreateRepository<T, K>() where T: class
        //{
        //    return new SeadQueryInfra.Repository<T, K>(
        //        this.mockFacetContext.Object
        //    );
        //}

        //[Fact]
        //public void Get_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository<Facet, int>();
        //    int id = 1;

        //    // Act
        //    var result = repository.Get(id);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void GetAll_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();

        //    // Act
        //    var result = repository.GetAll();

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void Find_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();
        //    Expression predicate = null;

        //    // Act
        //    var result = repository.Find(
        //        predicate);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void Add_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();
        //    TEntity entity = null;

        //    // Act
        //    repository.Add(
        //        entity);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void AddRange_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();
        //    IEnumerable entities = null;

        //    // Act
        //    repository.AddRange(
        //        entities);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void Remove_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();
        //    TEntity entity = null;

        //    // Act
        //    repository.Remove(
        //        entity);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void RemoveRange_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();
        //    IEnumerable entities = null;

        //    // Act
        //    repository.RemoveRange(
        //        entities);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void QueryRow_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();
        //    string sql = null;
        //    Func selector = null;

        //    // Act
        //    var result = repository.QueryRow(
        //        sql,
        //        selector);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void QueryRows_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var repository = this.CreateRepository();
        //    string sql = null;
        //    Func selector = null;

        //    // Act
        //    var result = repository.QueryRows(
        //        sql,
        //        selector);

        //    // Assert
        //    Assert.True(false);
        //}
    }
}
