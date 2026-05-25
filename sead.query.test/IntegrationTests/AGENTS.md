# Integration Tests Agent Guide

Use this guide when editing `sead.query.test/IntegrationTests/`.

## Start Here

- Start with the nearest host, controller, or persistence test already covering the same boundary.
- Use `Startup.cs`, `TestHostBuilder.cs`, and `TestHostFixture.cs` before inventing new host setup.
- Keep the scenario as narrow as possible while still crossing the intended boundary.

## Working Rules

- Use integration tests for HTTP, DI wiring, repository behavior, and provider-specific SQL or persistence.
- Reach for containers only when in-process or lighter-weight setups cannot prove the behavior.
- Avoid broad end-to-end tests when a smaller host-backed slice is enough.
