using Moq;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetsConfig2Tests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IRepositoryRegistry> mockRepositoryRegistry;

        public FacetsConfig2Tests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetsConfig2 CreateFacetsConfig2()
        {
            return new FacetsConfig2(
                this.mockRepositoryRegistry.Object);
        }

        [Fact]
        public void CanCreateSimpleConfig()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register()) {

                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);

                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");

                Assert.Equal("sites", facetsConfig.TargetCode);
                Assert.Equal(facetsConfig.TargetCode, facetsConfig.TargetFacet.FacetCode);
            }
        }

        [Fact]
        public void CanCreateSimpleConfigByJSON()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register()) {
                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);
                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");
                string json1 = JsonConvert.SerializeObject(facetsConfig);
                FacetsConfig2 facetsConfig2 = JsonConvert.DeserializeObject<FacetsConfig2>(json1);
                facetsConfig2.SetContext(fixture.RepositoryRegistry);
                string json2 = JsonConvert.SerializeObject(facetsConfig);
                Assert.Equal(json1, json2);
            }
        }

        [Fact]
        public void CanGenerateSingleFacetsConfigWithoutPicks()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register()) {
                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);
                foreach (var facetCode in fixture.Data.DiscreteFacetComputeCount.Keys) {
                    FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);
                    Assert.Equal(facetCode, facetsConfig.TargetFacet.FacetCode);
                }
            }
        }

        [Fact]
        public void SetContext_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();
            IRepositoryRegistry context = null;

            // Act
            var result = facetsConfig2.SetContext(
                context);

            // Assert
            Assert.True(false);
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
        public void GetCacheId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfig2 = this.CreateFacetsConfig2();

            // Act
            var result = facetsConfig2.GetCacheId();

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
