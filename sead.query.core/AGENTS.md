# Core Agent Guide

Use this guide when working inside `sead.query.core/`.

## Start Here

- Start with the nearest type in `Model/`, `Interfaces/`, `QueryBuilder/`, or `QueryComposer/`.
- Use this project for shared contracts, domain models, and query abstractions.
- Check callers only after identifying the core contract that owns the behavior.

## Boundaries

- Keep this project free of API wiring and repository implementation details.
- Prefer small, explicit contracts over convenience helpers with hidden behavior.
- Avoid introducing infrastructure concerns here.

## Ignore Unless Asked

- `Deprecated/`
- Broad scans across unrelated model areas
