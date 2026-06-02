---
description: "Use when writing or updating XML summary comments for C# classes, interfaces, and other public extension points in SEAD Query API."
applyTo: "**/*.cs"
---
# C# Summary Comment Guidance

Use this instruction when writing or editing XML `<summary>` comments in C# source files.

## Goal

Write summaries so a junior developer can answer this question quickly:

What does this type do, and what does it produce?

## Core rules

- State the type's main responsibility and the thing it produces first.
- Use plain language and keep the summary easy to scan.
- Keep the summary to one sentence when practical.
- Focus on purpose, boundary, and the primary output or behavior.
- Prefer active verbs such as `Builds`, `Loads`, `Resolves`, `Maps`, `Compiles`, `Selects`, `Validates`, or `Orchestrates`.
- Keep the `<summary>` short; put examples or extra clarification in `<remarks>` when needed.

## What to include

- What the type does
- What it builds, loads, resolves, maps, validates, orchestrates, or returns
- The concrete result, output, or handoff it produces when that matters
- The scope or boundary when that context matters
- The variant or domain type it is responsible for, when relevant

## What to avoid

- Repeating the class name
- Listing every dependency
- Describing every method
- Vague filler such as "handles logic", "service for operations", or "contains methods for"
- Long implementation detail that belongs in remarks or code comments instead

## When to add `<remarks>`

Add a short `<remarks>` block when the summary alone would still leave a junior developer unsure about:

- what the input or output means in practice
- the direction of a mapping or transformation
- a short concrete example that makes the type easier to understand
- an important constraint or boundary that is not obvious from the summary alone

Keep `<remarks>` brief. Prefer one small example or one clarifying note, not a second full summary.

## Type-specific guidance

- For orchestrators and central services: say that they orchestrate shared workflow, delegate specialized work, and produce a specific result type.
- For factories: say what they build and what the returned object represents.
- For handlers or strategies: say which case or domain type they handle and what they load, compute, or return.
- For resolvers or compilers: say what input they turn into what output.
- For mappers: say what source shape they map into what destination shape.

## Good pattern

```csharp
/// <summary>
/// [Verb] [main thing] and produces [result] for [specific scope or boundary].
/// </summary>
```

## Good examples

```csharp
/// <summary>
/// Builds composed facet-content handoffs for supported target facet types.
/// </summary>
```

```csharp
/// <summary>
/// Builds validated composed facet-content requests from facet configuration and route metadata.
/// </summary>
```

```csharp
/// <summary>
/// Loads composed facet content and returns counted items for discrete target facets.
/// </summary>
```

```csharp
/// <summary>
/// Resolves predicate SQL for discrete facet picks and returns routable SQL for the route-compiler path.
/// </summary>
```

```csharp
/// <summary>
/// Resolves SQL that maps discrete facet picks to matching anchor records.
/// </summary>
/// <remarks>
/// Example: if the user picks category IDs 1 and 2, the generated SQL returns the
/// anchor records linked to those selected values.
/// </remarks>
```

## Bad examples

```csharp
/// <summary>
/// This class handles facet content.
/// </summary>
```

```csharp
/// <summary>
/// Service for composed facet content service operations.
/// </summary>
```

```csharp
/// <summary>
/// Contains methods for loading data and doing logic.
/// </summary>
```

## Final check

Before finishing, make sure:

- A new developer can understand the type's purpose in a few seconds.
- The summary says what is special about this type.
- The summary would still make sense even if the type name were hidden.
