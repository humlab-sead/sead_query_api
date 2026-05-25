# System Requirements Specification: SEAD Query System Overhaul

**Document Version:** 1.0  
**Date:** August 27, 2025  
**Project:** SEAD Query API Modernization  
**Branch:** query-engine-overhaul  

---

## Key Concepts

### Anchor

The common entity type that serves as the the central subject of all queries and as the universal link when composing query. All facets must resolve to a set of anchor keys. It is defined by its primary key, the **Anchor Key** (e.g., `sample_id`). All filtering operations ultimately resolve to a list of these keys, and facets in a query chain must hence return the same type of anchor key.

### Query Types

| Query                       | Description  |
|---------------------------- | ------------- |
| Facet Predicate Query (FPQ) | The compiled SQL query for filtering the facet source based on user's chosen source keys. Returns a set of source keys. This is the fundamental building block that encapsulates facet-specific filtering logic. |
| Anchor Predicate Query (FPQ) | The compiled SQL query for filtering the facet source based on user's chosen source keys. Returns a set of anchor keys. This is the fundamental building block that encapsulates facet-specific filtering logic. |
| Composed Filter Query (cFPQ) | The compiled SQL query combining multiple FPQs using INTERSECT operations. Returns a set of anchor keys matching ALL active facets. This represents the final filtering stage producing result set identifiers. |
| Facet Content Query (FCQ)   | The compiled SQL query returning aggregated view of data grouped by facet categories. Used to populate facet UI elements with counts and category information, filtered by current query context. |
| Decoded Facet Content Query (dFCQ)   | Variant of FCQ with human-readable categories |


### Facet

A facet represents a logical, configurable filter (predicate) that targets a specific domain entity, and answers the question "What anchor keys are connected to my target entities"?
It encapsulates all the information needed to both display a filter control to a user and apply that filter to a database query.

A facet can be seen as a parameterized query that given an anchor and user input returns related entities as a set of (target key, anchor key) pairs. The logic for deciding if a target and an anchor is related is fully encapsulated within the facet.

Facet Properties:

| Property           | Description  |
|--------------------|------------- |
| `FacetId`            | Internal integer system id (primary key) |
| `FacetCode`          | A human-readable representation of the `FacetId` (e.g., "taxa", "site_region") |
| `DisplayTitle` | The human-readable name shown in the UI (e.g., "Taxonomic Groups"). |
| `Description` | A short text describing the facet. |
| `FacetGroup` | Logical grouping of facets (e.g., a "Dataset" group), used in the UI layer. |
| `FacetType` | Defines how the facet's behaviour. This is a crucial property. See types below. |
| `CategoryIdExpr`     |  The column or expression in the facet's source table that contains the filterable values. For discrete facets, typically the primary key. For range facets, a numeric column. For intersect facets, a range data type. |
| `CategoryNameExpr`   | The column or expression providing human-readable labels for CategoryIdExpr values. Used in FCQ queries when displaying category names to users. |
| `CategoryIdType`     | The PostgreSQL data type of the CategoryIdExpr column (e.g., integer, numeric, int4range). Determines SQL generation strategy and client-side data handling. |
| `CategoryIdOperator` | The SQL operator used when filtering on CategoryIdExpr values. Typically 'IN' for discrete facets, '&&' for range intersections, 'BETWEEN' for range filtering. |
| `FacetTemplate` | **New** The SQL template for the FCQ |
| `AnchorRoutes` | **New** A dictionary specifying routes to the anchors the facet supports |

### Facet Type

Defines a facet's behaviour.

| Facet Type           | Description  |
| -------------------- | ------------- |
| Discrete Facet       | Facet with a finite set of distinct categories (e.g., countries, taxa, materials). Users select specific values from a list. Filtering uses IN clauses. |
| Range Facet          | Facet with continuous numeric values (e.g., age, depth, temperature). Users specify min/max ranges. Content queries group values into bins for histogram display. |
| Intersect Facet      | Facet with stored range values (e.g., date ranges, spatial extents). Users specify a query range, system finds stored ranges that intersect. Uses PostgreSQL range data types. |
| GeoPolygon Facet     | Spatial facet supporting geographic filtering (e.g., sites within polygon, samples within region). Uses PostGIS spatial operations and geometry data types. |

### Facet Source
This is the entiry that a facet's filtering logic operates on. This corresponds to a table, view or UDF in the dtabase containing the categories/values that define a facet (e.g., `tbl_sites` for Site facet, `tbl_taxa` for Taxa facet). This may be different from the anchor entity. For example, a "Taxon" facet targets the `tbl_taxa` table but could resolves to dataset anchor keys.

### Route System
Architecture component that defines paths between facet source entities and anchor tables through database relationships. Eliminates need for explicit auto-generating joins using e.g. Diskstras Algorithm.

### Strategy Pattern
Design pattern implementation where each facet type (discrete, range, GIS) has its own resolver class containing type-specific logic for SQL generation and data processing.

