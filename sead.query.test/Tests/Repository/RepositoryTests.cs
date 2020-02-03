using SeadQueryInfra;
using SeadQueryTest.Mocks;
using System;

namespace SeadQueryTest.Repository
{
    public class RepositoryTests : IDisposable
    {
        private FacetContext mockContext;

        public RepositoryTests()
        {
            mockContext = JsonSeededFacetContextFactory.Create();
        }


        public void Dispose()
        {
            mockContext.Dispose();
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
