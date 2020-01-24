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
    public class FacetsConfig2Tests
    {
        public object ReconstituteFacetConfigService { get; private set; }

        private FacetsConfig2 CreateFacetsConfig2()
        {
            return new FacetsConfig2() {
            };
        }

        [Fact]
        public void CanCreateSimpleConfig()
        {
            using (var context = JsonSeededFacetContextFactory.Create())
            using (var registry = new RepositoryRegistry(context))
            using (var container = TestDependencyService.CreateContainer(context, null))
            using (var scope = container.BeginLifetimeScope()) {

                var fixture = new FacetsConfigFactory(registry);

                FacetsConfig2 facetsConfig = fixture.CreateSingleFacetsConfigWithoutPicks("sites");

                Assert.Equal("sites", facetsConfig.TargetCode);
                Assert.Equal(facetsConfig.TargetCode, facetsConfig.TargetFacet.FacetCode);
            }
        }

        [Fact]
        public void Reconstitute_SingleFacetsConfigWithoutPicks_IsEqual()
        {
            // Arrange
            var registry = FakeFacetsGetByCodeRepositoryFactory.Create();
            var reconstituter = new FacetConfigReconstituteService(registry);
            FacetsConfig2 facetsConfig = new FacetsConfigFactory().CreateSingleFacetsConfigWithoutPicks("sites");
            string json1 = JsonConvert.SerializeObject(facetsConfig);
            // Act
            FacetsConfig2 facetsConfig2 = reconstituter.Reconstitute(json1);
            string json2 = JsonConvert.SerializeObject(facetsConfig2);
            // Assert
            Assert.Equal(json1, json2);
        }

        [Fact(Skip = "To be removed since it only tests test code")]
        public void CanGenerateSingleFacetsConfigWithoutPicks()
        {
            var data = new Mocks.FacetConfigsByUriFixtures();

            var fixture = new FacetsConfigFactory();

            foreach (var facetCode in data.DiscreteFacetComputeCount.Keys) {

                FacetsConfig2 facetsConfig = fixture.CreateSingleFacetsConfigWithoutPicks(facetCode);

                Assert.Equal(facetCode, facetsConfig.TargetFacet.FacetCode);

            }

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

        [Fact(Skip = "Not implemented")]
        public void GetFacetCodesWithPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetFacetCodesWithPicks();

            // Assert
            Assert.True(false);
        }

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
