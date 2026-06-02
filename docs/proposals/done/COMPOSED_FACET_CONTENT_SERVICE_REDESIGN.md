The `ComposedFacetContentService` is a good candidate for a **target facet handler / strategy** abstraction. Right now `ComposedFacetContentService.Load(...)` knows too much about the differences between `Discrete`, `Range`, `Intersect`, and `GeoPolygon`: it branches for geo-polygon, interval facets, target-only discrete facets, category info services, row mapping, and distribution construction. The conditionals are concentrated around `FacetTypeId` checks and service selection. 

A good direction is to move facet-type-specific behavior into plugins like:

```csharp
public interface IComposedFacetContentHandler
{
    EFacetType FacetType { get; }

    bool CanHandle(FacetsConfig2 facetsConfig);

    string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn);

    FacetContent Load(
        FacetsConfig2 facetsConfig,
        ComposedFacetContentRequest request,
        ComposedFilterQuery composedFilterQuery
    );
}
```

Then your main service becomes more orchestration-oriented:

```csharp
public sealed class ComposedFacetContentService : IComposedFacetContentService
{
    private readonly IReadOnlyDictionary<EFacetType, IComposedFacetContentHandler> _handlers;
    private readonly IComposedRequestFactory _requestFactory;
    private readonly IComposedFilterQueryFactory _filterQueryFactory;

    public ComposedFacetContentService(
        IEnumerable<IComposedFacetContentHandler> handlers,
        IComposedRequestFactory requestFactory,
        IComposedFilterQueryFactory filterQueryFactory)
    {
        _handlers = handlers.ToDictionary(handler => handler.FacetType);
        _requestFactory = requestFactory;
        _filterQueryFactory = filterQueryFactory;
    }

    public bool CanHandle(FacetsConfig2 facetsConfig)
    {
        return TryGetHandler(facetsConfig, out var handler)
            && handler.CanHandle(facetsConfig)
            && _requestFactory.CanCreate(facetsConfig, handler);
    }

    public FacetContent Load(FacetsConfig2 facetsConfig)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);

        if (!TryGetHandler(facetsConfig, out var handler))
        {
            throw new InvalidOperationException(
                $"Facet type '{facetsConfig.TargetFacet?.FacetTypeId}' is not supported by the composed facet-content path.");
        }

        var request = _requestFactory.Create(facetsConfig, handler);
        var composedFilterQuery = _filterQueryFactory.Create(request);

        return handler.Load(facetsConfig, request, composedFilterQuery);
    }

    private bool TryGetHandler(
        FacetsConfig2 facetsConfig,
        out IComposedFacetContentHandler handler)
    {
        handler = null;

        var facetType = facetsConfig?.TargetFacet?.FacetTypeId;
        return facetType is not null && _handlers.TryGetValue(facetType.Value, out handler);
    }
}
```

The main win is that `Load` no longer says:

```csharp
if GeoPolygon ...
else if Range or Intersect ...
else Discrete ...
```

Instead, each facet type owns its own behavior.

For example, your current geo-polygon block could become:

