---
name: sead-architecture-design
description: "Design and review architecture changes for the SEAD Query API. Use when the task is about component boundaries, runtime flows, configuration governance, migration shape, design proposals, DESIGN.md updates, ADR-style reasoning, or phase planning for architectural work."
argument-hint: "Describe the architectural question, proposal, or design artifact you need"
---

# SEAD Architecture Design

Use this skill when the task is about architecture, design reasoning, or design artifacts for this repository rather than a narrow code fix.

## What This Skill Covers

- Explaining current runtime structure, component ownership, and flow boundaries
- Reviewing or drafting architecture proposals, design notes, and `docs/DESIGN.md` updates
- Planning architecture migrations, parity work, and legacy cutover sequencing
- Evaluating whether a change belongs in `api`, `core`, `composer`, or `infra`
- Identifying configuration, route, anchor, and validation implications of design changes

## Canonical Sources

Start with these sources in this order unless the user asks for historical context.

1. `.github/architecture-map.yaml`
2. `AGENTS.md`
3. `docs/DESIGN.md`
4. `docs/DIAGRAMS.md`
5. the nearest project-local `AGENTS.md`

Treat `docs/proposals/` as design intent, not shipped behavior, unless the task is explicitly proposal work.

For AI/ML feature ideation or candidate feature design, also use `.github/instructions/features/AGENTS.md` and the relevant feature description files in `.github/instructions/features/`.

## Core Rules

- Never describe planned or aspirational architecture as current shipped behavior.
- Prefer the current runtime baseline over archived or legacy material.
- Preserve layer boundaries: API owns transport, core owns shared contracts, composer owns query composition, infra owns repositories and infrastructure concerns.
- Keep unsupported request families explicit. Do not casually reintroduce silent legacy fallback in recommendations.
- Treat anchor-centered routing and imported runtime configuration as first-class design constraints.
- Separate decision documents from execution documents: proposal, phase plan, task plan, and DESIGN updates have different jobs.
- Never invent commands, endpoints, schemas, file paths, or component responsibilities.

## Bounded Evidence Procedure

Use the lightest evidence pass that can support a defensible design answer.

1. Read `.github/architecture-map.yaml` and the relevant repo-level guidance.
2. Read the target artifact instructions when writing a proposal, phase plan, task plan, or `docs/DESIGN.md`.
3. If the task is AI/ML feature design, read the feature-folder `AGENTS.md` and the relevant feature files before widening further.
4. If the task affects one owning layer, read one nearby implementation surface or one project-local `AGENTS.md` for that layer.
5. Stop after that bounded pass unless the user explicitly asks for a broader design audit.

For design prose tasks, do not widen into broad codebase exploration when the canonical docs already answer the question.

## Required Design Checks

When proposing an architecture change, answer these checks explicitly when relevant:

- Which project owns the new behavior?
- What current boundary or flow changes?
- Does the change widen or narrow the validated composed surface?
- Does it affect anchors, routes, or result projection handoff?
- Does it affect imported facet configuration, activation, or revision governance?
- What remains unsupported or intentionally deferred?
- What is the narrowest credible validation path?
- Which maintained artifact should record the change?

## Output Expectations

- Separate current state from proposed state.
- Call out invariants, tradeoffs, non-goals, and validation.
- Prefer compact, concrete recommendations over broad brainstorming.
- Use phase plans for sequencing and proposals for decisions.
- When uncertainty remains, record the uncertainty explicitly instead of smoothing over it.