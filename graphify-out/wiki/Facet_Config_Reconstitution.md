# Facet Config Reconstitution

> 38 nodes · cohesion 0.08

## Key Concepts

- **ITypedQueryProxy** (6 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **.QueryKeyValues2()** (5 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **.QueryRows()** (5 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **.QueryKeyValues2()** (5 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **.QueryRows()** (5 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **IDataReader** (5 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **T** (5 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **.QueryRow()** (4 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **.GetRangeAsync()** (4 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **.QueryRow()** (4 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **IDataReader** (4 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **T** (4 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **IDynamicQueryProxy** (3 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **.GetRange()** (3 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **.QueryScalars()** (3 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **.GetRange()** (3 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **.QueryScalars()** (3 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **List** (3 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **List** (3 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **Facet.cs** (2 connections) — `sead.query.core/Model/Entities/Facet.cs`
- **Facet** (2 connections) — `sead.query.core/Model/Entities/Facet.cs`
- **.GetResolvedTableNames()** (2 connections) — `sead.query.core/Model/Entities/Facet.cs`
- **.Query()** (2 connections) — `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- **DatabaseQueryProxy.cs** (2 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- **.Query()** (2 connections) — `sead.query.infra/Repository/DatabaseQueryProxy.cs`
- *... and 13 more nodes in this community*

## Relationships

- No strong cross-community connections detected

## Source Files

- `sead.query.core/Interfaces/IDatabaseQueryProxy.cs`
- `sead.query.core/Model/Entities/Facet.cs`
- `sead.query.infra/Repository/DatabaseQueryProxy.cs`

## Audit Trail

- EXTRACTED: 104 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [[index]] to navigate.*