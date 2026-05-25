# API Services Agent Guide

Use this guide when editing `sead.query.api/Services/`.

## Start Here

- Start from the specific service called by the controller or startup wiring.
- Check `Reconstitute/` for view-state and request reconstruction flows.
- Step into `core`, `composer`, or `infra` only when this folder is clearly delegating.

## Service Rules

- Keep services as API-facing orchestration.
- Preserve explicit dependencies through constructor injection.
- Do not hide persistence logic here when it belongs in `sead.query.infra`.

## Cheap Validation

- Prefer the narrowest integration or host-backed test touching the same service path.
