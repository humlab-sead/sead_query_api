---
description: "Use for DEVELOPMENT.md and other developer-facing documentation in SEAD Query API, including local setup, contributor workflow, development practices, code quality checks, and day-to-day implementation guidance."
applyTo: "docs/DEVELOPMENT.md"
---
# Development Docs

## Purpose

Keep `docs/DEVELOPMENT.md` focused on how contributors set up, understand, modify, validate, and extend the codebase during day-to-day development. Write for developers working on the repository, not for operators of deployed environments.

When editing an existing document, preserve its structure unless reorganization is explicitly requested.

## What belongs in `docs/DEVELOPMENT.md`

- Prerequisites and local setup and bootstrap steps
- Local configuration needed for development
- Project structure and code organization
- Common development commands
- Coding conventions and repository-specific practices
- Linting, formatting, type-checking, and other code quality checks
- Local validation workflow before commit or pull request
- Database or migration workflow, if relevant to development
- Debugging and troubleshooting guidance for common development issues
- Pointers to related design, testing, and operations documentation

## Scope boundaries

- `docs/DEVELOPMENT.md`: contributor workflow, local setup, repository conventions, common commands, and day-to-day development practices
- `docs/OPERATIONS.md`: runtime environments, deployment, release flow, verification, rollback, recovery, and operational dependencies
- `docs/DESIGN.md`: architecture, major design decisions, system structure, and rationale
- `docs/TESTING.md`: test strategy, test levels, quality goals, and repository-specific testing guidance
- `README.md`: short project overview and entry-point links, not the full developer guide
- `docs/archive/`: historical reference only; do not treat archived docs as the source of truth for current development practice

## Writing rules

- Include enough detail for a contributor to set up the project, run it locally, validate changes, and follow expected repository practices without unnecessary background reading.
- Prefer concrete repository-specific wording: commands, scripts, file paths, config files, and tools over generic descriptions.
- Avoid repeating information already defined in scripts, config, or workflow files when a short explanation plus a reference is enough.
- Distinguish one-time setup from day-to-day development workflow; distinguish local dev configuration from runtime or deployment configuration; distinguish developer validation from CI behavior.
- Keep repository-specific conventions explicit when they differ from generic .NET practice.
- Every section should answer a real developer question. If it does not support developer action, shorten it or remove it.
- Mark intentionally undefined processes `TBD` rather than inventing process.
- Target 800–1800 words; stay under 2500. Move specialized guidance to companion documents rather than expanding this file.

## Sources to trust

- `Makefile` — supported development commands
- `sead_query_api.sln` and `*.csproj` — solution structure, target frameworks, and project relationships
- `.vscode/tasks.json` — editor-supported restore, build, and test commands
- `AGENTS.md` — canonical conventions and architecture rules
- `README.md` — project overview and entry-point links
- `docs/DESIGN.md`, `docs/OPERATIONS.md` — cross-references for scope boundaries

Verify development claims against current scripts, config, tool definitions, and repository structure before documenting them.