using System;
using System.Collections.Generic;
using static QuerySeadDomain.Utility;
using System.Linq;

namespace QuerySeadDomain.QueryBuilder
{

    public class FacetPickFilterCompiler {

        public FacetPickFilterCompiler()
        {
        }

        public virtual string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config)
        {
            return null;
        }

        protected static Dictionary<EFacetType, FacetPickFilterCompiler> compilers = null;
        public static FacetPickFilterCompiler GetCompiler(EFacetType type)
        {
            if (compilers == null)
                compilers = new Dictionary<EFacetType, FacetPickFilterCompiler>() {
                    { EFacetType.Discrete, new DiscreteFacetPickFilterCompiler() },
                    { EFacetType.Range, new RangeFacetPickFilterCompiler() },
                    { EFacetType.Unknown, new FacetPickFilterCompiler() }
                };
            return compilers.ContainsKey(type) ? compilers[type] : compilers[EFacetType.Unknown];
        }
    }

    class RangeFacetPickFilterCompiler : FacetPickFilterCompiler {

        public override string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config)
        {
            var bound = config.GetPickedLowerUpperBounds();
            int lower = (int)bound[EFacetPickType.lower];
            int upper = (int)bound[EFacetPickType.upper];

            string criteria;
            if (lower == upper) {                                                 // Safer to do it this way if equal
                criteria = $" (floor({currentFacet.CategoryIdExpr}) = {lower})";
            } else {
                criteria = $" ({currentFacet.CategoryIdExpr} >= {lower} and {currentFacet.CategoryIdExpr} <= {upper})";
            }
            criteria += str_prefix(" AND ", currentFacet.QueryCriteria);

            return criteria;
        }
    }

    class DiscreteFacetPickFilterCompiler : FacetPickFilterCompiler {

        public override string Compile(FacetDefinition targetFacet, FacetDefinition currentFacet, FacetConfig2 config)
        {
            if (targetFacet.FacetCode == currentFacet.FacetCode)
                return "";

            List<string> picks = config.GetPickValues().Select(x => $"'{x}'").ToList();

            if (picks.Count == 0)
                return "";

            string criteria = $" ({currentFacet.CategoryIdExpr}::text in (" + String.Join(", ", picks) + ")) ";
            return criteria;
        }
    }
}
