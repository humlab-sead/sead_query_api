# SEAD Faceted Query API

[![Release workflow](https://github.com/humlab-sead/sead_query_api/actions/workflows/release.yml/badge.svg)](https://github.com/humlab-sead/sead_query_api/actions/workflows/release.yml)
[![GitHub Issues](https://img.shields.io/github/issues/humlab-sead/sead_query_api.svg)](https://github.com/humlab-sead/sead_query_api/issues)
[![GitHub Stars](https://img.shields.io/github/stars/humlab-sead/sead_query_api.svg)](https://github.com/humlab-sead/sead_query_api/stargazers)
[![License](https://img.shields.io/github/license/humlab-sead/sead_query_api.svg)](LICENSE.txt)
[![DOI](https://zenodo.org/badge/89851568.svg)](https://zenodo.org/badge/latestdoi/89851568)

SEAD Faceted Query API is a .NET 9 REST service for faceted browsing over the SEAD PostgreSQL database. It is used to reconstruct facet state, compile query logic from configured facets, and return either facet content or result sets to SEAD browser clients and other HTTP/JSON consumers.

## What This Repository Contains

- An ASP.NET Core API host in `sead.query.api`
- Shared domain, facet, and query contracts in `sead.query.core`
- Infrastructure and repository code in `sead.query.infra`
- An in-progress query-engine redesign in `sead.query.composer`
- Unit and integration tests in `sead.query.test`

## Prerequisites

- .NET SDK 9.0
- Git
- PostgreSQL access for database-backed development workflows
- Docker, if you want container-backed test scenarios or Docker-based local runs

## Quick Start

```bash
git clone https://github.com/humlab-sead/sead_query_api.git
cd sead_query_api
dotnet restore sead_query_api.sln
make build
make serve
```

Then open the local URL reported by ASP.NET Core at startup.

## API Note

| Interface     | Access                                                                                                                                                 |
|---------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| HTTP/JSON API | Run `sead.query.api` locally with `make serve` or `dotnet run --project sead.query.api/sead.query.api.csproj` and use the URL printed by ASP.NET Core. |

## Documentation

- [Architecture](docs/DESIGN.md)
- [Development](docs/DEVELOPMENT.md)
- [Testing](docs/TESTING.md)
- [Diagrams](docs/DIAGRAMS.md)
- [Requirements](docs/REQUIREMENTS.md)
- [Operations](docs/OPERATIONS.md)
- Docker guide: `TBD`
- [Agent guidance](AGENTS.md) (`.github/instructions/` contains the focused repository instruction files)

## Status

- Current runtime: existing faceted query API
- Active redesign: route-based query-engine overhaul on `query-engine-overhaul`
- Release automation: GitHub Actions release workflow in `.github/workflows/release.yml`

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## License

Licensed under the terms in [LICENSE.txt](LICENSE.txt).

## Authorship

Maintained by the SEAD development team at Humlab, Umea University.