```csharp
public sealed class GeoPolygonComposedFacetContentHandler : IComposedFacetContentHandler
{
    private readonly ITypedQueryProxy _queryProxy;
    private readonly IFacetContentQueryComposer _facetContentQueryComposer;
    private readonly IGeoPolygonCategoryInfoService _categoryInfoService;

    public EFacetType FacetType => EFacetType.GeoPolygon;

    public GeoPolygonComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        IGeoPolygonCategoryInfoService categoryInfoService)
    {
        _queryProxy = queryProxy;
        _facetContentQueryComposer = facetContentQueryComposer;
        _categoryInfoService = categoryInfoService;
    }

    public bool CanHandle(FacetsConfig2 facetsConfig)
    {
        // This preserves your current rule:
        // GeoPolygon target facets do not support additional predicate facets.
        var affectedConfigs = facetsConfig.GetConfigsThatAffectsTarget(
            facetsConfig.TargetCode,
            facetsConfig.GetFacetCodes());

        var predicateConfigs = affectedConfigs
            .Where(config => !string.Equals(
                config.FacetCode,
                facetsConfig.TargetCode,
                StringComparison.OrdinalIgnoreCase))
            .ToList();

        return predicateConfigs.Count == 0;
    }

    public string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        if (ComposedFacetHelpers.TryResolveSimpleColumnOnTable(
            targetFacet.CategoryIdExpr,
            targetFacet.TargetTable,
            out var targetJoinColumn))
        {
            return targetJoinColumn;
        }

        var primaryKey = targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
        return ComposedFacetHelpers.IsPlaceholderPrimaryKey(primaryKey)
            ? string.Empty
            : primaryKey;
    }

    public FacetContent Load(
        FacetsConfig2 facetsConfig,
        ComposedFacetContentRequest request,
        ComposedFilterQuery composedFilterQuery)
    {
        var categoryInfo = _categoryInfoService.GetCategoryInfo(
            facetsConfig,
            facetsConfig.TargetCode);

        var contentQueryPlan = _facetContentQueryComposer.Compose(
            facetsConfig,
            composedFilterQuery,
            request.TargetJoinColumn,
            request.AnchorToTargetSql,
            categoryInfo.Query);

        var categoryItems = _queryProxy.QueryRows(
            contentQueryPlan.Sql,
            ToGeoPolygonCategoryItem);

        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

        return new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items = categoryItems.Where(item => item.Count != null).ToList(),
            Distribution = categoryItems.ToDictionary(item => item.Category ?? "(null)"),
            IntervalInfo = categoryInfo,
            SqlQuery = contentQueryPlan.Sql,
            Picks = userPicks ?? [],
        };
    }

    private static CategoryItem ToGeoPolygonCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.Category2String(0),
            Count = reader.GetInt32(1),
            Extent = [reader.GetDecimal(2), reader.GetDecimal(3)],
            Name = reader.Category2String(0),
        };
    }
}
```

Your range and intersect handlers can probably share a base class because the current code treats them almost identically: both are “interval” facets, both use an `ICategoryInfoService`, both query counted categories, then query the outer category list and merge counts back into it. That behavior currently lives behind `IsIntervalFacetType(...)` and `GetIntervalCategoryInfoService(...)`.  

For example:

```csharp
public abstract class IntervalComposedFacetContentHandler : IComposedFacetContentHandler
{
    private readonly ITypedQueryProxy _queryProxy;
    private readonly IFacetContentQueryComposer _facetContentQueryComposer;
    private readonly ICategoryInfoService _categoryInfoService;

    public abstract EFacetType FacetType { get; }

    protected IntervalComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        ICategoryInfoService categoryInfoService)
    {
        _queryProxy = queryProxy;
        _facetContentQueryComposer = facetContentQueryComposer;
        _categoryInfoService = categoryInfoService;
    }

    public virtual bool CanHandle(FacetsConfig2 facetsConfig)
    {
        return true;
    }

    public virtual string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        var targetPrimaryKey = targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;

        return ComposedFacetHelpers.IsPlaceholderPrimaryKey(targetPrimaryKey)
            ? anchorKeyColumn
            : targetPrimaryKey;
    }

    public FacetContent Load(
        FacetsConfig2 facetsConfig,
        ComposedFacetContentRequest request,
        ComposedFilterQuery composedFilterQuery)
    {
        var categoryInfo = _categoryInfoService.GetCategoryInfo(
            facetsConfig,
            facetsConfig.TargetCode);

        var contentQueryPlan = _facetContentQueryComposer.Compose(
            facetsConfig,
            composedFilterQuery,
            request.TargetJoinColumn,
            request.AnchorToTargetSql,
            categoryInfo.Query);

        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

        var categoryCounts = _queryProxy
            .QueryRows(contentQueryPlan.Sql, ToRangeCategoryItem)
            .ToDictionary(item => item.Category ?? "(null)");

        var outerCategoryCounts = _queryProxy
            .QueryRows(categoryInfo.Query, _categoryInfoService.SqlCompiler.ToItem)
            .ToList();

        foreach (var item in outerCategoryCounts)
        {
            if (categoryCounts.TryGetValue(item.Category ?? "(null)", out var countedItem))
            {
                item.Count = countedItem.Count;
            }
        }

        return new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items = outerCategoryCounts.Where(item => item.Count != null).ToList(),
            Distribution = categoryCounts,
            IntervalInfo = categoryInfo,
            SqlQuery = contentQueryPlan.Sql,
            Picks = userPicks ?? [],
        };
    }

    private static CategoryItem ToRangeCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.IsDBNull(0) ? "(null)" : reader.GetString(0),
            Count = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
            Extent =
            [
                reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
            ],
            Name = reader.IsDBNull(0) ? "(null)" : reader.GetString(0),
        };
    }
}
```

