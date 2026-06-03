using System;
using System.Collections.Generic;

namespace SeadQueryCore;

public sealed record FacetTemplateRuntimeSnapshot(
    string TemplateKey,
    string BaseSql,
    string TemplateContract,
    string BaseAnchor,
    IReadOnlyDictionary<string, string> AnchorSqlByTable
)
{
    public static FacetTemplateRuntimeSnapshot Empty { get; } =
        new(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        );
}
