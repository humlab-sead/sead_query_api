# Test Project Agent Guide

Use this guide when working inside `sead.query.test/`.

## Start Here

- Put isolated behavior in `UnitTests/`.
- Put host, HTTP, DI, or persistence boundary checks in `IntegrationTests/`.
- Use `QueryComposer/` for composer-focused scenarios that do not fit the generic integration folders.

## Working Rules

- Start from the nearest existing test file before creating a new fixture or helper.
- Reuse local test builders and host fixtures when they keep the test readable.
- Keep validation as narrow as possible for the touched production slice.

## Ignore Unless Asked

- `TestResults/`
- `bin/`, `obj/`, and `tmp/`
- `Purgatory/` unless the task explicitly targets it
