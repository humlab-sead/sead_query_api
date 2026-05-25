# RouteCompiler Unit Tests Agent Guide

Use this guide when editing `sead.query.test/UnitTests/QueryComposer/RouteCompiler/`.

## Start Here

- Keep test files aligned with `ArrowRouteParser`, `RouteGraph`, `RouteResolver`, or `RouteSqlCompiler`.
- Add coverage next to the class under test instead of creating mixed route-compiler test files.
- Read the production class and the neighboring test file before adding new scaffolding.

## Working Rules

- Assert route behavior, graph behavior, and SQL behavior separately.
- Prefer deterministic route inputs over generated data.
- Keep tests fast enough to remain the first validation step after edits in `RouteCompiler/`.
