using Newtonsoft.Json;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetsConfig2Tests : DisposableFacetContextContainer
    {
        private readonly MockFacetsConfigFactory FacetsConfigFactory;

        public FacetsConfig2Tests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
            FacetsConfigFactory = new MockFacetsConfigFactory(Registry);
        }

        private FacetsConfig2 CreateFacetsConfig2()
        {
            return new FacetsConfig2() {
            };
        }

        [Fact]
        public void CanCreateSimpleConfig()
        {
            using (var container = TestDependencyService.CreateContainer(Context, null))
            using (var scope = container.BeginLifetimeScope()) {

                FacetsConfig2 facetsConfig = FacetsConfigFactory.CreateSingleFacetsConfigWithoutPicks("sites");

                Assert.Equal("sites", facetsConfig.TargetCode);
                Assert.Equal(facetsConfig.TargetCode, facetsConfig.TargetFacet.FacetCode);
            }
        }

        [Fact]
        public void Reconstitute_SingleFacetsConfigWithoutPicks_IsEqual()
        {
            // Arrange
            var reconstituter = new FacetConfigReconstituteService(Registry);
            FacetsConfig2 facetsConfig = FacetsConfigFactory.CreateSingleFacetsConfigWithoutPicks("sites");
            string json1 = JsonConvert.SerializeObject(facetsConfig);
            // Act
            FacetsConfig2 facetsConfig2 = reconstituter.Reconstitute(json1);
            string json2 = JsonConvert.SerializeObject(facetsConfig2);
            // Assert
            Assert.Equal(json1, json2);
        }

        [Fact(Skip = "Not implemented")]
        public void GetConfig_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();
            string facetCode = null;

            // Act
            var result = facetsConfig2.GetConfig(
                facetCode);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void GetFacetCodes_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetFacetCodes();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void GetFacetConfigsWithPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetFacetConfigsWithPicks();

            // Assert
            Assert.True(false);
        }

        //[Fact(Skip = "Not implemented")]
        //public void GetFacetCodesWithPicks_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var facetsConfig2 = this.CreateFacetsConfig2();

        //    // Act
        //    var result = facetsConfig2.GetFacetCodesWithPicks();

        //    // Assert
        //    Assert.True(false);
        //}

        [Fact(Skip = "Not implemented")]
        public void GetFacetConfigsAffectedByFacet_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();
            List<string> facetCodes = null;
            Facet targetFacet = null;

            // Act
            var result = facetsConfig2.GetFacetConfigsAffectedBy(
                targetFacet,
                facetCodes);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void DeletePicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.ClearPicks();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void CollectUserPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();
            string onlyCode = null;

            // Act
            var result = facetsConfig2.CollectUserPicks(
                onlyCode);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void HasPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();
            EFacetType facetType = default(global::SeadQueryCore.EFacetType);

            // Act
            var result = facetsConfig2.HasPicks(
                facetType);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void GetPicksCacheId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetPicksCacheId();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void GetTargetTextFilter_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetTargetTextFilter();

            // Assert
            Assert.True(false);
        }
    }
}
