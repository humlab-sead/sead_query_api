namespace SeadQueryCore.Plugin.Discrete;

public class DiscretePickFilterCompiler : IDiscretePickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (targetFacet.FacetCode == currentFacet.FacetCode || !config.HasPicks())
            return "";

        return SqlCompileUtility.InExpr(currentFacet.CategoryIdExpr, config.GetPickValues());
    }
}
