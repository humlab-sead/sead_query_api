---
description: "Use when writing or updating design proposals. Covers style, structure, naming, and Copilot workflow for proposals."
applyTo: "docs/proposals/**"
---
# Proposal Writing Guide

Use this guide when writing or updating design proposals for this repository.

## Style

- Be clear, succinct, and matter-of-fact.
- Write problem-first, not background-first.
- Follow KISS: prefer simple explanations and simple wording.
- Follow DRY in prose: do not restate the same point in multiple sections.
- Use direct, concrete language.
- Keep sentences short.
- Use short sections with clear headings.
- Avoid fluff, hype, repetition, and vague statements.
- Include only relevant detail.
- Every sentence should add information.
- Stop when the decision is clear; do not expand a proposal into general documentation.

## Default Standard

- A proposal should be as short as possible while still being precise.
- Prefer focused documents over broad ones.
- Explain the problem, the recommendation, and the tradeoffs before implementation detail.
- Include enough technical detail to support the decision, not enough to replace the implementation work.
- If a section does not help the reader decide, cut it.
- For major efforts, keep the proposal separate from the phase plan by default. The proposal is the decision document; the phase plan is the execution-sequencing document.

## What A Proposal Should Do

- State the problem precisely.
- Explain why the problem matters now.
- Define scope and non-goals.
- Recommend a concrete path forward.
- Call out tradeoffs, risks, and open questions.
- Make implementation and validation expectations clear enough for follow-up work.

## What A Proposal Should Not Become

- Do not turn a proposal into a full implementation spec unless that level of detail is needed.
- Do not turn a proposal into a full phase plan by default. Use a separate phase plan when the work has multiple phases, migration sequencing, parity tracking, or ongoing execution updates.
- Do not pad the document with background that does not affect the decision.
- Do not mix multiple unrelated decisions into one proposal.
- Do not hide the recommendation behind neutral brainstorming.
- Do not add sections just because a template has them.
- Do not explain obvious context the intended readers already know.

## Structure

Use the template in [docs/templates/PROPOSAL_TEMPLATE.md](../../docs/templates/PROPOSAL_TEMPLATE.md) by default.

Not every section must be used. The default expectation is that proposals are organized around a small core:

- Summary
- Problem
- Scope
- Proposed design
- Tradeoffs and risks
- Validation and acceptance criteria
- Final recommendation

Add optional sections only when they earn their place, for example:

- Current behavior
- Alternatives considered
- Open questions
- Delivery order
- Implementation handoff

The default shape should feel lean, not exhaustive.

For smaller efforts, a short `Delivery order` or `Implementation handoff` section inside the proposal may be enough. For major efforts, keep those sections compact and link to a separate phase plan instead of embedding a full execution document.

## Naming

- Use filenames that describe the actual scope of the proposal.
- Prefer specific names over generic ones.
- Avoid naming a proposal after an implementation detail if the proposal is really about a broader product or workflow change.

## Working With Copilot

- If you want strict adherence, explicitly reference this guide and the proposal template when asking for a new proposal.
- If proposal-writing rules are also captured in repo instructions, you do not need to repeat them every time.
- **Keep proposal work focused.** Proposal writing is primarily prose; avoid unnecessary codebase exploration unless the request depends on specific files or symbols.
- **Keep proposal and execution artifacts distinct.** If the user asks for an ordered multi-phase delivery path, create or update a separate phase plan unless the effort is small enough that a compact delivery-order section is clearly sufficient.
- **Do not search the codebase** unless a specific file or symbol is directly referenced in the request. Write from the brief and from context already in the conversation.
- **Draft in a single pass.** Do not iterate section by section across multiple turns. Ask the user one clarifying question if needed, then produce the full draft.

## Practical Rule Of Thumb

When in doubt, optimize for:

- high problem-focused precision
- simple wording
- low repetition
- minimal but sufficient structure
- a clear recommendation
