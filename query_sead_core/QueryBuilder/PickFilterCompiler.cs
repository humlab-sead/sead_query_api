using System;
using System.Collections.Generic;
using static QueryFacetDomain.Utility;
using System.Linq;

namespace QueryFacetDomain.QueryBuilder
{

    public class FacetPickFilterCompiler {

        public FacetPickFilterCompiler()
        {

        }

        public virtual string compile(IUnitOfWork context, string targetCode, string currentCode, FacetConfig2 config)
        {
            return null;
        }

        public virtual bool is_affected_position(int targetPosition, int currentPosition)
        {
            return targetPosition > currentPosition;
        }

        protected static Dictionary<EFacetType, FacetPickFilterCompiler> compilers = null;
        public static FacetPickFilterCompiler getCompiler(EFacetType type)
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

        public override string compile(IUnitOfWork context, string targetCode, string currentCode, FacetConfig2 config)
        {
            FacetDefinition facet = context.Facets.GetByCode(currentCode);

            var bound = config.GetPickedLowerUpperBounds();

            int lower = (int)bound[EFacetPickType.lower];
            int upper = (int)bound[EFacetPickType.upper];

            string criteria;
            if (lower == upper) {                                                 // Safer to do it this way if equal
                criteria = $" (floor({facet.CategoryIdExpr}) = {lower})";
            } else {
                criteria = $" ({facet.CategoryIdExpr} >= {lower} and {facet.CategoryIdExpr} <= {upper})";
            }

            criteria += str_prefix(" AND ", facet.QueryCriteria);

            return criteria;
        }

        public override bool is_affected_position(int targetPosition, int currentPosition)
        {
            return base.is_affected_position(targetPosition, currentPosition) || (targetPosition == currentPosition);
        }
    }

    class DiscreteFacetPickFilterCompiler : FacetPickFilterCompiler {

        public override string compile(IUnitOfWork context, string targetCode, string currentCode, FacetConfig2 config)
        {
            if (targetCode == currentCode)
                return "";

            FacetDefinition facet = context.Facets.GetByCode(currentCode);
            List<string> picks = config.GetPickValues().Select(x => $"'{x}'").ToList();

            if (picks.Count == 0)
                return "";

            string criteria = $" ({facet.CategoryIdExpr}::text in (" + String.Join(", ", picks) + ")) ";
            return criteria;
        }
    }

}
