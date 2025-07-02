---
applyTo: '**/*.cs'
---

# Coding Style

## General Guidelines
- Adhere to Microsoft's [coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Follow the official Microsoft .NET C# coding conventions: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Prefer clarity and readability over brevity.
- Use consistent formatting and naming throughout the codebase.

## Naming Conventions
- Follow the official Microsoft [C# identifier naming rules and conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names?source=recommendations)
- Prefix interfaces with `I` (e.g., `IOrderService`).
- Use meaningful, descriptive names; avoid abbreviations.

## Formatting
- Use 4 spaces for indentation (no tabs).
- Use file-scoped namespaces to simplify structure and improve readability.
- Add a blank line between method definitions.
- Place opening braces on a new line for methods, properties, and types (unless using file-scoped namespaces, then follow the file-scoped style).

### Example: File-Scoped Namespaces
```csharp
// Before
namespace MyNamespace
{
    public class ExampleClass
    {
        // ...existing code...
    }
}
// After
namespace MyNamespace;

public class ExampleClass
{
    // ...existing code...
}
```
- All new files must use file-scoped namespaces. Refactor existing files during updates or maintenance.

## Variable Declaration
- Use `var` for local variable declarations when the type is obvious.
- Prefer explicit types if it improves clarity.


## Code Structure
- One type per file (class, interface, enum, etc.).
- Organize files by feature/domain when possible.
- Group using directives at the top of the file, outside the namespace.
- Place related types in the same namespace.
- Use partial classes only when necessary (e.g., for code generation).

## Comments & Documentation
- Use XML documentation comments (`///`) for public APIs.
- Write comments to explain why, not what, when necessary.

## Null Checks & Exceptions
- Use guard clauses for argument validation.
- Use `nameof` for parameter names in exceptions.

## Modern C# Features
- Use pattern matching and expression-bodied members where appropriate.
- Prefer object and collection initializers.
