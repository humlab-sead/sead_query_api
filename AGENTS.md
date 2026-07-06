# SEAD Query API Agent Guide

This file provides always-on guidance for coding agents working in this repository. Keep it short, repo-specific, and focused on current behavior.

When a deeper folder contains its own `AGENTS.md`, prefer the nearest one for task-local guidance and use this file as the repository-wide baseline.

## Instruction precedence

* Follow the user's explicit request first.
* Follow the nearest applicable `AGENTS.md` for task-local guidance.
* Follow focused instruction files under `.github/instructions/` when they match the task.
* Treat current repository files and source-of-truth docs as authoritative when instructions appear stale or incomplete.
* Do not invent architecture, schema relationships, runtime behavior, CI behavior, or migration status.

## Source of truth

* Prefer current files in `docs/`, the solution projects, and checked-in build/runtime assets over old chat context or archived notes.
* Ignore `docs/archive/` unless the user asks for historical context.
* Treat `docs/proposals/` as planned design, not shipped behavior, unless the task is proposal work.
* Use `.github/architecture-map.yaml` as the compact architecture overview for design and migration tasks.
* Start with `README.md`, `docs/DEVELOPMENT.md`, `docs/DESIGN.md`, `docs/TESTING.md`, `docs/OPERATIONS.md`, and `docs/DIAGRAMS.md`.
* Use `docs/DEVELOPMENT.md` for document-placement guidance, including when to use proposals, phase plans, task plans, durable docs, or archives.

## Repository shape

This repository is a .NET 9 solution with these main projects:

* `sead.query.api/`: ASP.NET Core API host and HTTP-facing composition
* `sead.query.core/`: domain models and shared query abstractions
* `sead.query.composer/`: query composition, route handling, and SQL-generation work
* `sead.query.infra/`: repositories, caching, and infrastructure concerns
* `sead.query.test/`: unit, integration, and live test coverage

## Architecture rules

* Keep API boundary concerns in `sead.query.api`.
* Keep domain and query logic in `sead.query.core` and `sead.query.composer`.
* Keep infrastructure and repository implementations in `sead.query.infra`.
* Prefer dependency injection and explicit interfaces over static state or hidden lookups.
* Do not move business logic into controllers, DTOs, or configuration classes.
* Treat the current runtime as authoritative; planned query-engine overhaul work must not be described as already shipped.

## Working style

* Use the existing solution structure, root `Makefile`, VS Code tasks, and `dotnet` commands already present in the repo.
* Keep edits small and aligned with current naming, nullability, async, and DI patterns.
* Run targeted validation for the touched slice before finishing, and widen scope only when the change crosses layers.
* Do not invent operational or CI behavior that is not defined in the repository; mark missing process as `TBD` in docs.
* For architecture or design-oriented work, read and follow `.github/skills/sead-architecture-design/SKILL.md`.
* For work that depends on the SEAD database model, join paths, or repository-safe SQL, read and follow `.github/skills/sead-database/SKILL.md`.

## Detailed instructions

Use the focused instruction files under `.github/instructions/` instead of expanding this file:

* `coding.instructions.md`: C# coding instructions
* `csharp-summaries.instructions.md`: guidance for XML summary comments on C# types
* `csharp-polymorphic-design.instructions.md`: use polymorphic design patterns instead of type-code conditionals
* `unit-and-integration-tests.instructions.md`: unit, integration, and live test guidance
* `design.instructions.md`: guidance for `docs/DESIGN.md`
* `development.instructions.md`: guidance for `docs/DEVELOPMENT.md`
* `testing.instructions.md`: guidance for `docs/TESTING.md`
* `operations.instructions.md`: guidance for `docs/OPERATIONS.md`
* `readme.instructions.md`: guidance for `README.md`
* `diagrams.instructions.md`: Mermaid diagram guidance
* `proposal-writing-guide.instructions.md`: proposal or change-request documents under `docs/proposals/`
* `phase-plan.instructions.md`: ordered multi-phase implementation plans
* `task-plan.instructions.md`: actionable per-phase task plans with work breakdown, validation, and definition of done
* `github-workflow.instructions.md`: issue, branch, and commit workflow guidance
* `conventional-commits.instructions.md`: commit message format
* `glossary.instructions.md`: guidance for `docs/GLOSSARY.md`

## Documentation workflow

* Use a proposal document when the reader needs to decide whether to do the work.
* Use a phase plan when the decision is made and the reader needs the ordered path from current state to target state.
* Use a task plan when one phase needs concrete implementation steps and tracked execution.
* Move long-lived truth into `docs/GLOSSARY.md`, `docs/REQUIREMENTS.md`, `docs/DESIGN.md`, `docs/DEVELOPMENT.md`, `docs/TESTING.md`, or `docs/OPERATIONS.md` once it should outlive a proposal.
* Archive material that is historical and no longer authoritative.

For GitHub Copilot-specific always-on guidance, also see `.github/copilot-instructions.md`.

## graphify

This project has a knowledge graph at graphify-out/ with god nodes, community structure, and cross-file relationships.

When the user types `/graphify`, invoke the `skill` tool with `skill: "graphify"` before doing anything else.

Rules:
- For codebase questions, first run `graphify query "<question>"` when graphify-out/graph.json exists. Use `graphify path "<A>" "<B>"` for relationships and `graphify explain "<concept>"` for focused concepts. These return a scoped subgraph, usually much smaller than GRAPH_REPORT.md or raw grep output.
- Dirty graphify-out/ files are expected after hooks or incremental updates; dirty graph files are not a reason to skip graphify. Only skip graphify if the task is about stale or incorrect graph output, or the user explicitly says not to use it.
- If graphify-out/wiki/index.md exists, use it for broad navigation instead of raw source browsing.
- Read graphify-out/GRAPH_REPORT.md only for broad architecture review or when query/path/explain do not surface enough context.
- After modifying code, run `graphify update .` to keep the graph current (AST-only, no API cost).

## rtk (Token Optimization)

Prefix shell commands with `rtk` to compress output and reduce token usage (saves 60–90%). Examples: `rtk uv run pytest`, `rtk make test`, `rtk git log -10`. Use `rtk gain` for analytics and `rtk proxy <cmd>` for raw output. See `RTK.md` for details.
