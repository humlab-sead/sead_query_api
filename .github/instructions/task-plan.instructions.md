# Copilot Instructions: Phase Task Plans

When asked to create a task plan for a development phase, generate a Markdown document that turns the phase description into actionable implementation work.

This file is the canonical instruction for phase task plans. Avoid duplicating overlapping guidance in other instruction files.

## Output

Return only the Markdown task plan unless the user asks for explanation.

Prefer lean plans. Include optional sections only when they add useful guidance. Do not create empty or repetitive sections just to satisfy a template.

Default to implementation planning, not staffing, scheduling, or release management, unless the user explicitly asks for those dimensions.

## Section Priority

Use this priority order when deciding what to include:

1. **Essential:** Include unless the user explicitly asks for a very short plan.
2. **Recommended:** Include when the phase is non-trivial or has multiple work areas.
3. **Optional:** Include only when relevant; otherwise skip.

## Default Structure

1. Phase Summary — Essential
2. Execution Rules — Essential for multi-task implementation plans
3. Work Breakdown — Essential
4. Progress Tracker — Essential
5. Definition Of Done — Essential
6. Validation And Testing — Recommended
7. Deliverables — Recommended
8. Scope — Recommended
9. Risks And Mitigations — Optional
10. Open Questions — Optional
11. Assumptions — Optional

## Rules

- Preserve the phase title, goal, focus areas, and acceptance criteria.
- Convert acceptance criteria into checkable outcomes.
- Ensure every acceptance criterion is covered by at least one work area and one Definition Of Done item.
- If the phase changes behavior, contracts, data, migrations, or APIs, include at least one validation activity that covers the affected acceptance criteria.
- Make tasks concrete, implementation-oriented, and independently checkable.
- Use Markdown checklists for work items and Definition Of Done.
- Use the Progress Tracker table for status. Do not duplicate the same status in multiple formats unless it adds new information.
- Use `TBD` for unknown owners, dates, links, commands, decisions, and repository-specific details.
- Do not invent project facts, file paths, commands, APIs, or test names.
- If information is missing but the goal is clear, make only structural assumptions about sequencing, grouping, or ordering of work, and record them explicitly.
- Do not infer owners, dates, dependencies, PR workflow, file paths, commands, APIs, or test names.
- When the phase produces documentation, inventories, or other maintained artifacts, state the target document or file location explicitly.
- Prefer the repository's document-placement guidance over ad hoc storage locations.
- Put unresolved decisions in **Open Questions** only if they affect implementation.
- Skip sections that would only contain generic filler.

## Section Guidance

### Phase Summary — Essential

Include phase title, goal, focus, and acceptance criteria as a checklist. Include status only when useful. Include owner only if provided; otherwise omit it or mark `TBD`. Skip dates, links, branch, or PR fields unless provided or useful.

### Execution Rules — Essential For Multi-Task Plans

Include this section when the phase has multiple checklist tasks or will be executed incrementally by an AI coding assistant.

Use concise global rules such as:

- Treat each unchecked checkbox as one implementation task unless the task explicitly says otherwise.
- Complete only the named checkbox and the smallest supporting changes needed to make it valid.
- Do not continue into the next unchecked checkbox after completing the current task.
- Do not widen scope into adjacent work areas unless the current checkbox explicitly requires it.
- If related work is discovered, record it as a follow-up rather than silently expanding the current task.
- Prefer focused tests that prove the current checkbox over broad refactors, broad migrations, or unrelated cleanup.
- At the end of each task, update checkbox status, progress notes, validation evidence, and deferred follow-up.

### Work Breakdown — Essential

Create 3-6 work areas based on the phase focus. For each area include objective, checklist tasks, and completion criteria. End each area with an observable completion condition. Prefer tasks that can reasonably be implemented and reviewed independently. For very small phases, use 1-2 work areas.

For broad or easy-to-misread checklist items, add short implementation notes under the task.

Use implementation notes to define boundaries, required validation cases, exclusions, or follow-up handling. Do not use them for detailed step-by-step coding instructions.

Example:

```md
- [ ] extend schema validation for inline SQL fields, base-anchor references, and explicit anchor-to-SQL mappings

  Implementation notes:
  - validate the inline `sql` authoring block shape
  - validate base-anchor references
  - validate explicit anchor-to-SQL mapping shape
  - add focused valid and invalid validation tests
  - keep placeholder validation for the next checkbox
```

### Progress Tracker — Essential

Use a compact table:

| Area | Status | Notes |
|---|---|---|
| TBD | Not started | TBD |

Use simple statuses: Not started, In progress, Blocked, Done.

Keep this section compact. It should summarize area status, not repeat the full task list.

### Definition Of Done — Essential

Use a final checklist that confirms acceptance criteria coverage, validation, review, and follow-up capture.

For multi-task plans, include confirmation that adjacent tasks were not silently absorbed into completed work and that deferred work is recorded explicitly.

### Validation And Testing — Recommended

Include when the phase changes code, contracts, data, migrations, APIs, or behavior. Prefer repository-specific checks only when they are explicitly known from the user request or workspace context. Otherwise use placeholders such as `<test-command>`. List required checks such as type checks, unit tests, regression tests, contract tests, documentation review, or inventory review.

### Deliverables — Recommended

Include when the phase has concrete outputs such as code, docs, inventories, tests, migration scripts, or review artifacts.

For documentation or inventory deliverables, include the target file or document family in the description or link column. If the target location is still undecided and that decision affects implementation, keep it as an explicit open question.

| Deliverable | Description | Status | Link |
|---|---|---|---|
| TBD | TBD | Not started | TBD |

### Scope — Recommended

Include when boundaries matter. Use **In scope** and **Out of scope**. Skip for simple phases where scope is obvious from the work breakdown.

Use this section to prevent task drift when the phase has adjacent validation, persistence, runtime, compiler, migration, or documentation work.

### Risks And Mitigations — Optional

Include only when there are meaningful risks, such as undocumented legacy behavior, contract drift, stale inventory, ambiguous unsupported behavior, task-boundary drift, uncontrolled migration scope, or insufficient regression coverage.

### Open Questions — Optional

Include only for unresolved decisions that could block or redirect implementation. Do not list questions whose answers are obvious from the phase description.

### Assumptions — Optional

Include only when assumptions are needed to avoid inventing facts. Keep them few, explicit, and limited to sequencing, grouping, or ordering of work.

## Style

Use direct task verbs: identify, document, implement, update, validate, test, review, classify, confirm.

Avoid vague tasks like "look into", "handle", or "think about".

Do not repeat the same item verbatim across Work Breakdown, Progress Tracker, Deliverables, and Definition Of Done.

Target concise plans: roughly 1-2 screens for simple phases and 2-4 screens for non-trivial phases.
