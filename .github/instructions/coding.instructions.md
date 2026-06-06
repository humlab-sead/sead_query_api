---
applyTo: '**/*.cs'
---

# C# and .NET Coding Guidance

Use this instruction when editing C# source files in this repository.

## Core Principles

- Use C# 13 and .NET 10 conventions.
- Prefer small, explicit, composable code over framework-heavy or overly abstract designs.
- Preserve the existing style of the file unless there is a clear reason to normalize it as part of the change.
- Fix root causes rather than layering workarounds on top of unclear behavior.
- Keep changes minimal and focused.

## Naming and Structure

- Use descriptive names that reflect domain meaning.
- Use `PascalCase` for types, methods, properties, and constants.
- Use `camelCase` for parameters, locals, and private fields.
- Prefix interfaces with `I`.
- Prefer one primary type per file.
- Keep namespaces aligned with folder structure when practical.

## Formatting and File Layout

- Use 4 spaces for indentation and do not use tabs.
- Keep using directives at the top of the file.
- Prefer file-scoped namespaces for new files.
- When editing an existing file, preserve the current namespace style unless the task is explicitly a style cleanup.
- Insert blank lines between logical blocks so methods remain easy to scan.

## Language Features

- Use `var` when the type is obvious from the right-hand side; use explicit types when it improves readability.
- Use object, collection, and target-typed `new` initializers when they make the code shorter without hiding intent.
- Use pattern matching, switch expressions, and expression-bodied members when they improve clarity.
- Do not rewrite stable code only to use newer syntax.

## Nullability and Validation

- Validate public method arguments with guard clauses.
- Prefer `ArgumentNullException.ThrowIfNull(...)` for null checks.
- Use `nameof(...)` for exception parameter names.
- Return empty collections instead of `null` unless `null` has a real semantic meaning.
- Follow the file or project nullable context already in place; do not introduce broad nullability changes unless required by the task.

## Exceptions and Error Handling

- Throw the most specific exception type that fits the contract.
- Use exceptions for exceptional paths, not normal branching.
- Include enough context in exception messages to make failures diagnosable.
- Avoid swallowing exceptions; either handle them meaningfully or let them propagate.

## APIs and Dependencies

- Keep constructors explicit about their dependencies.
- Prefer dependency injection and interfaces at architectural boundaries.
- Avoid service locator patterns and hidden global state.
- Keep domain logic out of controllers, startup wiring, and repository plumbing.

## Async and Collections

- Suffix asynchronous methods with `Async`.
- Pass `CancellationToken` through public async APIs when cancellation is relevant.
- Avoid blocking async code with `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`.
- Prefer `IReadOnlyList<T>`, `IReadOnlyCollection<T>`, or `IEnumerable<T>` for read-only contracts when mutation is not required.

## Comments and Documentation

- Add XML documentation for public APIs, extension points, and non-obvious behavior.
- Write comments to explain intent, invariants, or tradeoffs.
- Do not add comments that restate the code line by line.

## Change Discipline

- Keep edits focused on the task.
- Avoid drive-by renames, formatting-only churn, or unrelated refactors.
- If a file mixes old and new style, improve only the touched area unless a broader cleanup is part of the task.
