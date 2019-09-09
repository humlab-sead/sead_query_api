using System;
using System.Collections.Generic;
using static SeadQueryCore.Utility;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public interface IFacetPickFilterCompiler
    {
        string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config);
    }

    public class RangeFacetPickFilterCompiler : IFacetPickFilterCompiler {
        public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
        {
            var picks = config.GetPickValues(true);
            return picks.Count != 2 ? ""
                : UtilitySqlCompiler.BetweenExpr(currentFacet.CategoryIdExpr, picks[0], picks[1])
                    .GlueIf(currentFacet.QueryCriteria, " AND ");
        }

        private string CompileSql(string expr, decimal lower, decimal upper)
        {
            return (lower == upper) ? $" (floor({expr}) = {lower})" : $" ({expr} >= {lower} and {expr} <= {upper})";
        }
    }

    public class DiscreteFacetPickFilterCompiler : IFacetPickFilterCompiler {
        public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
        {
            if (targetFacet.FacetCode == currentFacet.FacetCode || !config.HasPicks())
                return "";

            string criteria = UtilitySqlCompiler.InExpr(currentFacet.CategoryIdExpr, config.GetPickValues());
            return criteria;
        }
    }

    public class GeoFacetPickFilterCompiler : IFacetPickFilterCompiler
    {
        public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
        {
            throw new NotImplementedException();
        }
    }

    public class UndefinedFacetPickFilterCompiler : IFacetPickFilterCompiler
    {
        public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
        {
            throw new ArgumentException();
        }
    }
}
