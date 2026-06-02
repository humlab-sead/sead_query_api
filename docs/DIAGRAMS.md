# Architecture Diagrams

This document provides visual diagrams for the SEAD Query API.

Use `docs/DESIGN.md` as the primary written architecture reference. These diagrams summarize the current runtime, the core application flows, the active composer-based query baseline, and the explicit compatibility boundaries that still remain.

## Diagram Status

- Current runtime diagrams reflect the current application baseline, including the composer-owned query path used for the validated composed surface.
- Compatibility diagrams show where legacy fallback still exists for unsupported request families.
- Detailed deployment and operations diagrams are `TBD`.

## System Overview

```mermaid
%%{init: {
  "themeCSS": ".edgeLabel { background-color: transparent !important; } .edgeLabel p { background-color: transparent !important; }"
}}%%
flowchart TB
    Client["Client UI"]
    Api["sead.query.api"]
    Core["sead.query.core"]
    Infra["sead.query.infra"]
    Composer["sead.query.composer"]
    Db[("SEAD PostgreSQL")]
    Tests["sead.query.test"]

    Client -->|"Facet/result<br/>requests"| Api

    Api -->|"Domain/query<br/>contracts"| Core
    Api -->|"Composed query<br/>services"| Composer
    Api -->|"Repositories<br/>and services"| Infra

    Core -->|"Query<br/>contracts"| Composer
    Infra -->|"Config<br/>and data"| Db
    Composer -->|"Routes<br/>and SQL"| Db

    Tests -. "Runtime<br/>behavior" .-> Api
    Tests -. "Compilers<br/>and services" .-> Core
    Tests -. "Repositories<br/>and fixtures" .-> Infra
    Tests -. "Composed<br/>query paths" .-> Composer

    classDef edge fill:#edf3ff,stroke:#6c8ebf,color:#1f2d3d;
    classDef runtime fill:#e8f5ec,stroke:#5b8f6a,color:#1f3527;
    classDef compose fill:#fff4df,stroke:#c28b2c,color:#4a3620;
    classDef store fill:#f3f3f3,stroke:#8a8a8a,color:#333333;
    classDef test fill:#f7e8f0,stroke:#b56a8a,color:#472734;

    class Client edge;
    class Api,Core,Infra runtime;
    class Composer compose;
    class Db store;
    class Tests test;
```

## Current Request Surface Summary

```mermaid
flowchart TB
    Request[Client request]
    Reconstitute[Reconstitute request models]
    Clean[Normalize active selections]
    Decide{Facet or result?}
    Facet[Facet content path]
    Result[Result projection path]
    Compile[Compile SQL]
    Payload[Build response payload]
    Response[HTTP response]
    Db[(SEAD PostgreSQL)]

    Request --> Reconstitute --> Clean --> Decide
    Decide --> Facet --> Compile
    Decide --> Result --> Compile
    Compile --> Db
    Db --> Payload --> Response

    classDef input fill:#edf3ff,stroke:#6c8ebf,color:#1f2d3d;
    classDef process fill:#e8f5ec,stroke:#5b8f6a,color:#1f3527;
    classDef store fill:#f3f3f3,stroke:#8a8a8a,color:#333333;
    classDef output fill:#fff4df,stroke:#c28b2c,color:#4a3620;

    class Request input;
    class Reconstitute,Clean,Facet,Result,Compile,Payload process;
    class Db store;
    class Response output;
```

## Application Startup And Runtime Composition

```mermaid
sequenceDiagram
    autonumber
    actor Ops as Host process
    participant Host as Program and Startup
    participant CFG as App configuration
    participant DI as DI container
    participant API as Controllers and middleware
    participant SVC as Query and domain services
    participant INF as Infra repositories
    participant DB as PostgreSQL

    Ops->>Host: Start API host
    Host->>CFG: Load appsettings and environment
    CFG-->>Host: Runtime settings
    Host->>DI: Register API, core, infra, and composer services
    DI-->>Host: Service graph
    Host->>API: Start HTTP pipeline
    API-->>Ops: Runtime ready
    API->>DI: Resolve scoped services per request
    DI-->>API: Controllers, services, repositories
    API->>SVC: Execute request flow
    SVC->>INF: Load config and data as needed
    INF->>DB: Query runtime data and facet config
    DB-->>INF: Rows and active configuration
```

## Configuration Authoring, Import, And Runtime Use

