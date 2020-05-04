using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore
{
    public static class FacetConfigFactory
    {
        public static FacetConfig2 Create(Facet facet, int position)
        {
            var config = new FacetConfig2
            {
                Facet = facet,
                Picks = new List<FacetConfigPick>(),
                FacetCode = facet.FacetCode,
                Position = position
            };
            return config;
        }
    }
}
