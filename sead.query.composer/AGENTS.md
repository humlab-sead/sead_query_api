# Composer Agent Guide

Use this guide when working inside `sead.query.composer/`.

## Start Here

- Start in `QueryComposer/`; that is the active owning area for composition logic.
- For route and path issues, go directly to `QueryComposer/RouteCompiler/`.
- For SQL-generation behavior, stay close to the compiler or service that emits it.

## Boundaries

- Keep query composition and SQL-generation logic here, not in controllers or repositories.
- Preserve explicit route, graph, and compiler contracts.
- Treat this project as the main home of the query-engine overhaul work.

## Ignore Unless Asked

- `Archived/`
- `bin/` and `obj/`

## Cheap Validation

- Start with composer-focused tests under `sead.query.test/UnitTests/QueryComposer/` or `sead.query.test/QueryComposer/`.
