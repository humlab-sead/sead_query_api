## Purpose

This document describes how the SEAD Query API is built, configured, deployed, verified, monitored, and recovered in supported runtime environments.

It is an operations-facing guide for maintainers of deployed environments. It is not a local development guide and does not describe internal implementation details beyond what is needed to operate the service.

## Operational Status

- Primary runtime model in the repository: containerized ASP.NET Core service plus Redis, defined in `docker/docker-compose.yml`.
- Primary build inputs: `docker/Dockerfile`, `docker/Dockerfile.compiled`, and `docker/Makefile`.
- Release automation present: `.github/workflows/release.yml`.
- Full CI gate, formal environment promotion flow, and documented rollback automation are `TBD`.

## Runtime Environments

The repository clearly supports at least two runtime contexts:

- local or operator-managed container runtime via Docker Compose
- GitHub-hosted release automation for tagging and release-note generation on `main`

Environment roles visible in the repository:

- `Development`: referenced by `docker/.env` and local development configuration
- `Production`: represented by `conf/appsettings.Production.json` and containerized runtime settings
- `Test`: represented by `conf/appsettings.Test.json`, but used for validation rather than operational deployment

Additional named environments, promotion stages, or cluster tiers are not fully documented in the repository and should be treated as `TBD` until explicitly defined.

## Runtime Topology

The Docker Compose definition describes a small runtime topology.

- `sead_query_api`: the main .NET service container
- `redis.cache`: Redis sidecar used as a cache dependency

Operational characteristics from `docker/docker-compose.yml`:

- the API container is built from the local Docker context using `docker/Dockerfile`
- the API image is tagged with `${IMAGE_NAME}:${IMAGE_TAG}`
- the service restarts automatically
- the service exposes container port `8089` and publishes host port `8090`
- Redis exposes `6379/tcp` internally on the compose network
- both services run on the `sead_query_net` network with fixed internal addresses

This is the operational baseline reflected by the repository. If production runs in a different topology, that deployment detail is not currently documented here and should be added separately rather than assumed.

## Runtime Configuration and Secrets

Operational runtime configuration is split between mounted config files, environment variables, and deployment-local env files.

### Mounted configuration

The compose file mounts:

- `./sead_query_api/conf` to `${ASPNETCORE_APPSETTINGS_FOLDER}`

The configured runtime folder is:

- `/app/config`

Relevant configuration files in the repository include:

- `conf/appsettings.Production.json`
- `conf/appsettings.Development.json`
- `conf/hosting.json`

The container runtime also sets:

- `ASPNETCORE_APPSETTINGS_FOLDER=/app/config`
- `ASPNETCORE_URLS=http://0.0.0.0:8089`

### Environment files

The Docker Compose service references:

- `build.env`

That file is not present in the repository root as checked in here, so it should be treated as deployment-environment material managed outside the committed source tree.

The Docker tooling in `docker/` also uses:

- `docker/.env`

This file currently defines image tags, branch/tag selection, build mode, and ASP.NET environment values.

### Secrets model

The repository does not provide a full centralized secrets-management runbook.

Current observable practice:

- local and scripted workflows use env files
- some helper commands outside the Docker deployment path use vault-backed files in `~/vault/`
- production app settings include host, database, and cache settings in JSON

The supported production secrets-management model should therefore be documented as partially externalized and `TBD` at a policy level. Do not assume the repository itself is the full secret source of truth.

## Build Artifacts

Two image-build paths are currently described.

### Source-based image build

`docker/Dockerfile`:

- clones the repository for the specified `BRANCH`
- restores the API project and its project references
- publishes `sead.query.api`
- produces a runtime image based on `mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim`

This is the most current container build path in the repository.

### Host-compiled image build

`docker/Makefile` also supports `image-host-compiled`, which:

- runs `dotnet publish` on the host into `docker/publish`
- builds an image from `docker/Dockerfile.compiled`

Operational note:

- `docker/Dockerfile.compiled` still uses an ASP.NET 8.0 base image while the active projects target .NET 9

Treat that path as under review until the runtime base image matches the current framework target.

## Supported Build and Deployment Commands

From `docker/Makefile`, the current supported operational commands are:

Build the main image:

```bash
cd docker
make image
```

Build from host-published binaries:

```bash
cd docker
make image-host-compiled
```

Open a shell in the built image:

```bash
cd docker
make bash
```

Restart the configured Docker Compose cluster:

```bash
cd docker
make restart-docker-compose-cluster
```

These commands are the operational surface currently expressed in the repository. If a different deployment system exists outside the repo, it should be documented explicitly instead of inferred here.

## Deployment Flow

The repository suggests the following operational rollout flow.

