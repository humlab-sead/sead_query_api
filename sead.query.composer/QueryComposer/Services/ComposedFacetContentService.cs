using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;
using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

/// <summary>
/// Orchestrates the composed facet-content path.
/// Unsupported requests fail explicitly.
/// </summary>
public sealed class ComposedFacetContentService : IComposedFacetContentService
{
    private readonly IReadOnlyDictionary<EFacetType, IComposedFacetContentHandler> _handlers;
    private readonly IComposedFacetContentRequestFactory _requestFactory;
    private readonly IComposedFacetContentFilterQueryFactory _filterQueryFactory;

    public ComposedFacetContentService(
        IEnumerable<IComposedFacetContentHandler> handlers,
        IComposedFacetContentRequestFactory requestFactory,
        IComposedFacetContentFilterQueryFactory filterQueryFactory
    )
    {
        ArgumentNullException.ThrowIfNull(handlers);

        _handlers = handlers.ToDictionary(handler => handler.FacetType);
        _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
        _filterQueryFactory = filterQueryFactory ?? throw new ArgumentNullException(nameof(filterQueryFactory));
    }

    public bool CanHandle(FacetsConfig2 facetsConfig)
    {
        return TryGetHandler(facetsConfig, out var handler) && _requestFactory.TryCreate(facetsConfig, handler, out _, out _);
    }

    public FacetContent Load(FacetsConfig2 facetsConfig)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);

        if (!TryGetHandler(facetsConfig, out var handler))
        {
            throw new InvalidOperationException(
                $"The composed facet-content service cannot handle this request: target facet '{facetsConfig.TargetCode ?? facetsConfig.TargetFacet?.FacetCode ?? "(unknown)"}' uses facet type '{facetsConfig.TargetFacet?.FacetTypeId}' which the composed facet-content path does not support. Call CanHandle(...) before invoking Load. Unsupported facet-content requests no longer fall back to the legacy runtime."
            );
        }

        if (!_requestFactory.TryCreate(facetsConfig, handler, out var request, out var failureReason))
        {
            throw new InvalidOperationException(
                $"The composed facet-content service cannot handle this request: {failureReason} Call CanHandle(...) before invoking Load. Unsupported facet-content requests no longer fall back to the legacy runtime."
            );
        }

        var composedFilterQuery = _filterQueryFactory.Create(request);
        return handler.Load(facetsConfig, request, composedFilterQuery);
    }

    private bool TryGetHandler(FacetsConfig2 facetsConfig, out IComposedFacetContentHandler handler)
    {
        handler = null;

        var facetType = facetsConfig?.TargetFacet?.FacetTypeId;
        return facetType is not null && _handlers.TryGetValue(facetType.Value, out handler);
    }
}
