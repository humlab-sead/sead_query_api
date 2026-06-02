using System.Collections.Generic;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Services;

public sealed record ComposedFacetContentRequest(
    string AnchorTable,
    string AnchorKeyColumnName,
    string TargetJoinColumn,
    string AnchorToTargetSql,
    IReadOnlyList<FacetConfig2> PredicateConfigs
);