Then:

```csharp
public sealed class RangeComposedFacetContentHandler
    : IntervalComposedFacetContentHandler
{
    public override EFacetType FacetType => EFacetType.Range;

    public RangeComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        IRangeCategoryInfoService categoryInfoService)
        : base(queryProxy, facetContentQueryComposer, categoryInfoService)
    {
    }
}

public sealed class IntersectComposedFacetContentHandler
    : IntervalComposedFacetContentHandler
{
    public override EFacetType FacetType => EFacetType.Intersect;

    public IntersectComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        IIntersectCategoryInfoService categoryInfoService)
        : base(queryProxy, facetContentQueryComposer, categoryInfoService)
    {
    }
}
```

The discrete handler would contain the current default branch plus the special “target-only discrete” behavior. That behavior is currently hidden as a special case inside `Load`: when there are no predicate configs and `AnchorToTargetSql` is present, it calls `CreateTargetOnlyDiscreteFacetContent(...)`.  

```csharp
public sealed class DiscreteComposedFacetContentHandler : IComposedFacetContentHandler
{
    private readonly ITypedQueryProxy _queryProxy;
    private readonly IFacetContentQueryComposer _facetContentQueryComposer;
    private readonly IDiscreteCategoryInfoService _categoryInfoService;

    public EFacetType FacetType => EFacetType.Discrete;

    public DiscreteComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        IDiscreteCategoryInfoService categoryInfoService)
    {
        _queryProxy = queryProxy;
        _facetContentQueryComposer = facetContentQueryComposer;
        _categoryInfoService = categoryInfoService;
    }

    public bool CanHandle(FacetsConfig2 facetsConfig)
    {
        return true;
    }

    public string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        if (ComposedFacetHelpers.TryResolveSimpleColumnOnTable(
            targetFacet.CategoryIdExpr,
            targetFacet.TargetTable,
            out var targetJoinColumn))
        {
            return targetJoinColumn;
        }

        var primaryKey = targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
        return ComposedFacetHelpers.IsPlaceholderPrimaryKey(primaryKey)
            ? string.Empty
            : primaryKey;
    }

    public FacetContent Load(
        FacetsConfig2 facetsConfig,
        ComposedFacetContentRequest request,
        ComposedFilterQuery composedFilterQuery)
    {
        var contentQueryPlan = _facetContentQueryComposer.Compose(
            facetsConfig,
            composedFilterQuery,
            request.TargetJoinColumn,
            request.AnchorToTargetSql,
            categoryInfoQuery: null);

        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

        var categoryItems = _queryProxy.QueryRows(
            contentQueryPlan.Sql,
            ToCategoryItem);

        if (request.PredicateConfigs.Count == 0 &&
            !string.IsNullOrWhiteSpace(request.AnchorToTargetSql))
        {
            return CreateTargetOnlyFacetContent(
                facetsConfig,
                contentQueryPlan.Sql,
                userPicks,
                categoryItems);
        }

        return new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items = categoryItems.Where(item => item.Count != null).ToList(),
            Distribution = categoryItems.ToDictionary(item => item.Category ?? "(null)"),
            IntervalInfo = new FacetContent.CategoryInfo
            {
                Count = categoryItems.Count,
                Query = contentQueryPlan.Sql
            },
            SqlQuery = contentQueryPlan.Sql,
            Picks = userPicks ?? [],
        };
    }

    private FacetContent CreateTargetOnlyFacetContent(
        FacetsConfig2 facetsConfig,
        string sqlQuery,
        Dictionary<string, FacetsConfig2.UserPickData> userPicks,
        List<CategoryItem> categoryItems)
    {
        var categoryInfo = _categoryInfoService.GetCategoryInfo(
            facetsConfig,
            facetsConfig.TargetCode);

        var categoryCounts = categoryItems.ToDictionary(item => item.Category ?? "(null)");

        var outerCategoryCounts = _queryProxy
            .QueryRows(categoryInfo.Query, _categoryInfoService.SqlCompiler.ToItem)
            .ToList();

        foreach (var item in outerCategoryCounts)
        {
            if (categoryCounts.TryGetValue(item.Category ?? "(null)", out var countedItem))
            {
                item.Count = countedItem.Count;
            }
        }

        return new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items = outerCategoryCounts.Where(item => item.Count != null).ToList(),
            Distribution = categoryCounts,
            IntervalInfo = categoryInfo,
            SqlQuery = sqlQuery,
            Picks = userPicks ?? [],
        };
    }

    private static CategoryItem ToCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.Category2String(0),
            Count = reader.GetInt32(1),
            Extent = [reader.GetInt32(1)],
            Name = reader.Category2String(0),
        };
    }
}
```

