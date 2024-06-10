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
            const decimal pickValue = 19.1M;
            var facetConfigPick = new FacetConfigPick(pickValue);

            var result = facetConfigPick.ToDecimal();

            Assert.Equal(pickValue, result);
        }

        [Fact]
        public void ToInt_DiscreteIntegerStringPick_ConvertedToCorrespondingInt()
        {
            const string pickValue = "19";
            var facetConfigPick = new FacetConfigPick(pickValue);
            var result = facetConfigPick.ToInt();

            Assert.Equal(19, result);
        }

    }
}
