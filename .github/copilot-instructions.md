# SEAD Query API Copilot Instructions

Keep this file short, always-on, and repo-specific. Put detailed or task-specific guidance in `.github/instructions/*.instructions.md` so it loads only when relevant.

## Source of truth

- Prefer current docs in `docs/` and checked-in runtime/build files over old chat context or archived notes.
- Ignore `docs/archive/` unless the user explicitly asks for historical context.
- Treat `docs/proposals/` as design intent, not current behavior, unless the user asks about planned changes.
- Start with `docs/DEVELOPMENT.md`, `docs/DESIGN.md`, `docs/TESTING.md`, `docs/OPERATIONS.md`, `docs/DIAGRAMS.md`, and `README.md`.

## Repository shape

This repository is a .NET solution centered on these projects:

- `sead.query.api/`: ASP.NET Core API host and HTTP-facing composition
- `sead.query.core/`: domain models, query-building abstractions, and shared core logic
- `sead.query.composer/`: query composition, route handling, and SQL-generation logic
- `sead.query.infra/`: infrastructure, repositories, caching, and integration helpers
- `sead.query.test/`: unit, integration, and live test coverage

## Always-on architecture rules

- Keep controller and DTO concerns in `sead.query.api`; keep domain and query behavior in `core` and `composer`.
- Prefer dependency injection and explicit interfaces over static state or hidden service lookups.
- Do not move business logic into controllers, DTOs, or configuration classes.
- Keep infrastructure concerns in `sead.query.infra` unless the code clearly belongs to the API boundary.
- Treat the current runtime as authoritative; planned query-engine overhaul work must not be described as already shipped.

## Workflow expectations

- Target .NET 9 and existing solution/project structure.
- Use the root `Makefile`, VS Code tasks, or `dotnet` commands already present in the repo.
- Run targeted tests for the changed slice before finishing; widen validation only when the change crosses layers.
- Keep changes small and aligned with existing naming, nullability, async, and DI patterns.
- When a task is about the SEAD schema, table relationships, or safe SQL join paths, load the local skill at `.github/skills/sead-database/SKILL.md`.

## Task-specific instruction files

Use the focused instruction files instead of expanding this file:

- `coding.instructions.md`: C# coding rules for source files
- `unit-and-integration-tests.instructions.md`: test authoring conventions in `sead.query.test`
- `design.instructions.md`: guidance for `docs/DESIGN.md`
- `development.instructions.md`: guidance for `docs/DEVELOPMENT.md`
- `testing.instructions.md`: guidance for `docs/TESTING.md`
- `operations.instructions.md`: guidance for `docs/OPERATIONS.md`
- `readme.instructions.md`: guidance for `README.md`
- `diagrams.instructions.md`: Mermaid diagram conventions
- `proposal-writing-guide.instructions.md`: proposal-writing guidance in `docs/proposals/`
- `phase-plan.instructions.md`: guidance for ordered multi-phase implementation plans
- `task-plan.instructions.md`: guidance for actionable per-phase task plans
- `github-workflow.instructions.md`: issue, branch, and commit workflow guidance
- `conventional-commits.instructions.md`: commit message format
