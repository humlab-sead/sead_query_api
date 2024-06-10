using System;
using System.Collections.Generic;

namespace SeadQueryCore.Plugin.Intersect;

public class IntersectPickFilterCompiler : IIntersectPickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (!config.HasPicks())
            return currentFacet.Criteria;

        if (config.GetPickCount() < 2)
            throw new ArgumentException("Invalid bounds size");

        return CompileExpr(currentFacet, config.GetPickValues(true))
            .GlueIf(currentFacet.Criteria, " AND ");
    }

    public string CompileExpr(Facet facet, List<decimal> bounds)
    {
        return SqlCompileUtility.RangeExpr(facet.CategoryIdExpr, facet.CategoryIdType, facet.CategoryIdOperator, bounds);
    }
}

