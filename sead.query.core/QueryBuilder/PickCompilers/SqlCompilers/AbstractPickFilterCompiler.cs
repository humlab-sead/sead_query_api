namespace SeadQueryCore.QueryBuilder;


public abstract class AbstractPickFilterCompiler(bool applyFilterWhenNoPicks = false, bool applyFilterOnSelf = false) : IPickFilterCompiler
{
    protected bool ApplyFilterOnSelf { get; set; } = applyFilterOnSelf;
    protected bool ApplyFilterWhenNoPicks { get; set; } = applyFilterWhenNoPicks;

    public virtual string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        if (!ApplyFilterOnSelf && targetFacet.FacetCode == currentFacet.FacetCode)
            return "";

        if (!ApplyFilterWhenNoPicks && !config.HasPicks())
            return "";

        return SqlCompileUtility.InExpr(currentFacet.CategoryIdExpr, config.GetPickValues());
    }

    protected abstract string CompileExpr(Facet targetFacet, Facet currentFacet, FacetConfig2 config);

}
