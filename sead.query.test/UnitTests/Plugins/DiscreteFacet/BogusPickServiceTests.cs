using Moq;
using SeadQueryCore.Plugin.Discrete;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System.Collections.Generic;
using Xunit;

namespace SQT.Plugins.Discrete
{
    [Collection("UsePostgresDockerSession")]
    public class DeleteBogusPickServiceTests() : MockerWithFacetContext()
    {
        protected virtual Mock<IValidPicksSqlCompiler> MockValidPickCompiler(string returnSql)
        {
            var mockCompiler = new Mock<IValidPicksSqlCompiler>();
            mockCompiler
                .Setup(z => z.Compile(It.IsAny<QuerySetup>(), It.IsAny<List<int>>()))
                .Returns(returnSql);
            return mockCompiler;
        }

        //[Theory]
        //[InlineData("sites:sites")]
        //public void Delete_StateUnderTest_ExpectedBehavior(string uri)
        //{
        //    //// Arrange
        //    var fakeQuerySetup = FakeQuerySetup(uri);
        //    var mockValidPicksCompiler = MockValidPickCompiler("#SQL-QUERY#");
        //    List<CategoryCountItem> fakeValues = null;
        //    var mockQueryProxy = MockTypedQueryProxy(fakeValues);

        //    var service = new BogusPickService(FakeRegistry(), fakeQuerySetup, mockValidPicksCompiler.Object, mockQueryProxy.Object);
        //    //// Act
        //    //var result = service.Delete(
        //    //    facetsConfig);

        //    //// Assert
        //    //Assert.True(false);
        //}

    }
}
