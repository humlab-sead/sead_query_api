using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SQT.Model
{
    public class FacetConfigPickTests
    {
        [Fact]
        public void ToDecimal_WhenDecimalValue_ConvertedValue()
        {
            // Arrange
            const decimal pickValue = 19.1M;
            var facetConfigPick = new FacetConfigPick(EPickType.discrete, pickValue);

            // Act
            var result = facetConfigPick.ToDecimal();

            // Assert
            Assert.Equal(pickValue, result);
        }

        [Fact]
        public void ToInt_DiscreteIntegerStringPick_ConvertedToCorrespondingInt()
        {
            // Arrange
            const string pickValue = "19";
            var facetConfigPick = new FacetConfigPick(EPickType.discrete, pickValue);
            // Act
            var result = facetConfigPick.ToInt();

            // Assert
            Assert.Equal(19, result);
        }

        [Fact]
        public void CreateLowerUpper_LowerUpper_ListOfTwoPicks()
        {
            // Arrange
            const decimal lower = 5;
            const decimal upper = 10;

            // Act
            var result = FacetConfigPick.CreateLowerUpper(lower, upper);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(EPickType.lower, result[0].PickType);
            Assert.Equal(lower.ToString(), result[0].PickValue);
            Assert.Equal(EPickType.upper, result[1].PickType);
            Assert.Equal(upper.ToString(), result[1].PickValue);
        }
    }
}
