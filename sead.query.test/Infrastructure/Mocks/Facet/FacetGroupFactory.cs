using SeadQueryCore;
using System;

namespace SeadQueryTest.Mocks
{
    public static class FacetGroupFactory
    {
        public static FacetGroup Fake(
            int facetGroupId = 999
        ) => new FacetGroup
        {
            //FacetGroupId = 1,
            Description = Guid.NewGuid().ToString(),
            DisplayTitle = Guid.NewGuid().ToString(),
            FacetGroupKey = Guid.NewGuid().ToString()
        };
    }
}
