# Architecture Diagrams

This document provides visual diagrams for the SEAD Query API.

Use `docs/DESIGN.md` as the primary written architecture reference. These diagrams summarize the current runtime, the core request workflows, and the planned query-engine overhaul.

## Diagram Status

- Current runtime diagrams reflect the existing faceted query API flow.
- Overhaul diagrams describe the planned route-based, anchor-centered query model on `query-engine-overhaul`.
- Detailed deployment and operations diagrams are `TBD`.

## System Overview

```mermaid
flowchart LR
    Client[Client UI]
    Api[sead.query.api]
    Core[sead.query.core]
    Infra[sead.query.infra]
    Composer[sead.query.composer\nIn progress]
    Db[(SEAD PostgreSQL)]
    Tests[sead.query.test]

    Client -->|Facet and result requests| Api
    Api -->|Uses domain and query contracts| Core
    Api -->|Loads repositories and services| Infra
    Infra -->|Reads config and data| Db
    Api -. planned query engine .-> Composer
    Composer -. route and predicate composition .-> Core
    Composer -. planned SQL compilation .-> Db
    Tests -. validates runtime and redesign .-> Api
    Tests -. validates compilers and services .-> Core
    Tests -. validates repositories and fixtures .-> Infra
    Tests -. validates overhaul work .-> Composer

    classDef edge fill:#edf3ff,stroke:#6c8ebf,color:#1f2d3d;
    classDef runtime fill:#e8f5ec,stroke:#5b8f6a,color:#1f3527;
    classDef planned fill:#fff4df,stroke:#c28b2c,color:#4a3620;
    classDef store fill:#f3f3f3,stroke:#8a8a8a,color:#333333;
    classDef test fill:#f7e8f0,stroke:#b56a8a,color:#472734;

    class Client edge;
    class Api,Core,Infra runtime;
    class Composer planned;
    class Db store;
    class Tests test;
```

## Current Runtime Responsibilities

```mermaid
flowchart LR
    Request[Facet request]
    Reconstitute[Reconstitute config]
    Clean[Remove bogus picks]
    Resolve[Resolve target facet]
    Select[Select facet content service]
    Compile[Compile interval or category SQL]
    Count[Load category counts]
    Assemble[Assemble facet content]
    Response[Facet response]
    Db[(SEAD PostgreSQL)]

    Request --> Reconstitute --> Clean --> Resolve --> Select --> Compile
    Compile --> Db
    Db --> Count --> Assemble --> Response

    classDef input fill:#edf3ff,stroke:#6c8ebf,color:#1f2d3d;
    classDef process fill:#e8f5ec,stroke:#5b8f6a,color:#1f3527;
    classDef store fill:#f3f3f3,stroke:#8a8a8a,color:#333333;
    classDef output fill:#fff4df,stroke:#c28b2c,color:#4a3620;

    class Request input;
    class Reconstitute,Clean,Resolve,Select,Compile,Count,Assemble process;
    class Db store;
    class Response output;
```

## Current Facet Content Sequence

```mermaid
sequenceDiagram
    autonumber
    actor UI as Client UI
    participant API as API Controller
    participant RC as ReconstituteConfigService
    participant FL as FacetLoadService
    participant BP as BogusPickService
    participant FS as FacetContentService
    participant QC as SQL Compiler
    participant DB as PostgreSQL

    UI->>API: Populate facet request
    API->>RC: Reconstitute request payload
    RC-->>API: FacetsConfig
    API->>FL: Load target facet content
    FL->>BP: Remove invalid picks
    BP-->>FL: Normalized FacetsConfig
    FL->>FS: Resolve service by facet type
    FS->>QC: Compile interval or category SQL
    QC->>DB: Execute facet SQL
    DB-->>QC: Category rows
    FS->>DB: Load category counts and outer counts
    DB-->>FS: Aggregated counts
    FS-->>FL: FacetContent
    FL-->>API: FacetContent
    API-->>UI: Facet response
```