```mermaid
sequenceDiagram
    autonumber
    actor Author as Maintainer
    participant Host as Host entry point
    participant VAL as Config validator
    participant IMP as Config importer
    participant DB as PostgreSQL facet schema
    participant API as API runtime
    participant REP as Config repository

    Author->>Host: Run validate-facet-config
    Host->>VAL: Validate YAML and routes
    VAL-->>Host: Validation result
    Author->>Host: Run import-facet-config
    Host->>IMP: Normalize and import configuration
    IMP->>DB: Write facet config and config_revision
    DB-->>IMP: Active revision stored
    Author->>API: Start or continue runtime
    API->>REP: Load active facet configuration
    REP->>DB: Read active revision and routes
    DB-->>REP: Runtime configuration
    REP-->>API: Active config models
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
    participant CFS as ComposedFacetContentService
    participant LGC as Legacy facet compiler
    participant DB as PostgreSQL

    UI->>API: Populate facet request
    API->>RC: Reconstitute request payload
    RC-->>API: FacetsConfig
    API->>FL: Load target facet content
    FL->>BP: Remove invalid picks
    BP-->>FL: Normalized FacetsConfig
    FL->>FS: Load normalized target facet
    FS->>FS: Check composed support boundary
    alt Supported composed request
        FS->>CFS: Load composed facet content
        CFS->>DB: Execute composed facet and count SQL
        DB-->>CFS: Category rows and counts
        CFS-->>FS: FacetContent
    else Unsupported request
        FS->>LGC: Compile legacy facet SQL
        LGC->>DB: Execute legacy count query
        DB-->>LGC: Category rows and counts
        LGC-->>FS: FacetContent
    end
    FS-->>FL: FacetContent
    FL-->>API: FacetContent
    API-->>UI: Facet response
```

## Current Facet Capability Boundary

```mermaid
sequenceDiagram
    autonumber
    participant FL as FacetLoadService
    participant FS as FacetContentService
    participant CFS as ComposedFacetContentService
    participant LEG as Legacy category path

    FL->>FS: Load(facetsConfig, targetFacet)
    FS->>CFS: CanHandle(request)
    alt Supported composed request
        CFS-->>FS: true
        FS->>CFS: Load composed content
        CFS-->>FS: FacetContent
    else Unsupported request
        CFS-->>FS: false
        FS->>LEG: Load legacy category content
        LEG-->>FS: FacetContent
    end
    FS-->>FL: FacetContent
```

## Current Result Generation Sequence

```mermaid
sequenceDiagram
    autonumber
    actor UI as Client UI
    participant API as ResultController
    participant RC as ReconstituteConfigService
    participant LRS as LoadResultService
    participant RS as ResultService
    participant HB as ResultProjectionHandoffBuilder
    participant SQL as Result SQL Compiler
    participant PAY as Result Payload Service
    participant DB as PostgreSQL

    UI->>API: Result request with facet state
    API->>RC: Reconstitute facet and result config
    RC-->>API: FacetsConfig and ResultConfig
    API->>LRS: Load result set
    LRS->>RS: Load(FacetsConfig, ResultConfig)
    RS->>HB: Build result handoff
    HB-->>RS: QuerySetup and ResultFields
    RS->>SQL: Compile(handoff.QuerySetup, result facet, handoff.ResultFields)
    SQL->>DB: Execute result SQL
    DB-->>SQL: Result rows
    RS->>PAY: Load extra payload for view type
    PAY-->>RS: Payload
    RS-->>LRS: ResultContentSet
    LRS-->>API: ResultContentSet
    API-->>UI: Tabular or map response
```

## Current Result Handoff Boundary

```mermaid
sequenceDiagram
    autonumber
    participant RS as ResultService
    participant HB as ResultProjectionHandoffBuilder
    participant CH as Composed Handoff Builder
    participant LH as Legacy Handoff Builder
    participant SQL as Result SQL Compiler

    RS->>HB: Build(facetsConfig, resultConfig)
    alt Supported composed request
        HB->>CH: Resolve composed result handoff
        CH->>CH: Build composed_filter
        opt Routed result target
            CH->>CH: Build target_route
        end
        CH-->>HB: QuerySetup and ResultFields
    else Unsupported request
        HB->>LH: Build legacy query setup
        LH-->>HB: QuerySetup and ResultFields
    end
    HB-->>RS: ResultProjectionHandoff
    RS->>SQL: Compile final result SQL
```

## Current Composed Result Handoff Internals

```mermaid
sequenceDiagram
    autonumber
    participant CH as ComposedResultProjectionHandoffBuilder
    participant PF as Path Finder
    participant PR as Predicate Resolver
    participant RC as Route SQL Compiler
    participant CF as Composed Filter Composer

    CH->>PF: Resolve target and anchor path
    PF-->>CH: Route path
    CH->>PR: Resolve predicate plan
    PR-->>CH: Predicate SQL fragments
    opt Routed result target
        CH->>RC: Compile target route SQL
        RC-->>CH: target_route CTE
    end
    CH->>CF: Compose anchor filter SQL
    CF-->>CH: composed_filter CTE
    CH-->>CH: Return QuerySetup.LeadingSql, joins, and result fields
```

## Current Composer Component Model

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
    classDef composer fill:#fff4df,stroke:#c28b2c,color:#4a3620;
    classDef compose fill:#e8f5ec,stroke:#5b8f6a,color:#1f3527;
    classDef store fill:#f3f3f3,stroke:#8a8a8a,color:#333333;
    classDef output fill:#f7e8f0,stroke:#b56a8a,color:#472734;

    class Request input;
    class Composer,Parser,Graph,Resolvers composer;
    class Ctes,Compose,Projection compose;
    class Db store;
    class Response output;
```

## Current Composed Query Sequence

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

## Current Composed Facet Content Sequence

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

- Use the system and startup diagrams when discussing the application as a whole.
- Use the facet and result sequences when tracing request execution and compatibility boundaries.
- Use the composer diagrams when reasoning about the current composed query baseline.
- When a diagram and prose differ, `docs/DESIGN.md` is the primary written source of truth.