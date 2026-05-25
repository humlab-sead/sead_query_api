# Unit Tests Agent Guide

Use this guide when editing `sead.query.test/UnitTests/`.

## Start Here

- Mirror the production folder or namespace as closely as practical.
- Use the nearest existing test file in the same feature area as the starting point.
- Prefer real value objects and small concrete models over excessive mocking.

## Working Rules

- Keep one behavior per test.
- Use mocks only for true collaborators.
- Do not pull in host, database, file system, or network dependencies here.

## Cheap Validation

- Run the smallest `dotnet test` filter that covers the touched test class or namespace.
