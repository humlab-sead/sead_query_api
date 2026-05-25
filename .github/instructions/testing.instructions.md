---
description: "Use for TESTING.md and other testing-focused documentation in sead_query_api, including test strategy, test levels, validation scope, local test workflow, and repository-specific .NET testing guidance."
applyTo: "docs/TESTING.md"
---
# Testing Docs

## Purpose

- Use this instruction when editing `docs/TESTING.md` or other testing-focused documentation.
- Keep `docs/TESTING.md` focused on how the codebase is validated, what kinds of tests exist, what each test level is responsible for, and how contributors should run and interpret tests.
- Write for developers and maintainers who need to understand or apply the repository's testing approach.
- Treat the current test suite, test configuration, CI workflows, and repository tooling as the primary source of truth.

## What belongs in `docs/TESTING.md`

- Testing purpose and intended audience
- Testing goals and quality expectations
- Test levels and their responsibilities
- Repository-specific testing conventions
- What should be covered by unit, integration, contract, and end-to-end tests, if those levels exist
- Test execution commands
- Test environment expectations
- Test data, fixtures, mocks, and stubs at a policy level
- Validation expectations before merge
- Relationship between local testing and CI validation
- Common testing pitfalls and troubleshooting guidance
- Pointers to related development, design, and operations documentation

## What does not belong in `docs/TESTING.md`

- Local environment bootstrap instructions that belong in development documentation
- Production deployment, rollback, backup, or runtime incident procedures
- High-level system design rationale that belongs in design documentation
- Exhaustive endpoint reference that should come from generated API docs or code
- Detailed line-by-line explanations of individual test files
- Large catalogs of specific test cases better kept close to the code
- Generic tutorials on test frameworks or CI systems unless they are repository-specific

Those topics belong in development documentation, operations documentation, design/architecture documentation, generated API documentation, or code-level documentation.

## Scope boundaries

- `docs/TESTING.md`: test strategy, test levels, quality expectations, validation workflow, and repository-specific testing guidance
- `docs/DEVELOPMENT.md`: contributor workflow, local setup, common commands, and day-to-day development practices
- `docs/OPERATIONS.md`: runtime environments, deployment, release flow, rollback, recovery, and operational dependencies
- `docs/DESIGN.md`: architecture, major design decisions, system structure, and rationale
- `README.md`: short overview and entry-point links, not the full testing guide
- `docs/archive/`: historical reference only; do not treat archived docs as the source of truth for current testing practice

## Writing rules

- Keep the document scoped, concise, and practical.
- Include enough detail for a contributor to understand the repository's testing model, choose the right test level, run the relevant checks, and interpret expected results.
- Prefer concrete repository-specific wording: test commands, directories, test filters, fixtures, environment requirements, and CI checks.
- Prefer short sections, concrete bullets, and explicit guidance over long narrative prose.
- Avoid generic testing theory unless it directly explains a repository-specific rule or convention.
- Distinguish clearly between test levels such as unit, integration, contract, and end-to-end tests when they exist.
- Distinguish local validation workflow from CI pipeline execution.
- Distinguish test policy from implementation detail in individual test modules.
- Describe fixture, mock, and test-data strategy at the level of guidance and boundaries, not as an inventory of every fixture.
- Include commands only when they are part of the supported testing workflow.
- Every section should answer a real testing question related to scope, execution, coverage responsibility, failure interpretation, or validation before merge.
- If a section does not support testing action or testing understanding, shorten it or remove it.
- When a testing process or expectation is intentionally not defined yet, keep the section present and mark it clearly as `TBD` instead of inventing process.
- Reflect the actual repository stack: `xUnit`, `Moq`, `FluentAssertions`, `AutoFixture`, `Testcontainers`, and `Microsoft.AspNetCore.TestHost` where relevant.
- Distinguish active test locations from excluded or non-authoritative areas such as `Purgatory/` or `QueryComposer/BackBurner/` when those are not part of the normal test build.

## Concision and size expectations

- Keep `docs/TESTING.md` scoped, concise, and useful as the main testing guide and entry point.
- Target length: about 800-1800 words.
- Prefer staying under 2500 words.
- If the document needs to grow beyond that, move detailed or specialized guidance into focused companion documents and keep `docs/TESTING.md` as the concise overview and entry point.

## Sources to trust

- `sead.query.test/` — test suite structure and test patterns
- `sead.query.test/sead.query.test.csproj` — test framework, package references, and copied test configuration
- `.vscode/tasks.json` — editor-supported restore, build, and test commands
- `Makefile` — supported local test and test-data commands
- `conf/appsettings.Test.json` and `conf/.env` — local test configuration inputs
- `.github/workflows/` — CI or release automation that may affect validation expectations
- `docs/DEVELOPMENT.md` and `docs/DESIGN.md` — cross-references for workflow and architecture boundaries

Verify testing claims against the current test suite, test configuration, scripts, and CI workflows before documenting them.