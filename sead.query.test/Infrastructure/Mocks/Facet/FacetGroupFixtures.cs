using System;
using System.Collections.Generic;
using System.Text;
using SeadQueryCore;

namespace SeadQueryTest.Fixtures
{
    public static class FacetGroupFixtures
    {
        public static FacetGroup SpaceTime = new FacetGroup {
                FacetGroupId = 2,
                FacetGroupKey = "space_time",
                DisplayTitle = "Space/Time",
                IsApplicable = true,
                IsDefault = false
            };

        public static FacetGroup Measuredvalues = new FacetGroup {
            FacetGroupId = 5,
            FacetGroupKey = "measured_values",
            DisplayTitle = "Measured values",
            IsApplicable = true,
            IsDefault = false
        };
    }
}
