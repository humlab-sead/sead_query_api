---
name: sead-database
description: "Understand and work with the SEAD PostgreSQL database model. Use when answering questions about SEAD schema, sites, locations, sample groups, physical samples, datasets, analysis entities, taxa, chronology, bibliography, and safe SQL joins across SEAD-derived applications."
argument-hint: "Describe the SEAD schema, SQL, join path, or change task"
---

# SEAD Database

Use this skill when the task is about the SEAD database model rather than generic PostgreSQL advice.

## What This Skill Covers

- Explaining SEAD concepts at the model level rather than as one repository's implementation details
- Working from trusted schema sources such as live schema exports, checked-in DDL, comments, or catalog extracts supplied by the current workspace
- Mapping common entity chains such as site to sample group to physical sample to analysis entity to dataset or abundance
- Writing or reviewing SQL against the core SEAD model used by SEAD applications
- Explaining where dating, bibliography, methods, and provenance metadata live

## Core Rules

- Never invent table names, columns, foreign keys, or domain relationships.
- Prefer trusted schema facts from the active workspace or live schema export over generic archaeology assumptions.
- Distinguish between conceptual entities, physical tables, application behavior, and repository workflow.
- Ask for clarification when the user does not specify site scope, dataset scope, method, chronology, or whether they want abundance data or typed analysis values.
- When writing SQL, state assumptions and note when the query should be validated against a live or deployed schema.
- Keep repository-specific migration or deployment guidance separate unless the current workspace explicitly supplies it.

## Procedure

1. Start with [schema overview](./references/schema-overview.md) to place the question in the right area of the model.
2. Use trusted workspace-specific schema snapshots or live exports first when they exist.
3. Use [schema summary](./references/sead-schema-summary.md) for a compact table-family map.
4. Use [core entities](./references/core-entities.md) to explain SEAD terms precisely.
5. Use [join patterns](./references/join-patterns.md) to derive the shortest verified path between entities.
6. Use [SQL style guide](./references/sql-style-guide.md) when writing or reviewing queries.
7. Use [glossary](./references/glossary.md) when the user needs domain vocabulary.
8. If the current workspace has repository-specific SEAD database workflow guidance, load that separately.

## Output Expectations

- Explain the join path before presenting complex SQL.
- Call out where multiplicative joins are likely.
- Separate verified facts from assumptions.
- Prefer small, reviewable SQL snippets over large speculative queries.