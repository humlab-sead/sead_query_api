---
description: "Use for OPERATIONS.md, deployment/runbook docs, environments, runtime configuration, CI/CD, observability, rollback, and incident-readiness documentation in SEAD Query API."
applyTo: "docs/OPERATIONS.md"
---
# Operations Docs

## Purpose

- Use this instruction when editing `docs/OPERATIONS.md` or other operations-facing documentation.
- Keep `docs/OPERATIONS.md` focused on how the running system is built, configured, promoted, deployed, verified, monitored, rolled back, and recovered.
- Write for operators and maintainers of deployed environments, not for day-to-day feature development.
- Treat current workflow files, scripts, container definitions, and runtime configuration as the primary source of truth.

## What belongs in `docs/OPERATIONS.md`

- Environments and their roles
- Runtime configuration and secrets model
- Operational assumptions and invariants
- Operational data layout, mounted paths, and versioned assets
- Build artifacts and where they come from
- Deployment flow
- CI pipeline stages
- CD triggers and release process
- Post-deployment verification and smoke checks
- Rollback procedure
- Health checks, observability, and alerting
- Backup, recovery, and incident basics

## What does not belong in `docs/OPERATIONS.md`

- Local development setup
- Contributor workflow for writing code
- Unit-test patterns and test fixture details
- General Git tutorials
- Internal implementation detail that only matters when changing code

Those topics belong in developer-facing documentation, design/architecture documentation, or code-level documentation.

## Scope boundaries

- `docs/OPERATIONS.md`: runtime environments, release flow, deployment, verification, recovery, and operational dependencies
- `docs/DEVELOPMENT.md`: contributor workflow, commit conventions, local commands, development and testing practices
- `README.md`: short overview and entry-point links, not the full runbook
- `docs/archive/`: historical reference only; do not treat archived docs as the source of truth for current operations

## Writing rules

- Keep the document scoped, concise, and practical.
- Include enough detail for an operator or maintainer to perform supported operational tasks without reading source code first.
- Prefer concrete operational wording: branches, workflow names, image tags, env vars, config files, mounted paths, commands, and verification steps.
- Prefer short sections, concrete bullets, and explicit procedures over long narrative prose.
- Avoid background explanation, design rationale, and repeated detail that belongs in workflow files, scripts, config, or other docs.
- Distinguish build-time configuration from runtime configuration.
- Distinguish CI stages from CD triggers and deployment steps.
- Distinguish artifact creation from artifact rollout.
- Keep environment-specific procedures explicit when they differ.
- Summarize operational flow in prose, but do not duplicate low-level workflow or script logic line-by-line when repository files already define it.
- Include commands only when they are part of the supported operational procedure.
- Every section should answer a real operational question related to build, deploy, verification, rollback, recovery, or runtime maintenance.
- If a section does not support operational action, shorten it or remove it.
- When a procedure is intentionally not defined yet, keep the section present and mark it clearly as `TBD` instead of inventing process.

## Concision and size expectations

- Keep `docs/OPERATIONS.md` scoped, concise, and useful as an operational overview and runbook entry point.
- Target length: about 800-1800 words.
- Prefer staying under 2500 words.
- If the document needs to grow beyond that, move detailed or exceptional procedures into focused companion runbooks and keep `docs/OPERATIONS.md` as the concise overview and entry point.

## Sources to trust

- `docker/docker-compose.yml`, `docker/Dockerfile` — runtime service definitions
- `docker/Makefile`, `docker/Dockerfile.compiled` — build and deployment inputs
- `.github/workflows/` — CI/CD pipeline definitions
- `AGENTS.md` — canonical architecture and operational rules
- `docs/DEVELOPMENT.md` — scope boundary for local vs runtime configuration

Verify operational claims against current workflow files, Docker config, and runtime configuration before documenting them.
