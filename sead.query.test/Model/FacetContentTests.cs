using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;
using static SeadQueryCore.FacetContent;

namespace SeadQueryTest.Model
{
    public class FacetContentTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<FacetsConfig2> mockFacetsConfig2;
        private Mock<List<ContentItem>> mockList;
        private Mock<Dictionary<string, CategoryCountItem>> mockCountItem;
        private Mock<Dictionary<string, FacetsConfig2.UserPickData>> mockUserPickData;

        public FacetContentTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockFacetsConfig2 = this.mockRepository.Create<FacetsConfig2>();
            this.mockList = this.mockRepository.Create<List<ContentItem>>();
            this.mockCountItem = this.mockRepository.Create<Dictionary<string, CategoryCountItem>>();
            this.mockUserPickData = this.mockRepository.Create<Dictionary<string, FacetsConfig2.UserPickData>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetContent CreateFacetContent()
        {
            int intervalCount = 120;
            string intervalQuery = "";
            return new FacetContent(
                this.mockFacetsConfig2.Object,
                this.mockList.Object,
                this.mockCountItem.Object,
                this.mockUserPickData.Object,
                intervalCount,
                intervalQuery);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var facetContent = this.CreateFacetContent();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
