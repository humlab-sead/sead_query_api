using System;
using System.Collections.Generic;
using System.Text;
using SeadQueryCore;

namespace SeadQueryTest.Infrastructure.Scaffolds
{
    public static class FacetTypeInstances
    {
        public static FacetType Discrete = new FacetType {
            FacetTypeId = SeadQueryCore.EFacetType.Discrete,
            FacetTypeName = "discrete",
            ReloadAsTarget = false
        };

        public static FacetType Range = new FacetType {
            FacetTypeId = SeadQueryCore.EFacetType.Range,
            FacetTypeName = "range",
            ReloadAsTarget = true
        };
    }
}
