# QueryComposer Agent Guide

Use this guide when editing `sead.query.composer/QueryComposer/`.

## Start Here

- Pick the nearest owning folder first: `RouteCompiler/`, `Services/`, or a local compiler.
- For route text, graph traversal, or route SQL, stay inside `RouteCompiler/` until proven otherwise.
- Read the matching test area before broadening the search.

## Working Rules

- Keep composition logic explicit and local to the owning class.
- Avoid spreading the same rule across compiler, service, and caller layers.
- Prefer fixing the route or compiler abstraction rather than patching API callers.

## Cheap Validation

- Run the narrowest composer test slice that matches the touched class or folder.
