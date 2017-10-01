using System;
using System.Collections.Generic;
using static QuerySeadDomain.Utility;
using System.Linq;

namespace QuerySeadDomain.QueryBuilder
{
    public interface IFacetPickFilterCompiler
    {
        string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config);
    }

    public class RangeFacetPickFilterCompiler : IFacetPickFilterCompiler {

        public string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config)
        {
            var picks = config.GetPickValues(true);
            return picks.Count != 2 ? "" 
                : UtilitySqlCompiler.BetweenExpr(currentFacet.CategoryIdExpr, picks[0], picks[1])
                    .AddIf(" AND ", currentFacet.QueryCriteria);
        }

        private string CompileSql(string expr, decimal lower, decimal upper)
        {
            return (lower == upper) ? $" (floor({expr}) = {lower})" : $" ({expr} >= {lower} and {expr} <= {upper})";
        }

    }

    public class DiscreteFacetPickFilterCompiler : IFacetPickFilterCompiler {

        public string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config)
        {
            if (targetFacet.FacetCode == currentFacet.FacetCode || !config.HasPicks())
                return "";

            string criteria = UtilitySqlCompiler.InExpr(currentFacet.CategoryIdExpr, config.GetPickValues());
            return criteria;
        }
    }

    public class GeoFacetPickFilterCompiler : IFacetPickFilterCompiler
    {
        public string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config)
        {
            throw new NotImplementedException();
        }
    }

    public class UndefinedFacetPickFilterCompiler : IFacetPickFilterCompiler
    {
        public string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config)
        {
            throw new ArgumentException();
        }
    }
}
