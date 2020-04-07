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
    public class DefaultResultServiceTests : DisposableFacetContextContainer
    {

        public DefaultResultServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact(Skip = "Not implemented")]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var compiler = new Mock<IResultCompiler>().Object;
            var queryProxy = new Mock<IDatabaseQueryProxy>().Object;

            var service = new DefaultResultService(Registry, compiler, queryProxy);

            FacetsConfig2 facetsConfig = null;
            ResultConfig resultConfig = null;

            // Act
            var result = service.Load(facetsConfig, resultConfig);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void Load_WhenOnline_IsTrue()
        {
            //using (var connection = SqliteConnectionFactory.CreateAndOpen())
            //using (var context = JsonSeededFacetContextFactory.Create(connection))
            //using (var container = TestDependencyService.CreateContainer(context, null))
            //using (var scope = container.BeginLifetimeScope()) {

            //    // Arrange
            //    var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)";
            //    var facetsConfig = JsonSeededFacetsConfigByUriFactory.Create(context, uri);
            //    var resultKeys = new List<string>() { "site_level" };
            //    var resultConfig = ResultConfigFactory.Create("tabular", resultKeys);
            //    var service = scope.ResolveKeyed<IResultService>(EFacetType.Range);

            //    // Act
            //    var resultSet = service.Load(facetsConfig, resultConfig);

            //}
        }
    }
}