## Current Result Generation Sequence

```mermaid
sequenceDiagram
    autonumber
    actor UI as Client UI
    participant API as API Controller
    participant RC as ReconstituteConfigService
    participant RS as Result Service
    participant QB as Query Builder
    participant DB as PostgreSQL

    UI->>API: Result request with facet state
    API->>RC: Reconstitute facet and result config
    RC-->>API: FacetsConfig and ResultConfig
    API->>RS: Load result set
    RS->>QB: Compile filter query
    QB->>DB: Execute filtered query
    DB-->>QB: Filtered anchor or result rows
    RS->>QB: Compile result projection
    QB->>DB: Execute result query
    DB-->>QB: Result payload
    RS-->>API: Result set
    API-->>UI: Tabular or map response
```

## Planned Overhaul Component Model

```mermaid
flowchart LR
    Request[Facet request]
    Composer[Query Composer]
    Parser[Arrow Route Parser]
    Graph[Route Graph]
    Resolvers[Facet Predicate Resolvers]
    Ctes[Facet CTEs]
    Compose[INTERSECT or join composition]
    Projection[Facet content or result projection]
    Db[(SEAD PostgreSQL)]
    Response[Response payload]

    Request --> Composer
    Composer --> Parser
    Parser --> Graph
    Composer --> Resolvers
    Graph --> Resolvers
    Resolvers --> Ctes
    Ctes --> Compose
    Compose --> Projection
    Projection --> Db
    Db --> Response

    classDef input fill:#edf3ff,stroke:#6c8ebf,color:#1f2d3d;
    classDef planned fill:#fff4df,stroke:#c28b2c,color:#4a3620;
    classDef compose fill:#e8f5ec,stroke:#5b8f6a,color:#1f3527;
    classDef store fill:#f3f3f3,stroke:#8a8a8a,color:#333333;
    classDef output fill:#f7e8f0,stroke:#b56a8a,color:#472734;

    class Request input;
    class Composer,Parser,Graph,Resolvers planned;
    class Ctes,Compose,Projection compose;
    class Db store;
    class Response output;
```

## Planned Anchor-Based Query Composition Sequence

```mermaid
sequenceDiagram
    autonumber
    actor UI as Client UI
    participant API as API Controller
    participant QC as Query Composer
    participant RP as Arrow Route Parser
    participant RG as Route Graph
    participant FR as Facet Resolver Set
    participant DB as PostgreSQL

    UI->>API: Request with active facets
    API->>QC: Compose query for request
    QC->>QC: Resolve active anchor type
    loop For each active facet
        QC->>RP: Resolve route definition
        RP-->>QC: Concrete route tokens
        QC->>RG: Resolve route edges
        RG-->>QC: Table relations
        QC->>FR: Build facet predicate query
        FR-->>QC: Facet CTE
    end
    QC->>QC: Compose CTEs with INTERSECT or join
    QC->>DB: Execute composed query
    DB-->>QC: Anchor set
    QC-->>API: Filtered anchor set
    API-->>UI: Filtered response or next-stage request context
```

## Planned Facet Content Flow In The Overhaul

```mermaid
sequenceDiagram
    autonumber
    actor UI as Client UI
    participant API as API Controller
    participant QC as Query Composer
    participant FR as Facet Resolver
    participant DB as PostgreSQL

    UI->>API: Populate target facet
    API->>QC: Compose active filter set
    QC->>FR: Build target facet predicate and content query
    FR-->>QC: Anchor-filtered FCQ inputs
    QC->>DB: Execute composed anchor filter and FCQ
    DB-->>QC: Category rows and counts
    QC-->>API: FacetContent
    API-->>UI: Updated facet categories
```

## Reading Guide

- Use the current-runtime diagrams when reasoning about today’s API behavior.
- Use the overhaul diagrams when discussing the target query model on `query-engine-overhaul`.
- When the two disagree, `docs/DESIGN.md` should state which behavior is current and which is planned.