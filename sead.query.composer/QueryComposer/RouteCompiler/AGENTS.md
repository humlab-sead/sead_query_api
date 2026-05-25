# RouteCompiler Agent Guide

Use this guide when editing `sead.query.composer/QueryComposer/RouteCompiler/`.

## Start Here

- `ArrowRouteParser.cs`: arrow syntax, macro expansion, and route token parsing.
- `RouteGraph.cs`: graph construction and route lookup.
- `RouteResolver.cs`: route selection logic.
- `RouteSqlCompiler.cs`: SQL generation for resolved routes.

## Working Rules

- Keep route parsing, graph logic, route resolution, and route SQL concerns separated.
- Fix behavior in the owning class instead of adding compensating logic in callers.
- Prefer narrow tests that exercise the touched route behavior directly.

## Cheap Validation

- Start with `sead.query.test/UnitTests/QueryComposer/RouteCompiler/`.
- Widen to `sead.query.test/QueryComposer/` only if the change crosses from unit behavior into composed-query behavior.
