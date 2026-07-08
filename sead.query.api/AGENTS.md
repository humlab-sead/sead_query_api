# API Agent Guide

Use this guide when working inside `sead.query.api/`.

## Start Here

- Start with `Program.cs`, `Startup.cs`, the matching controller, and the nearest service.
- Use `DTOs/`, `Serializers/`, and `Middleware/` for HTTP boundary concerns.
- Use `Services/` for API orchestration and request-to-domain translation.

## Boundaries

- Keep controllers and DTOs thin.
- Do not move query composition, SQL, or repository behavior into the API project.
- Push domain logic into `sead.query.core` or `sead.query.composer` and data access into `sead.query.infra`.

## Cheap Validation

- Prefer targeted controller or host-backed tests in `sead.query.test/IntegrationTests/`.
- If the change is only wiring, validate the narrowest affected integration slice.
- For `--validate-facet-config` workflows in bootstrap/CI contexts, choose semantic validation when PostgreSQL is reachable (`make validate-facet-config`, optional `FACET_CONFIG_DB_HOST=...`) and offline validation when connectivity is intentionally unavailable (`make validate-facet-config-offline`).

## Ignore Unless Asked

- `bin/` and `obj/`
- Runtime config outside the touched behavior
