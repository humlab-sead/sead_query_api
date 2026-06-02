using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

public interface IComposedFacetContentFilterQueryFactory
{
    ComposedFilterQuery Create(ComposedFacetContentRequest request);
}