1. Select a release tag or branch through `SEAD_QUERY_API_TAG`.
2. Build the API image with `docker/Makefile`.
3. Deploy or restart the Docker Compose cluster in the target deployment location.
4. Use mounted config and environment files to supply runtime settings.
5. Verify that the API container and Redis container are both healthy enough to serve requests.

The helper target `restart-docker-compose-cluster` assumes a deployment workspace at:

- `/home/sead/supersead.humlab.umu.se`

This is a repository-specific operational assumption. If that path changes by environment, the site-specific runbook should override this document.

## CI/CD and Release Process

The active GitHub workflow in the repository is `.github/workflows/release.yml`.

What it currently does:

- triggers on pushes to `main`
- also supports manual execution through `workflow_dispatch`
- installs semantic-release and related plugins
- runs semantic-release using `GITHUB_TOKEN`

What it does not currently document as a complete operational pipeline:

- solution build and test stages before release
- image publication or registry push
- environment promotion steps
- deployment trigger from GitHub Actions into a running environment

Operationally, this means the repository has release automation, but not a fully documented CI/CD deployment pipeline. Treat build-and-release automation and deployment automation as separate concerns.

## Post-Deployment Verification

The repository does not define a formal smoke-test script for operators, so the supported verification checklist is minimal and practical.

After a deployment or restart:

1. confirm the API container is running
2. confirm the Redis container is running
3. verify the application is listening on the expected published port `8090`
4. verify the mounted config directory is present under `/app/config`
5. confirm application logs are being written and no immediate startup failures appear
6. perform a basic HTTP request against the API endpoint exposed by the deployment environment

Exact endpoint-level smoke checks are `TBD` and should be added when a stable operational verification path is defined.

## Logging, Observability, and Health

Observable runtime behavior from `conf/appsettings.Production.json` includes:

- Serilog console logging
- asynchronous sink configuration
- rolling file logging to `/var/log/sead-query-api-.log`

Operational implications:

- container stdout should be considered part of the operational log surface
- the runtime expects writable file-log access under `/var/log`
- log retention, forwarding, and alerting policy are not defined in the repository and are `TBD`

No dedicated health-check endpoint, metrics pipeline, dashboard definition, or alert routing is documented here. Those should be considered `TBD` until explicitly added to the repo or companion runbooks.

## Rollback

The repository does not contain a formal rollback automation workflow.

The practical rollback options implied by the current deployment model are:

- redeploy a previously known-good image tag
- reset `SEAD_QUERY_API_TAG` in the deployment environment to a known-good release
- rebuild and restart the Docker Compose cluster with that known-good version

Because rollback commands are environment-specific and the deployment cluster path is hard-coded in `docker/Makefile`, the exact rollback steps should be treated as site-runbook material. The high-level repository guidance is therefore:

- keep previously known-good image tags available
- roll back by version, not by manually editing running containers
- verify the service and Redis dependencies after rollback the same way as after forward deployment

## Backup and Recovery

The repository does not define a full backup or restore procedure for the SEAD database, Redis data, or configuration volumes.

What can be said from the checked-in operational files:

- Redis uses append-only persistence
- Redis data is volume-mounted from `${REDIS_DATA_DIR}`
- application configuration is mounted from a host directory into `/app/config`

The following are therefore `TBD` and should be owned by environment-specific operations documentation:

- PostgreSQL backup frequency and restore process
- Redis backup or cache-rebuild strategy
- host-path backup policy for mounted configuration
- disaster recovery objectives and recovery sequencing

## Incident Basics

For an operational incident affecting the service:

1. confirm whether the issue is API-only or also affects Redis
2. inspect container status and recent logs
3. verify the mounted configuration and environment file presence
4. confirm the expected image tag and runtime branch/release version
5. restart the compose cluster if the incident is a transient process failure
6. roll back to a known-good image if the incident correlates with a recent rollout

If the incident appears database-related, escalation and database recovery should follow the database team’s runbook, which is outside the scope of this repository and currently `TBD` here.

## Operational Gaps To Address

The repository still lacks a few operationally important pieces of documentation or automation:

- a formal CI build-and-test gate before release
- a documented image publication destination or registry policy
- explicit health-check and smoke-test procedures
- environment-specific deployment profiles
- a committed rollback runbook
- backup and restore procedures
- alerting and monitoring configuration

These gaps should remain visible in the operations guide rather than being filled with guessed process.

## Related Documents

- `docs/DEVELOPMENT.md` — local development workflow and setup
- `docs/DESIGN.md` — system structure and architecture decisions
- `docs/TESTING.md` — validation strategy and local test workflow
- `docs/DIAGRAMS.md` — visual system overview and flow diagrams
- `README.md` — repository entry point