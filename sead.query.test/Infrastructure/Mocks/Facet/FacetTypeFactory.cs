using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryTest.Mocks
{
    public static class FacetTypeFactory
    {
        public static FacetType Fake(
            EFacetType facetTypeId = EFacetType.Discrete
        ) => new FacetType
        {
            FacetTypeId = facetTypeId,
            FacetTypeName = Guid.NewGuid().ToString()
        };
    }
}
