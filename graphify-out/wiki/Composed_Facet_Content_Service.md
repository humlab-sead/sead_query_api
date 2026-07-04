# Composed Facet Content Service

> 9 nodes · cohesion 0.39

## Key Concepts

- **FacetTableRepository** (8 connections) — `sead.query.infra/Repository/FacetRepository.cs`
- **FacetTable** (5 connections) — `sead.query.infra/Repository/FacetRepository.cs`
- **.FindThoseWithAlias()** (4 connections) — `sead.query.infra/Repository/FacetRepository.cs`
- **.FindThoseWithAlias()** (3 connections) — `sead.query.core/Interfaces/IFacetRepository.cs`
- **.GetByAlias()** (3 connections) — `sead.query.infra/Repository/FacetRepository.cs`
- **FacetTable** (3 connections) — `sead.query.core/Interfaces/IFacetRepository.cs`
- **.GetByAlias()** (2 connections) — `sead.query.core/Interfaces/IFacetRepository.cs`
- **List** (2 connections) — `sead.query.infra/Repository/FacetRepository.cs`
- **List** (1 connections) — `sead.query.core/Interfaces/IFacetRepository.cs`

## Relationships

- [[Repository Interface Methods]] (4 shared connections)
- [[Arrow Route Parsing]] (2 shared connections)

## Source Files

- `sead.query.core/Interfaces/IFacetRepository.cs`
- `sead.query.infra/Repository/FacetRepository.cs`

## Audit Trail

- EXTRACTED: 31 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [[index]] to navigate.*