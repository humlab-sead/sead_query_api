# SEAD Query API Copilot Instructions

Keep this file short, always-on, and repo-specific. Put detailed path-specific or task-specific guidance in `.github/instructions/*.instructions.md` with an appropriate `applyTo` front matter pattern.

## Source of truth

- Prefer current docs in `docs/` and checked-in runtime/build files over old chat context, assumptions, or archived notes.
- Ignore `docs/archive/` unless the user explicitly asks for historical context.
- Treat `docs/proposals/` as design intent, not current behavior, unless the user asks about planned changes.
- Use `.github/architecture-map.yaml` as the compact architecture overview when the task is about design, boundaries, runtime flows, or migration shape.
- Start with `README.md`, `docs/DEVELOPMENT.md`, `docs/DESIGN.md`, `docs/TESTING.md`, `docs/OPERATIONS.md`, and `docs/DIAGRAMS.md`.
- Do not invent architecture, runtime behavior, schema relationships, or migration status. Verify against repository files.

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
- When repository-wide and path-specific instructions both apply, follow the more specific instruction unless it conflicts with current code or source-of-truth docs.

## Workflow expectations

- Target .NET 10 and the existing solution/project structure.
- Use the root `Makefile`, VS Code tasks, or `dotnet` commands already present in the repo.
- Run targeted tests for the changed slice before finishing; widen validation only when the change crosses layers.
- Keep changes small and aligned with existing naming, nullability, async, and DI patterns.
- For architecture or design work, read and follow `.github/skills/sead-architecture-design/SKILL.md`.
- For SEAD schema, table relationships, or safe SQL join paths, read and follow `.github/skills/sead-database/SKILL.md`.

## Task-specific instruction files

Use the focused instruction files instead of expanding this file. Each `.instructions.md` file should include `applyTo` front matter so Copilot applies it only where relevant.

- `coding.instructions.md`: C# coding rules for source files
- `csharp-summaries.instructions.md`: guidance for XML summary comments on C# types and extension points
- `csharp-polymorphic-design.instructions.md`: guidance for using polymorphic design patterns instead of type-code conditionals in C#
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
- `glossary.instructions.md`: guidance for `docs/GLOSSARY.md`
- `wricting-style.instructions.md` - concrete wording for docs, comments, and PR text

## graphify

For repo architecture or relationship questions, follow the graphify quick start in `AGENTS.md`.

<!-- rtk-instructions v2 -->
**rtk** is a CLI proxy that filters and compresses command outputs, saving 60-90% tokens.

Use `rtk` for shell commands unless raw output, shell built-ins, or interactive commands require otherwise.

Examples:
```bash
rtk uv run pytest
rtk make test
rtk pylint src/
rtk git status
rtk git log -10
```

If `rtk` fails, retry without it.

Meta: `rtk gain`, `rtk gain --history`, `rtk discover`, `rtk proxy <cmd>`
<!-- /rtk-instructions -->