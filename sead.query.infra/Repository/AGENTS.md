# Repository Agent Guide

Use this guide when editing `sead.query.infra/Repository/`.

## Start Here

- Start with the specific repository or registry involved in the failing behavior.
- Check `RepositoryRegistry.cs` when the issue is lookup or wiring related.
- Stay near the touched repository instead of scanning all repository classes.

## Working Rules

- Keep SQL and data mapping explicit.
- If the defect is really query composition, step back to `sead.query.composer/` rather than layering fixes here.
- Preserve repository contracts used by API and composer callers.

## Cheap Validation

- Prefer integration tests that exercise the affected repository boundary.
