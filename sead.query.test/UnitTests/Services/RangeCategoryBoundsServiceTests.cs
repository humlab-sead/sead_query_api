using SQT.Infrastructure;
using Xunit;

namespace SQT.Services
{
    [Collection("SeadJsonFacetContextFixture")]
    public class RangeCategoryBoundsServiceTests : DisposableFacetContextContainer
    {
        public RangeCategoryBoundsServiceTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }
    }
}
