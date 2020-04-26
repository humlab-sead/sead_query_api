using SeadQueryCore;
using System;

namespace SQT.Mocks
{
    public static class FacetGroupFactory
    {
        public static FacetGroup Fake(
            int facetGroupId = 999
        ) => new FacetGroup
        {
            FacetGroupId = facetGroupId,
            Description = Guid.NewGuid().ToString(),
            DisplayTitle = Guid.NewGuid().ToString(),
            FacetGroupKey = Guid.NewGuid().ToString()
        };
    }
}
