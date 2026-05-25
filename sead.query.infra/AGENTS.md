# Infra Agent Guide

Use this guide when working inside `sead.query.infra/`.

## Start Here

- Start with the nearest implementation in `Repository/`, `Cache/`, or `Encryption/`.
- Use this project for data access, caching, repository registry wiring, and infrastructure helpers.
- Step outward to callers only after confirming the infrastructure contract in play.

## Boundaries

- Keep database access and repository implementation here.
- Do not move business rules or HTTP concerns into infrastructure classes.
- Prefer explicit repository interfaces and constructor-injected collaborators.

## Cheap Validation

- Prefer the narrowest integration or repository-focused test that hits the changed behavior.
