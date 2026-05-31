using System.Collections.Generic;
using System.Linq;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryComposer;
using SQT.Infrastructure;
using Xunit;

namespace SQT.Plugins.Discrete
{
    [Collection("UsePostgresFixture")]
    public class DiscreteFacetContentServiceTests() : MockerWithFacetContext()
    {
        [Theory]
        [InlineData("sites:sites", false)]
        [InlineData("dataset_methods:dataset_methods", false)]
        public void Load_VariousDescreteFacets_Success(string uri, bool hasPicks)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var fakeValues = FakeDiscreteCategoryCountItems(5);
            var expected = new FacetContent
            {
                FacetsConfig = facetsConfig,
                Items = fakeValues,
                Distribution = fakeValues.ToDictionary(item => item.Category ?? "(null)"),
                IntervalInfo = new FacetContent.CategoryInfo(),
                SqlQuery = "select composed",
                Picks = hasPicks ? facetsConfig.CollectUserPicks(facetsConfig.TargetCode) ?? [] : [],
            };
            var composedService = new Mock<IComposedFacetContentService>(MockBehavior.Strict);
            composedService.Setup(service => service.Load(facetsConfig)).Returns(expected);

            // Act
            var service = new FacetContentService(composedService.Object);

            var result = service.Load(facetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Items.Count > 0);
            Assert.Equal(fakeValues.Count, result.Items.Count);
            Assert.Equal(hasPicks, result.Picks.Count > 0);
            composedService.Verify(service => service.Load(facetsConfig), Times.Once);
        }
    }
}
