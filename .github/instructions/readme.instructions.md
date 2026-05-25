---
description: "Use when editing README.md — project overview, entry-point links, quick start, and badge status. Not for detailed developer guides or operational runbooks."
applyTo: "README.md"
---
# README Instructions

## Purpose

- Use this instruction when editing `README.md`.
- Keep `README.md` focused on what the project is, why it exists, who it is for, and how to get started quickly.
- Write for developers and contributors who have just found the repository and need to understand its purpose and navigate to the right documentation.
- Treat `README.md` as a **front door**, not a manual. It should orient rather than document.

## What belongs in `README.md`

- One-paragraph project description: what the service does, who uses it, and what protocol it implements
- Status badges (CI build, license, release)
- High-level feature summary — short bullets, no implementation detail
- Quick start section showing the minimum commands to run the service locally
- Prerequisites (terse: tool names and version requirements only)
- Brief links to the primary documentation files (`docs/DESIGN.md`, `docs/DEVELOPMENT.md`, `docs/OPERATIONS.md`, `docs/TESTING.md`, `docs/DIAGRAMS.md`, `AGENTS.md`, and any checked-in Docker guide when present)
- One-line API access table or brief note on how to call the local HTTP API
- License and authorship

## What does not belong in `README.md`

- Detailed local setup walkthrough — that belongs in `docs/DEVELOPMENT.md`
- Code quality, linting, and formatting guidance — belongs in `docs/DEVELOPMENT.md`
- Full test commands and test marker explanations — belongs in `docs/TESTING.md`
- Production deployment procedures, Docker Compose options, and environment variable catalogs — belongs in `docs/OPERATIONS.md` and any checked-in Docker-specific docs
- Architecture deep-dives, component descriptions, key flows — belongs in `docs/DESIGN.md` and `docs/DIAGRAMS.md`
- Detailed code examples or configuration walk-throughs — belongs in `AGENTS.md` or `docs/DEVELOPMENT.md`
- Large API response JSON examples — belongs in generated API docs or a dedicated API reference
- "Common Gotchas" lists — belongs in `docs/DEVELOPMENT.md` or `docs/TESTING.md`
- Redundant table of contents when headings are already short and scannable

## Scope boundaries

- `README.md`: project identity, quick start, and navigation hub
- `docs/DEVELOPMENT.md`: contributor workflow, local setup, commands, conventions
- `docs/DESIGN.md`: architecture, component responsibilities, design decisions
- `docs/TESTING.md`: test strategy, levels, test selection, and validation before merge
- `docs/OPERATIONS.md`: environments, deployment, CI/CD, rollback, observability
- `docs/DIAGRAMS.md`: system diagrams (context, components, flows, state machines)
- `AGENTS.md`: AI coding agent instructions and canonical patterns
- checked-in Docker docs: container build and deployment details when present

## Writing rules

- No emoji in headings. Headings should be plain text.
- No table of contents unless the document is genuinely long (≥ 8 sections).
- Avoid repeating content that is already in a linked document — write a single sentence and link instead.
- Keep the quick start to ≤ 5 commands plus one URL.
- Prerequisites should be one line per tool: name, version, and link or install note.
- Use plain language. Avoid marketing copy.
- Do not describe the project as "powerful", "flexible", "robust", or similar adjectives without substance.
- Do not include architectural code examples (decorators, class definitions); architecture belongs in `AGENTS.md` and `docs/DESIGN.md`.
- Every section should answer a real question a new visitor has. If it does not, cut it.

## Concision and size expectations

- Target length: 100–250 lines.
- Stay under 300 lines.
- If additional content is genuinely needed, link to the appropriate `docs/` file instead of expanding the README.

## Sources to trust

- `Makefile` — canonical run and install commands
- `.github/workflows/` — badge URLs, CI status
- `docs/` — authoritative documentation for each topic
- `AGENTS.md` — canonical architectural patterns and project conventions
