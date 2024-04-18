namespace SeadQueryCore.QueryBuilder;
public class DiscreteFacetPickFilterCompiler : IPickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (targetFacet.FacetCode == currentFacet.FacetCode || !config.HasPicks())
            return "";

        return SqlCompileUtility.InExpr(currentFacet.CategoryIdExpr, config.GetPickValues());
    }
}
