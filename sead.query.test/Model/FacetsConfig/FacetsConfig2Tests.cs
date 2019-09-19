using Moq;
using Newtonsoft.Json;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetsConfig2Tests
    {
        private RepositoryRegistry mockRegistry;

        public object ReconstituteFacetConfigService { get; private set; }

        public FacetsConfig2Tests()
        {
            this.mockRegistry = new RepositoryRegistry(ScaffoldUtility.DefaultFacetContext());
        }

        private FacetsConfig2 CreateFacetsConfig2()
        {
            return new FacetsConfig2() {
            };
        }

        [Fact]
        public void CanCreateSimpleConfig()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register()) {

                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(mockRegistry);

                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");

                Assert.Equal("sites", facetsConfig.TargetCode);
                Assert.Equal(facetsConfig.TargetCode, facetsConfig.TargetFacet.FacetCode);
            }
        }

        [Fact]
        public void Reconstitute_SingleFacetsConfigWithoutPicks_IsEqual()
        {
            // Arrange
            var context = ScaffoldUtility.DefaultFacetContext();
            var reconstituter = new FacetConfigReconstituteService(mockRegistry);
            var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(mockRegistry);
            FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");
            string json1 = JsonConvert.SerializeObject(facetsConfig);
            FacetsConfig2 facetsConfig2 = reconstituter.Reconstitute(json1);
            string json2 = JsonConvert.SerializeObject(facetsConfig2);
            Assert.Equal(json1, json2);
        }

        [Fact]
        public void CanGenerateSingleFacetsConfigWithoutPicks()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register()) {
                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(mockRegistry);
                foreach (var facetCode in fixture.Data.DiscreteFacetComputeCount.Keys) {
                    FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);
                    Assert.Equal(facetCode, facetsConfig.TargetFacet.FacetCode);
                }
            }
        }

        [Fact]
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

        [Fact]
        public void GetFacetCodes_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetFacetCodes();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetFacetConfigsWithPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetFacetConfigsWithPicks();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetFacetCodesWithPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetFacetCodesWithPicks();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetFacetConfigsAffectedByFacet_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();
            List<string> facetCodes = null;
            Facet targetFacet = null;

            // Act
            var result = facetsConfig2.GetFacetConfigsAffectedByFacet(
                facetCodes,
                targetFacet);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void DeletePicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.DeletePicks();

            // Assert
            Assert.True(false);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
        public void GetPicksCacheId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetPicksCacheId();

            // Assert
            Assert.True(false);
        }

        [Fact]
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
