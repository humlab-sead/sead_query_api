# Dependency Injection and Logging

> 32 nodes · cohesion 0.06

## Key Concepts

- **.Compile()** (5 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- **.Compile()** (5 connections) — `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/MapResultSqlCompiler.cs`
- **.Compile()** (5 connections) — `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/TabularResultSqlCompiler.cs`
- **.GetSqlCompiler()** (3 connections) — `sead.query.core/Model/Extensions/ResultViewTypeExt.cs`
- **IResultSqlCompiler** (3 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- **ResultViewType.cs** (2 connections) — `sead.query.core/Model/Entities/ResultViewType.cs`
- **IResultSqlCompiler** (2 connections)
- **.Locate()** (2 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompilerLocator.cs`
- **.Locate()** (2 connections) — `sead.query.core/QueryBuilder/ResultCompilers/ResultSqlCompilerLocator.cs`
- **TabularResultSqlCompiler.cs** (2 connections) — `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/TabularResultSqlCompiler.cs`
- **SeadQueryCore** (1 connections) — `sead.query.core/Model/Entities/ResultViewType.cs`
- **SeadQueryCore** (1 connections) — `sead.query.core/Model/Extensions/ResultViewTypeExt.cs`
- **SeadQueryCore** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- **SeadQueryCore** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompilerLocator.cs`
- **SeadQueryCore** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/ResultSqlCompilerLocator.cs`
- **ResultViewType** (1 connections) — `sead.query.core/Model/Extensions/ResultViewTypeExt.cs`
- **Facet** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- **IEnumerable** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- **QuerySetup** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- **ResultSpecificationField** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- **IResultSqlCompiler** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompilerLocator.cs`
- **IResultSqlCompiler** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/ResultSqlCompilerLocator.cs`
- **Facet** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/MapResultSqlCompiler.cs`
- **IEnumerable** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/MapResultSqlCompiler.cs`
- **QuerySetup** (1 connections) — `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/MapResultSqlCompiler.cs`
- *... and 7 more nodes in this community*

## Relationships

- No strong cross-community connections detected

## Source Files

- `sead.query.core/Model/Entities/ResultViewType.cs`
- `sead.query.core/Model/Extensions/ResultViewTypeExt.cs`
- `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompiler.cs`
- `sead.query.core/QueryBuilder/ResultCompilers/IResultSqlCompilerLocator.cs`
- `sead.query.core/QueryBuilder/ResultCompilers/ResultSqlCompilerLocator.cs`
- `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/MapResultSqlCompiler.cs`
- `sead.query.core/QueryBuilder/ResultCompilers/SqlCompilers/TabularResultSqlCompiler.cs`

## Audit Trail

- EXTRACTED: 53 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [[index]] to navigate.*