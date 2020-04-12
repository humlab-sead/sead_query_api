using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SeadQueryTest.Repository
{
    [Collection("JsonSeededFacetContext")]
    public class RepositoryRegistryTests : DisposableFacetContextContainer
        {
        public RepositoryRegistryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        //[Fact]
        //public void GetRepositoryTypes_LoadsTypes()
        //{
        //    // Arrange
        //    var registry = new RepositoryRegistry(FacetContext);
        //    // Act
        //    var result = registry.GetRepositoryTypes().ToList();

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Any());
        //}


    }
}
