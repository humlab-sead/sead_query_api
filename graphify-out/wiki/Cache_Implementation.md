# Cache Implementation

> 32 nodes · cohesion 0.08

## Key Concepts

- **Interface.cs** (6 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Interface.cs`
- **GeoPolygonFacetPlugin** (6 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- **.RegisterPlugin()** (5 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- **ContainerBuilder** (4 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- **.Compile()** (4 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- **.Compile()** (4 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryInfoSqlCompiler.cs`
- **GeoPolygonPickFilterCompiler** (4 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/PickFilterCompiler.cs`
- **.Compile()** (4 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/PickFilterCompiler.cs`
- **.GetPolygonValues()** (4 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/PickFilterCompiler.cs`
- **IGeoPolygonCategoryCountSqlCompiler** (3 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Interface.cs`
- **IGeoPolygonCategoryInfoSqlCompiler** (3 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Interface.cs`
- **IGeoPolygonPickFilterCompiler** (3 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Interface.cs`
- **.Register()** (3 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- **.RegisterComposerRuntime()** (3 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- **.RegisterSharedPlugin()** (3 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- **.ToItem()** (3 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- **IGeoPolygonCategoryInfoService** (2 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Interface.cs`
- **FacetConfig2** (2 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/PickFilterCompiler.cs`
- **Plugin.cs** (1 connections) — `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- **CategoryItem** (1 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- **CompilePayload** (1 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- **Facet** (1 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- **IDataReader** (1 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- **QuerySetup** (1 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- **CategoryCountSqlCompiler.cs** (1 connections) — `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- *... and 7 more nodes in this community*

## Relationships

- [[Query Setup Extensions]] (1 shared connections)
- [[Facet API Controllers]] (1 shared connections)
- [[Facet URL Configuration]] (1 shared connections)
- [[Facet Pick Filter Compilation]] (1 shared connections)

## Source Files

- `sead.query.core/Plugins/GeoPolygonFacet/Interface.cs`
- `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`
- `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryCountSqlCompiler.cs`
- `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/CategoryInfoSqlCompiler.cs`
- `sead.query.core/Plugins/GeoPolygonFacet/SqlCompilers/PickFilterCompiler.cs`

## Audit Trail

- EXTRACTED: 80 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [[index]] to navigate.*