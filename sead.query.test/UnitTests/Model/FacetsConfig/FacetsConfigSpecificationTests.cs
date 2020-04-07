using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetsConfigSpecificationTests
    {

        private FacetsConfigSpecification CreateFacetsConfigSpecification()
        {
            return new FacetsConfigSpecification();
        }

        [Fact(Skip ="Not implemented")]
        public void IsSatisfiedBy_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfigSpecification = this.CreateFacetsConfigSpecification();
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = facetsConfigSpecification.IsSatisfiedBy(
                facetsConfig);

            // Assert
            Assert.True(false);
        }

        [Fact(Skip ="Not implemented")]
        public void IsSatisfiedBy_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var facetsConfigSpecification = this.CreateFacetsConfigSpecification();
            List<FacetConfig2> configs = null;

            // Act
            var result = facetsConfigSpecification.IsSatisfiedBy(configs);

            // Assert
            Assert.True(false);
        }
    }
}
