using Moq;
using SeadQueryCore;
using SQT.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace SQT.Model
{
    [Collection("JsonSeededFacetContext")]
    public class FacetsConfigSpecificationTests : DisposableFacetContextContainer
    {
        public FacetsConfigSpecificationTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("country:country/sites")]
        [InlineData("sites:country@5/sites")]
        [InlineData("sites:country@5/sites@4,5")]
        [InlineData("sites:dataset_master/dataset_methods@10/country@44/sites@4,5/")]
        public void IsSatisfiedBy_StateUnderTest_ExpectedBehavior(string uri)
        {
            // Arrange
            var facetsConfigSpecification = new FacetsConfigSpecification();
            FacetsConfig2 facetsConfig = FakeFacetsConfig(uri);

            // Act
            var result = facetsConfigSpecification.IsSatisfiedBy(facetsConfig);

           // Assert
            Assert.True(result);
        }

        public static FacetsConfig2 MkFaulty(FacetsConfig2 facetsConfig, string how)
        {
            if (how.Equals("NO-CONFIGS"))
                facetsConfig.FacetConfigs.Clear();
            if (how.Equals("NO-TARGET"))
                facetsConfig.FacetConfigs.Remove(facetsConfig.TargetConfig);
            if (how.Equals("NO-REQUEST-ID"))
                facetsConfig.RequestId = "";
            if (how.Equals("NO-TARGET-FACET"))
                facetsConfig.TargetFacet = null;
            return facetsConfig;
        }

        [Theory]
        [InlineData("sites:sites", "NO-CONFIGS")]
        [InlineData("sites:sites", "NO-TARGET")]
        [InlineData("sites:sites", "NO-REQUEST-ID")]
        [InlineData("sites:sites", "NO-TARGET-FACET")]
        public void IsSatisfiedBy_WhenFaultyConfig_RaisesXception(string uri, string how)
        {
            // Arrange
            var facetsConfigSpecification = new FacetsConfigSpecification();
            FacetsConfig2 facetsConfig = FakeFacetsConfig(uri);
            facetsConfig = MkFaulty(facetsConfig, how);
            // Act
            Assert.Throws<QuerySeadException>(
                () => facetsConfigSpecification.IsSatisfiedBy(facetsConfig)
            );

        }
    }
}