### CTE (Common Table Expression)
SQL WITH clause construct used to create named temporary result sets. In the new architecture, each facet generates a CTE containing its (source_id, anchor_id) pairs.

### INTERSECT Operation
SQL set operation that returns only rows present in ALL input queries. Used to combine multiple facet CTEs to find anchor keys matching all active filters.

### Facet Sequence (Chain)
A facet chain is a logical composed predicate where each facet answers "What anchor keys are connected to this entity?" The resulting predicate answers "What anchor keys exist in ALL facets in this sequence?"

### Facet Configuration (FacetConfig)
Client request data containing selected facet values, target facet for content, and query context. Drives the SQL generation process.

### Anchor Route Templates
Configuration defining how a facet connects to different anchor entities through routes or explicit SQL. Part of the route system for reducing template verbosity.

### Binning Strategy
Algorithm for grouping continuous numeric values into discrete intervals for range facet content display. Includes fixed count, fixed size, and percentile-based approaches.

## Core Architectural Principles

### The CTE + INTERSECT Model
The architecture transforms each facet into a self-contained Common Table Expression (CTE) that returns anchor IDs. Final filtering is achieved by INTERSECTing or INNER JOINing these CTEs.

### Key Architectural Components
- **Facet Predicate Resolvers**: Strategy pattern implementations for each facet type
- **Route System**: Reusable join path definitions to eliminate template explosion
- **Query Composer**: Orchestrates CTE assembly and final query generation
- **Anchor-Based Design**: All facets resolve to common anchor entities

### Anchor Entity Foundation
**Requirement ARCH-001**: The system SHALL be built around a configurable set of predefined "Anchor Entities" (e.g., Sample, Site, Analysis).

**Rationale**: Anchor entities serve as the "common language" for query composition, ensuring all facets can be meaningfully combined.

**Specification**:
- Each Anchor Entity MUST be uniquely identifiable by a single key column
- Anchor entities SHALL be configurable via database configuration
- ~~The system SHALL support a minimum of 8 concurrent anchor types~~

### The Facet Predicate Contract
**Requirement ARCH-002**: Every facet predicate MUST return exactly one column containing a set of distinct Anchor Keys.

**Rationale**: This contract ensures predictable composition and prevents data duplication in joins.

**Specification**:
- Facet predicate queries (FPQs) MUST use `SELECT DISTINCT anchor_id` or equivalent
- Facet queries SHALL NOT return duplicate anchor IDs
- ~~System SHALL validate facet contract compliance during development~~

### Chain Consistency (The Golden Rule)
**Requirement ARCH-003**: Within a single composed query, all participating facets MUST resolve to the same Anchor Entity.

**Rationale**: This constraint makes query composition simple, predictable, and performant.

**Specification**:
- The system SHALL reject attempts to mix facets with different anchor types
- Error messages SHALL clearly indicate anchor type mismatches
- UI SHALL only display facets compatible with the current anchor type

### CTE-Based Composition
**Requirement ARCH-004**: Each active facet SHALL be compiled into FPQ that can be used as a Common Table Expression (CTE).

**Rationale**: Use of CTEs provide clear, debuggable SQL structure and optimal PostgreSQL performance.

**Specification**:
- Final filtering achieved by `INNER JOIN` or `INTERSECT` of CTEs
- Each CTE SHALL follow naming convention: `facet_{facetCode}`
- ~~CTEs SHALL be ordered consistently for predictable query plans~~

### Facet Isolation and Encapsulation
**Requirement ARCH-005**: Each facet SHALL operate in complete isolation with all facet-specific logic encapsulated within the facet implementation.

**Rationale**: Encapsulation ensures maintainable, testable code where facets can be developed, tested, and debugged independently without affecting other system components.

**Specification**:
- Facets MUST NOT have dependencies on other facets or their internal logic
- All facet-specific SQL generation, data transformation, and business rules SHALL be contained within the facet's resolver implementation
- Facets SHALL interact with the system only through well-defined interfaces (`IFacetPredicateResolver`)
- Facet implementations MUST be independently testable without requiring other facets or complex system setup
- Changes to one facet's implementation SHALL NOT affect the behavior of any other facet

---

## Facet Definition and Behavior

### Multi-Anchor Support
**Requirement FACET-001**: A single facet definition SHOULD support multiple Anchor Entities to promote reusability.

**Implementation**: Store `PredicateTemplates` as JSON map: `{"sample": "...", "site": "..."}`

### Target vs. Anchor Entity Separation
**Requirement FACET-002**: Facets SHALL distinguish between Target Entity (filter source) and Anchor Entity (result key).

**Example**: A "Taxon" facet targets `tbl_taxa` for filtering but resolves to `sample_id` anchor keys.

### Facet Type Support
**Requirement FACET-003**: The system SHALL support the following facet types:

#### Discrete Facets
- **CategoryIdExpr**: ID column (e.g., `country_id`, `site_id`)
- **User Input**: List of specific IDs `[1, 5, 12]`
- **FPQ**: `WHERE country_id IN (1, 5, 12)`
- **FCQ**: Groups by exact ID values, returns counts per ID

#### Range Facets
- **CategoryIdExpr**: Numeric column (e.g., `age_value`, `depth_cm`)
- **User Input**: Min/max range `{min: 100, max: 500}`
- **FPQ**: `WHERE age_value BETWEEN 100 AND 500`
- **FCQ**: Groups numeric values into computed intervals, returns counts per interval

#### Intersect Facets
- **CategoryIdExpr**: Range column (e.g., `date_range` of type `int4range`)
- **User Input**: Single range `{min: 1200, max: 1400}`
- **FPQ**: `WHERE date_range && int4range(1200, 1400)`
- **FCQ**: Groups by stored range values, returns counts per range

#### GeoPolygon Facets
- **CategoryIdExpr**: Geometry column (e.g., `site_location`)
- **User Input**: Polygon geometry
- **FPQ**: `WHERE ST_Within(site_location, @polygon)`
- **FCQ**: Groups by geometric regions, returns counts per region

### Single Operator Constraint
**Requirement FACET-004**: Each facet SHALL use exactly one predefined filtering operator.

**Rationale**: Simplifies facet definition and UI presentation.

### Inactive Facet Handling
**Requirement FACET-005**: Facets with no user-provided arguments SHALL be ignored in query composition.

---

## Query Composition Model

### Mathematical Model

Let:
- $A$ = Anchor Entity (e.g., Sample, Site, Analysis)
- $Q^{A^{id}}_{f_i}$ = Facet Predicate Query (FPQ) for facet $f_i$ returning anchor IDs
- $Q^{A^{id}}_{composed}$ = Composed query for facet sequence $[f_1, f_2, ..., f_n]$

The composed query is defined as:

```sql
WITH 
  facet_f1 (anchor_id) AS (Q^{A^{id}}_{f_1}),
  facet_f2 (anchor_id) AS (Q^{A^{id}}_{f_2}),
  ...
  facet_fn (anchor_id) AS (Q^{A^{id}}_{f_n})
SELECT anchor_id 
FROM facet_f1
INTERSECT SELECT anchor_id FROM facet_f2
INTERSECT SELECT anchor_id FROM facet_f3
...
INTERSECT SELECT anchor_id FROM facet_fn;
```

### Route System
**Requirement COMP-001**: The system SHALL implement a route-based join system to eliminate template explosion.

**Implementation**:
- Define reusable route fragments between common table pairs
- Store routes as named, parameterized SQL snippets
- Facet templates reference routes instead of containing explicit join SQL
- Target: Reduce 400+ templates to <50 routes + facet-specific logic

---

## Facet Content and UI Population

### Aggregated Counts
**Requirement UI-001**: Each facet SHALL calculate aggregated counts filtered by all OTHER active facets.

**Specification**:
- Count entity: `COUNT(DISTINCT anchor_id)`
- Filtering: Apply all facets EXCEPT the current one
- Performance: Optimize for sub-second response times

### Range Facet Binning
**Requirement UI-002**: Range facets SHALL group values into computed intervals for histogram display.

**Implementation Options**:
- Fixed number of bins (e.g., 10 intervals)
- Fixed step size (e.g., every 100 units)
- Logarithmic scale for skewed distributions
- PostgreSQL `width_bucket()` function for efficient binning

### Content Query Variants
**Requirement UI-003**: Support multiple FCQ variants for performance optimization:

- **FCQ_BASIC**: Returns only IDs for composition (fast)
- **FCQ_EXTENDED**: Returns IDs + human-readable names (slower, for final display)

---

## Result Set Generation

### Decoupled Architecture
**Requirement RESULT-001**: Result set generation SHALL be completely decoupled from filtering logic.

**Process**:
1. Execute CFQ to get filtered anchor IDs
2. Pass anchor IDs to separate result set query
3. Join anchor IDs with target tables for final output

### Flexible Output Formats
**Requirement RESULT-002**: The system SHALL support multiple result set formats:

- **Entity Lists**: Full target entity data with properties
- **Geographic Data**: Coordinate data for mapping
- **Summary Statistics**: Aggregated analytical data
- **Export Formats**: CSV, JSON, GeoJSON output
- **Paginated Results**: Large result set handling

### User-Selectable Formats
**Requirement RESULT-003**: UI SHALL allow users to select desired result set format.

---

**Document Control**
- **Author**: SEAD Development Team
- **Reviewers**: [To be assigned]
- **Approval**: [To be assigned]
- **Next Review**: [To be scheduled]

*This document defines the system requirements for the SEAD Query System overhaul. For implementation details, see the [Implementation and Migration Plan](./implementation_and_migration_plan.md).*

