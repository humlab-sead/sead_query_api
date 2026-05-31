using System.Collections.Generic;

namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Composes multiple anchor-key predicate queries into one filter query.
/// </summary>
public interface IComposedFilterQueryComposer
{
    ComposedFilterQuery Compose(IReadOnlyCollection<PredicateQueryPlan> predicateQueries, string anchorTable, string anchorKeyColumn);
}
