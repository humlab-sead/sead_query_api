using Moq;
using SeadQueryCore;
using SQT.Fixtures;
using System;

namespace SQT.Mocks
{
    internal static class FacetFactory
    {
        public static Facet ByStore(string facetCode)
        {
            return FacetFixtures.Store[facetCode];
        }

        public static Facet Fake(
            string facetCode,
            FacetType t,
            FacetGroup g,
            bool is_applicable = true
        ) =>
            new Facet
            {
                FacetCode = facetCode,
                CategoryIdExpr = Guid.NewGuid().ToString(),
                CategoryNameExpr = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayTitle = Guid.NewGuid().ToString(),
                AggregateType = "",
                AggregateTitle = "",
                AggregateFacetId = 0,
                SortExpr = "",
                FacetGroup = g,
                FacetType = t,
                IsApplicable = is_applicable
            };

    }
}
