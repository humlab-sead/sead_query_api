using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using Xunit;

namespace SeadQueryTest.Services.Result
{
    [Collection("JsonSeededFacetContext")]
    public class DefaultResultServiceTests : DisposableFacetContextContainer
    {

        public DefaultResultServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact(Skip = "Not implemented")]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var compiler = new Mock<IResultQueryCompiler>().Object;
            var queryProxy = new Mock<IDynamicQueryProxy>().Object;

            var service = new DefaultResultService(Registry, compiler, queryProxy);

            FacetsConfig2 facetsConfig = null;
            ResultConfig resultConfig = null;

            // Act
            var result = service.Load(facetsConfig, resultConfig);

            // Assert
            Assert.True(false);
        }

    }
}
