
namespace SeadQueryCore.QueryBuilder;
public class RangeFacetPickFilterCompiler : IPickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (!config.HasPicks())
            return currentFacet.Criteria;

        var picks = config.GetPickValues(true);

        return SqlCompileUtility.BetweenExpr(currentFacet.CategoryIdExpr, picks)
            .GlueIf(currentFacet.Criteria, " AND ");

    }
}
