---
description: Encourage extensible C# design by replacing enum/type/category-based conditionals with polymorphic strategies, handlers, plugins, or registries where appropriate.
applyTo: "**/*.cs"
---


# Object-oriented design and extensibility instructions

When generating, refactoring, or reviewing C# code, avoid type/category-based conditionals where behavior varies by domain type.

Do not add new `if`, `switch`, or pattern-matching branches that select behavior based on enum values, string type codes, category names, discriminator fields, or values such as `FacetTypeId`, unless the conditional is genuinely simpler and unlikely to grow.

Prefer polymorphism over type-code branching.

Use object-oriented extension points such as:

* strategy classes
* handlers
* plugins
* resolvers
* factories
* registries
* dependency-injected collections
* small interfaces with focused responsibilities

Design the code so that adding a new domain type usually requires adding a new implementation class and registering it, not editing an existing central service.

Central services should orchestrate shared workflow only. They may validate common preconditions, select the appropriate strategy, compose shared state, and delegate the variant-specific work.

Move type-specific behavior into the corresponding strategy, handler, or plugin. This includes:

* capability checks
* validation rules specific to one type
* query composition differences
* row mapping differences
* category-info lookup differences
* join-column resolution differences
* output-shaping differences

Keep shared behavior in common services, helper classes, or abstract base classes. Use a base class only when it removes real duplication without hiding important behavior.

When refactoring existing conditional code:

1. Identify each branch that varies by type or category.
2. Define an interface that represents the behavior needed by the caller.
3. Create one implementation per type, or one implementation per type family when behavior is genuinely shared.
4. Inject `IEnumerable<TStrategy>` or an explicit registry into the orchestrating service.
5. Select the strategy once near the boundary.
6. Delegate behavior to the selected strategy.
7. Preserve existing behavior and error messages unless there is a clear reason to improve them.
8. Keep the refactor incremental and testable.

Avoid replacing a large `switch` with an equally opaque dictionary of lambdas. Prefer named classes with clear responsibilities.

Do not over-engineer very small or stable conditionals. A conditional is acceptable when it represents simple control flow, a guard clause, or a rule that is not expected to vary by domain type.

For code involving facets, routes, query composition, or content loading, avoid branching on `FacetTypeId` in the central service. Introduce a handler interface such as `IComposedFacetContentHandler`, with implementations such as `DiscreteComposedFacetContentHandler`, `RangeComposedFacetContentHandler`, `IntersectComposedFacetContentHandler`, or `GeoPolygonComposedFacetContentHandler`.

The main service should depend on abstractions and delegate facet-specific behavior to the selected handler.
