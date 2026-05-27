using System.Threading.Tasks;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.RouteCompiler;

public interface IFacetPredicateResolver
{
    Task<string> ResolveSqlAsync(FacetConfig2 facetConfig, AnchorTemplate anchorTemplate, string anchorTable, string anchorId);
}
