using System;

namespace SeadQueryCore.QueryBuilder;


public class RangesIntersectPickFilterCompiler : IPickFilterCompiler
{

    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (!config.HasPicks())
            return currentFacet.Criteria;

        var picks = config.GetPickValues(true);

        return SqlCompileUtility.RangesIntersectExpr(currentFacet.CategoryIdExpr, picks)
            .GlueIf(currentFacet.Criteria, " AND ");

    }
}
