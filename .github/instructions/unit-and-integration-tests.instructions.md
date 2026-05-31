---
applyTo: 'sead.query.test/**/*.cs'
---
# Unit and Integration Test Guidance

Use this instruction when editing tests in `sead.query.test`.

## Test Stack In This Repository

- Use `xUnit` as the default test framework.
- Use `Moq` for mocks and interaction-based assertions.
- Use `FluentAssertions` for expressive assertions when it improves readability.
- Use `AutoFixture` only when it reduces repetitive setup without hiding intent.
- Use `Testcontainers` only for integration scenarios that need real external infrastructure.
- Use `Microsoft.AspNetCore.TestHost` for API-hosted integration tests.
- Do not introduce `FakeItEasy`, `Microcks`, or other additional testing frameworks unless the repository explicitly adopts them.

## Folder and Scope Conventions

- Keep unit tests under `sead.query.test/UnitTests/`.
- Keep broader integration-style tests under `sead.query.test/IntegrationTests/` or the existing feature-specific integration folders already used in the repo.
- Mirror the production namespace or feature area so tests are easy to locate.
- Add new tests close to the area they validate instead of creating catch-all test files.

## Unit Test Guidance

- Test one behavior per test.
- Name tests with the pattern `Method_Scenario_ExpectedOutcome` or equivalent clear behavior wording.
- Keep arrange, act, and assert phases visually obvious.
- Mock only true dependencies or collaborators.
- Prefer real value objects and simple concrete models over mocks for passive data.
- Verify return values, state changes, and thrown exceptions.
- Avoid file system, network, database, clock, or environment dependencies unless the test is intentionally integration-scoped.

## Integration Test Guidance

- Use integration tests to verify boundaries between layers, persistence behavior, SQL generation, DI wiring, and HTTP endpoints.
- Prefer the narrowest realistic test setup that proves the behavior.
- Use in-memory or SQLite-backed setups when they are sufficient for the scenario.
- Use PostgreSQL Testcontainers when provider-specific SQL or database behavior must be exercised.
- Keep container-backed tests deterministic and clean up resources reliably.
- Do not make tests depend on external shared environments when a local container or in-process host is sufficient.

## Assertions and Test Quality

- Assert observable behavior, not incidental implementation details.
- Avoid brittle assertions against large serialized blobs or entire SQL strings when a smaller contract can be checked.
- When SQL output is the behavior under test, assert the meaningful clauses or structure rather than unrelated formatting.
- Prefer explicit expected values over snapshots unless the snapshot is already an established pattern in the repo.
- Keep randomized data stable by controlling seeds or using deterministic inputs when failures would otherwise be hard to reproduce.

## Maintenance and Discipline

- Add or update tests when behavior changes.
- Do not rewrite large existing test areas only to match a preferred style.
- Reuse existing helpers and builders when they improve clarity.
- Remove dead or obsolete test code rather than leaving ignored scaffolding behind.
- Run focused `dotnet test` commands for the touched area after significant changes.

## What To Avoid

- Do not add tests that only assert that mocks were called unless that interaction is the real contract.
- Do not overuse mocks for domain models or simple DTOs.
- Do not make tests depend on execution order.
- Do not leave TODO-only test placeholders committed.

## Updates
This rule must be updated if new tools or practices are adopted in the backend project.
