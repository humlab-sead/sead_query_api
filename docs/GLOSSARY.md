# SEAD Query API Architectural Glossary

This glossary is intended for junior developers working with the SEAD Query API and the query-engine overhaul.

## Active Route-Compiler Path

The current query-composer runtime path that generates SQL using route definitions, anchor templates, route discovery, and route compilation services. It is the modern replacement for older approaches that relied heavily on manually written join templates.

## Anchor

The common entity that all active facet filters must resolve to before they can be combined.

Think of an anchor as the "thing we are actually filtering". Examples might be sites, samples, or taxa.

## Anchor-Based Composition

A query-composition approach where every facet filter produces anchor keys. These anchor-key sets are then combined to determine the final filtered result.

## Anchor Key

The identifier column for the anchor entity.

For example, if the anchor is a site, the anchor key might be `site_id`. All composed predicates eventually resolve to anchor keys so they can be combined consistently.

## API Layer

The part of the application responsible for receiving HTTP requests and returning HTTP responses.

The API layer should not contain query-composition logic.

## Build (Query-Composer Context)

In query-composer documentation, "build" means "generate SQL for the next stage of processing."

It does **not** mean:
- execute the SQL
- query the database
- return an API response

## Category

A value displayed in a facet.

Examples:
- Sweden
- Mesolithic
- Pinus

## Category Count

The number of matching records associated with a category.

Example:

| Category | Count |
|----------|-------|
| Sweden   | 234   |
| Norway   | 51    |

## Component Boundary

A clearly defined separation of responsibility between parts of the system.

Examples:
- API Layer
- Core Layer
- Infrastructure Layer
- Composer Layer

## Composition

The process of combining multiple filters into one query result.

## Composition Contract

A documented agreement about how components interact.

Examples:
- what inputs are required
- what outputs are produced
- what validation rules apply

## Composer

The subsystem responsible for assembling queries from facet configurations, routes, predicates, and anchor definitions.

## Contract

A formal rule describing how a component can be used.

Good contracts make systems easier to understand, test, and maintain.

## CTE (Common Table Expression)

A named intermediate SQL query created using `WITH`.

CTEs make large queries easier to read and debug.

## Discrete Facet

A pick-based facet where users select one or more predefined values.

Examples:
- Country
- Species
- Material

On the composed path, selected values are translated into predicate SQL that eventually resolves matching anchor keys.

## Domain Model

The collection of business concepts represented in code.

Examples:
- Facet
- Anchor
- Route
- Facet Type

## Facet

A configurable filter shown in the user interface.

Examples:
- Country
- Time Period
- Species

## Facet Content

The information returned to populate a facet.

Examples:
- categories
- counts
- ranges

## Facet Type

A category of facet behavior.

Examples:
- Discrete
- Range
- Intersect
- Geo Polygon

## Filter

A condition that restricts the result set.

Example:

```sql
country = 'Sweden'
```

## Handoff

The transfer of responsibility, data, or results from one component to another.

Example:
- filter composition produces a filtered anchor set
- result generation consumes that anchor set

## Infrastructure Layer

The layer responsible for persistence and external resources.

Examples:
- repositories
- database access
- configuration loading

## Intersect Composition

Combining multiple anchor sets by keeping only anchors that appear in all sets.

## Join

A database operation that connects related tables.

## Legacy Path

The older implementation that remains authoritative until replacement functionality is fully validated.

## Orchestrator

A component whose main job is coordinating other components rather than performing all work itself.

## Predicate

A condition used to filter data.

Examples:

```sql
country = 'Sweden'
age > 1000
```

## Predicate Facet

A facet configuration that actively contributes filtering logic to a composed query.

In the current implementation this is a normal `FacetConfig2` that participates in filtering.

## Predicate Plan

An intermediate representation of filtering logic before final SQL generation.

Think of it as a blueprint for a query.

## Predicate SQL

A SQL subquery that represents one facet filter.

The composed query engine combines predicate SQL from multiple facets to produce the final anchor set.

## Projection

The process of transforming filtered data into the final result shape.

Examples:
- facet counts
- result rows
- summaries

## Query Compilation

The process of converting configuration and query models into executable SQL.

## Query Composer

The subsystem responsible for assembling composed queries from routes, predicates, anchors, and facet definitions.

## Range Facet

A facet that filters values within an interval.

Examples:
- Date Range
- Elevation Range
- Sample Depth

## Resolver

A component that converts one representation into another.

Example:
A facet resolver converts facet configuration into a predicate plan.

## Result Projection

The final stage that converts filtered anchors into rows, counts, summaries, or aggregations.

## Route

A defined traversal path through related database tables.

Routes describe how one entity can be reached from another.

## Route Graph

A representation of the available table relationships and routes in the schema.

## Route Resolver

A component responsible for selecting or resolving the route used during query composition.

## Secondary Predicate Facet

A predicate facet that is used specifically to filter the currently requested target facet.

The current composed path requires these facets to be routable and capable of exposing a simple source key.

## Source Id

The normalized alias representing the facet-side value or key produced by a predicate query.

It identifies the source-side value before the query is reduced to matching anchors.

## SQL Compiler

A component that converts query models into SQL statements.

## Strategy Pattern

A design pattern where different implementations handle different behaviors behind a common interface.

Examples:
- Discrete handlers
- Range handlers
- Geo handlers

## Target Facet

The facet currently being populated.

If the UI is loading category counts for Country, then Country is the target facet.

## Target Id

The normalized alias representing the anchor-side identifier emitted by a predicate query.

This value later participates in anchor-set composition.

## Validation

The process of verifying that inputs satisfy required rules before processing continues.

Examples:
- valid routes
- matching anchor types
- required operators

## Vertical Slice

A complete end-to-end implementation of a specific piece of functionality.

A vertical slice includes all layers needed to make that feature work.
