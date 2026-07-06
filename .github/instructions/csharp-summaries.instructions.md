---
description: "Use when writing or updating XML summary comments for C# classes, interfaces, and other public extension points in SEAD Query API."
applyTo: "**/*.cs"
---
# C# Summary Comment Guidance

A `<summary>` should let a junior developer answer: **what does this type do, and what does it produce?**

## Rules

- State the main responsibility and result first. One sentence when practical.
- Use active verbs: `Builds`, `Loads`, `Resolves`, `Maps`, `Compiles`, `Selects`, `Validates`, `Orchestrates`.
- Include the scope, boundary, or domain variant when it matters.
- Avoid repeating the class name, listing dependencies, describing every method, or vague filler ("handles logic", "service for operations").
- Use `<remarks>` only for a brief clarifying example or constraint the summary alone can't convey.

## Pattern

```csharp
/// <summary>
/// [Verb] [main thing] and produces [result] for [specific scope or boundary].
/// </summary>
```

## Examples

```csharp
// Good:
/// <summary>
/// Builds validated composed facet-content requests from facet configuration and route metadata.
/// </summary>

/// <summary>
/// Resolves SQL that maps discrete facet picks to matching anchor records.
/// </summary>
/// <remarks>
/// Example: picking category IDs 1 and 2 returns anchor records linked to those values.
/// </remarks>

// Bad:
/// <summary>
/// This class handles facet content.
/// </summary>
/// <summary>
/// Service for composed facet content service operations.
/// </summary>
```
