using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System;
using Xunit;

namespace SQT.Services
{
    [Collection("JsonSeededFacetContext")]
    public class RangeCategoryBoundsServiceTests : DisposableFacetContextContainer
    {
        public RangeCategoryBoundsServiceTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }
    }
}
