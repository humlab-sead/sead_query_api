# Facet Pick Filter Compilation

> 47 nodes · cohesion 0.05

## Key Concepts

- **IPickFilterCompiler** (6 connections)
- **.Compile()** (6 connections) — `sead.query.core/QueryBuilder/PickCompilers/PicksFilterCompiler.cs`
- **.Compile()** (5 connections) — `sead.query.core/QueryBuilder/PickCompilers/IPicksFilterCompiler.cs`
- **.PickCompiler()** (4 connections) — `sead.query.core/QueryBuilder/PickCompilers/PicksFilterCompiler.cs`
- **IntersectPickFilterCompiler** (4 connections) — `sead.query.core/Plugins/IntersectFacet/SqlCompilers/PickFilterCompiler.cs`
- **.Compile()** (4 connections) — `sead.query.core/Plugins/IntersectFacet/SqlCompilers/PickFilterCompiler.cs`
- **.CompileExpr()** (4 connections) — `sead.query.core/Plugins/IntersectFacet/SqlCompilers/PickFilterCompiler.cs`
- **IDiscretePickFilterCompiler** (3 connections) — `sead.query.core/Plugins/DiscreteFacet/Interface.cs`
- **IIntersectPickFilterCompiler** (3 connections) — `sead.query.core/Plugins/IntersectFacet/Interface.cs`
- **.Compile()** (3 connections) — `sead.query.core/QueryBuilder/PickCompilers/AbstractPickFilterCompiler.cs`
- **.CompileExpr()** (3 connections) — `sead.query.core/QueryBuilder/PickCompilers/AbstractPickFilterCompiler.cs`
- **UndefinedPickFilterCompiler** (3 connections) — `sead.query.core/QueryBuilder/PickCompilers/UndefinedFacetPickFilterCompiler.cs`
- **.Compile()** (3 connections) — `sead.query.core/QueryBuilder/PickCompilers/UndefinedFacetPickFilterCompiler.cs`
- **IRangePickFilterCompiler** (3 connections) — `sead.query.core/Plugins/RangeFacet/Interface.cs`
- **DiscretePickFilterCompiler** (3 connections) — `sead.query.core/Plugins/DiscreteFacet/SqlCompilers/PickFilterCompiler.cs`
- **.Compile()** (3 connections) — `sead.query.core/Plugins/DiscreteFacet/SqlCompilers/PickFilterCompiler.cs`
- **RangePickFilterCompiler** (3 connections) — `sead.query.core/Plugins/RangeFacet/SqlCompilers/PickFilterCompiler.cs`
- **.Compile()** (3 connections) — `sead.query.core/Plugins/RangeFacet/SqlCompilers/PickFilterCompiler.cs`
- **PicksFilterCompiler.cs** (2 connections) — `sead.query.core/QueryBuilder/PickCompilers/PicksFilterCompiler.cs`
- **Facet** (2 connections) — `sead.query.core/Plugins/IntersectFacet/SqlCompilers/PickFilterCompiler.cs`
- **Facet** (2 connections) — `sead.query.core/QueryBuilder/PickCompilers/AbstractPickFilterCompiler.cs`
- **FacetConfig2** (2 connections) — `sead.query.core/QueryBuilder/PickCompilers/AbstractPickFilterCompiler.cs`
- **FacetConfig2** (2 connections) — `sead.query.core/QueryBuilder/PickCompilers/PicksFilterCompiler.cs`
- **IPicksFilterCompiler** (1 connections)
- **AbstractPickFilterCompiler.cs** (1 connections) — `sead.query.core/QueryBuilder/PickCompilers/AbstractPickFilterCompiler.cs`
- *... and 22 more nodes in this community*

## Relationships

- [[Facet Aggregate Schema]] (1 shared connections)
- [[Cache Implementation]] (1 shared connections)
- [[Facet URL Configuration]] (1 shared connections)

## Source Files

- `sead.query.core/Plugins/DiscreteFacet/Interface.cs`
- `sead.query.core/Plugins/DiscreteFacet/SqlCompilers/PickFilterCompiler.cs`
- `sead.query.core/Plugins/IntersectFacet/Interface.cs`
- `sead.query.core/Plugins/IntersectFacet/SqlCompilers/PickFilterCompiler.cs`
- `sead.query.core/Plugins/RangeFacet/Interface.cs`
- `sead.query.core/Plugins/RangeFacet/SqlCompilers/PickFilterCompiler.cs`
- `sead.query.core/QueryBuilder/PickCompilers/AbstractPickFilterCompiler.cs`
- `sead.query.core/QueryBuilder/PickCompilers/IPickFilterCompiler.cs`
- `sead.query.core/QueryBuilder/PickCompilers/IPicksFilterCompiler.cs`
- `sead.query.core/QueryBuilder/PickCompilers/PicksFilterCompiler.cs`
- `sead.query.core/QueryBuilder/PickCompilers/UndefinedFacetPickFilterCompiler.cs`

## Audit Trail

- EXTRACTED: 100 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [[index]] to navigate.*