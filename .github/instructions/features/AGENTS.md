# AI/ML Feature Folder Guide

Use this guide when working inside `.github/instructions/features/`.

This folder contains candidate AI/ML feature descriptions for the SEAD Query API and local guidance for agents doing AI/ML feature design work.

## Purpose

- Use these files to explore feature ideas that fit the current SEAD Query API architecture.
- Treat each feature file as design-intent guidance for one candidate capability.
- Use the folder to support proposal writing, feature refinement, catalog maintenance, and design review.

## Start Here

- Start with `catalog.json` when you need deterministic discovery of candidate features.
- Read `README.md` for folder scope and discovery notes.
- Open only the feature files that match the user request or the shortlisted feature set.
- When the task is architectural, also use `.github/architecture-map.yaml`, `AGENTS.md`, and `.github/skills/sead-architecture-design/SKILL.md`.

## What These Files Mean

- These files describe candidate product capabilities, not shipped runtime behavior.
- The current system shape still constrains the design space: faceted browsing, route-based composition, anchor-centered filtering, and imported active configuration remain authoritative.
- A feature file should help an agent reason about fit, inputs, outputs, guardrails, and likely implementation shape without pretending the feature already exists.

## Operating Rules

- Do not describe a candidate feature as implemented unless another authoritative source says it is shipped.
- Keep recommendations grounded in the current structured query model; do not bypass it with unconstrained SQL generation.
- Prefer retrieval, ranking, structured completion, or explanation over free-form actions that would sidestep facet, route, or result contracts.
- Treat ambiguity as a first-class outcome. Clarification and ranked alternatives are usually better than silent guesses.
- Preserve explicit guardrails when editing or adding feature files.
- Keep feature descriptions short, concrete, and decision-useful.

## Feature Authoring Shape

Use the existing document pattern unless there is a reason to deviate:

- Purpose
- Why It Fits This System
- User Value
- Likely Inputs
- Expected Outputs
- Suggested AI/ML Shape
- Guardrails
- Success Signals

## Useful Agent Behaviors

- When asked for AI/ML feature ideas, shortlist from `catalog.json` first and then read the matching files.
- When asked to refine one feature, stay close to that file and adjacent architectural sources instead of reopening the whole repo.
- When asked for implementation planning, convert the feature description into a proposal, phase plan, or task plan rather than inflating the feature file itself.
- When asked to compare features, distinguish end-user query assistance, end-user interpretation, and maintainer authoring assistance.

## Validation Expectations

- Check that proposed inputs and outputs map to current system surfaces already named in the repo.
- Check that guardrails align with current runtime constraints and data authority boundaries.
- Check that success signals are observable and not just aspirational.

## Avoid

- broad architectural claims without checking the current design docs
- speculative model, vendor, or infrastructure choices when the feature description does not require them
- duplicating full proposal, phase-plan, or implementation-spec content inside a feature file