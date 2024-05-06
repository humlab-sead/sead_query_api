using SQT.Infrastructure;
using Xunit;

namespace SQT.Plugins.Range
{
    [Collection("SeadJsonFacetContextFixture")]
    public class RangeCategoryBoundsServiceTests : DisposableFacetContextContainer
    {
        public RangeCategoryBoundsServiceTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }
    }
}
