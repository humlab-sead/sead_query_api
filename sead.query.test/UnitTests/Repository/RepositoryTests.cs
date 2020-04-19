using Autofac;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("JsonSeededFacetContext")]
    public class RepositoryTests : DisposableFacetContextContainer
    {
        public RepositoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }


        [Fact]
        public void TestResolveUnitOfWork()
        {
            var builder = new ContainerBuilder();
            var setting = (ISetting)new SettingFactory().Create().Value;

            builder.RegisterInstance(setting).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<IFacetSetting>(setting.Facet).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<StoreSetting>(setting.Store).SingleInstance().ExternallyOwned();

            builder.RegisterInstance(FacetContext).As<IFacetContext>().SingleInstance();
            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>();

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IRepositoryRegistry>();
                Assert.True(service.Facets.GetAll().Any());
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void Get_ExpectedBehavior(int id)
        {
            // Arrange
            var repository = new Repository<Facet, int>(FacetContext);

            // Act
            var result = repository.Get(id);

            // Assert
            Assert.Equal(id, result.FacetId);
        }

        [Theory]
        [InlineData(typeof(Facet))]
        [InlineData(typeof(Table))]
        public void GetAll_OnVariousRepositories_Success(Type type)
        {
            // Arrange
            Type[] typeArgs = { type, typeof(int) };
            Type repoType = typeof(Repository<,>).MakeGenericType(typeArgs);
            var repository = Activator.CreateInstance(repoType, FacetContext);
            var methodInfo = repoType.GetMethod("GetAll");
            // Act
            IEnumerable<object> result = (IEnumerable<object>)methodInfo.Invoke(repository, null);

            // Assert
            Assert.True(result.Any());
        }

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