I would also extract `TryCreateRequest(...)` into a separate factory, because it currently mixes generic request validation with facet-type-specific rules. For example, this check is really a handler concern:

```csharp
if (targetFacet.FacetTypeId == EFacetType.GeoPolygon && predicateConfigs.Count > 0)
{
    ...
}
```

That rule belongs in `GeoPolygonComposedFacetContentHandler.CanHandle(...)`, not in the central service. 

A possible request factory shape:

```csharp
public interface IComposedFacetContentRequestFactory
{
    bool TryCreate(
        FacetsConfig2 facetsConfig,
        IComposedFacetContentHandler handler,
        out ComposedFacetContentRequest request,
        out string failureReason);

    ComposedFacetContentRequest Create(
        FacetsConfig2 facetsConfig,
        IComposedFacetContentHandler handler);
}
```

Inside the factory, replace this method:

```csharp
private string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
{
    if (targetFacet?.FacetTypeId is EFacetType.Range or EFacetType.Intersect)
    {
        ...
    }

    ...
}
```

with:

```csharp
var targetJoinColumn = handler.ResolveTargetJoinColumn(targetFacet, anchorKeyColumn);
```

That removes the interval-specific condition from the central request-building path. The original method currently special-cases `Range` and `Intersect`, then falls back to discrete-like behavior for other facets. 

For dependency injection, register the handlers as a collection:

```csharp
services.AddScoped<IComposedFacetContentHandler, DiscreteComposedFacetContentHandler>();
services.AddScoped<IComposedFacetContentHandler, RangeComposedFacetContentHandler>();
services.AddScoped<IComposedFacetContentHandler, IntersectComposedFacetContentHandler>();
services.AddScoped<IComposedFacetContentHandler, GeoPolygonComposedFacetContentHandler>();

services.AddScoped<IComposedFacetContentRequestFactory, ComposedFacetContentRequestFactory>();
services.AddScoped<IComposedFilterQueryFactory, ComposedFilterQueryFactory>();
```

The resulting architecture would look like this:

```text
ComposedFacetContentService
  ├─ selects handler by EFacetType
  ├─ asks request factory to create route/request
  ├─ asks filter query factory to create composed filter query
  └─ delegates content loading to handler

IComposedFacetContentHandler
  ├─ DiscreteComposedFacetContentHandler
  ├─ RangeComposedFacetContentHandler
  ├─ IntersectComposedFacetContentHandler
  └─ GeoPolygonComposedFacetContentHandler
```

I would not try to remove every conditional. Some of the existing checks are domain validation and should remain somewhere. The goal is to move conditionals from the **central workflow** into the **facet-type plugin** that owns the rule.

The most useful first refactor would be:

1. Introduce `IComposedFacetContentHandler`.
2. Move the `Load(...)` branches into `Discrete`, `Interval`, and `GeoPolygon` handlers.
3. Inject `IEnumerable<IComposedFacetContentHandler>` into `ComposedFacetContentService`.
4. Later extract `TryCreateRequest(...)` into a request factory.
5. Then move `ResolveTargetJoinColumn(...)` and target-specific `CanHandle(...)` rules into the handlers.

That lets you add a new facet type later by adding one new handler class, instead of editing the central `ComposedFacetContentService` every time.
