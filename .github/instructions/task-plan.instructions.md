# Copilot Instructions: Phase Task Plans

Generate a Markdown task plan that turns a phase description into actionable implementation work. Return only the plan unless asked for explanation. Default to implementation planning, not staffing or scheduling.

## Structure

Use this structure. Skip sections that would only contain generic filler.

| # | Section | Priority | Notes |
|---|---:|---|---|
| 1 | **Phase Summary** | Essential | Phase title, goal, focus, and acceptance criteria as a checklist. Skip dates/links/branch unless provided. |
| 2 | **Execution Rules** | Essential* | Required for multi-task plans executed incrementally by an AI assistant. |
| 3 | **Work Breakdown** | Essential | 3-6 work areas (1-2 for small phases), each with objective, checklist tasks, and completion criteria. |
| 4 | **Progress Tracker** | Essential | Compact table: Area, Status (Not started/In progress/Blocked/Done), Notes. |
| 5 | **Definition Of Done** | Essential | Final checklist: acceptance criteria coverage, validation, review, follow-up capture. |
| 6 | **Validation And Testing** | Recommended | Type checks, unit tests, regression, contract tests, doc review. Use `<test-command>` placeholders unless known. |
| 7 | **Deliverables** | Recommended | Table: Deliverable, Description, Status, Link. State target file location explicitly. |
| 8 | **Scope** | Recommended | **In scope** / **Out of scope**. Prevents task drift. |
| 9 | **Risks And Mitigations** | Optional | Only for meaningful risks (legacy behavior, contract drift, stale inventory). |
| 10 | **Open Questions** | Optional | Only unresolved decisions that could block implementation. |
| 11 | **Assumptions** | Optional | Only to avoid inventing facts. Limited to sequencing, grouping, or ordering. |

## Rules

- Preserve the phase title, goal, focus, and acceptance criteria. Convert criteria into checkable outcomes.
- Every acceptance criterion must be covered by at least one work area and one Definition Of Done item.
- Use Markdown checklists for work items and Definition Of Done. Tasks must be concrete and independently checkable.
- Use `TBD` for unknown owners, dates, links, commands, or decisions. Do not invent facts, file paths, APIs, or test names.
- If info is missing, make only structural assumptions about sequencing/grouping and state them.
- Put unresolved decisions in Open Questions only if they affect implementation.
- When producing docs or artifacts, state the target file location explicitly; prefer the repo's document-placement guidance.

### Execution Rules *(for multi-task AI-assisted plans)*

- Treat each unchecked checkbox as one task. Complete only that task and the smallest supporting changes.
- Do not continue to the next checkbox after completing the current one.
- Do not widen scope into adjacent work areas unless the current checkbox requires it.
- Record discovered related work as follow-up, not as silent scope expansion.
- Prefer focused tests proving the current checkbox over broad refactors or cleanup.
- After each task, update checkbox status, progress notes, validation evidence, and deferred follow-up.

### Work Breakdown

3-6 work areas. Each: objective, checklist tasks, completion criteria ending with an observable condition. For broad checklist items, add short implementation notes (boundaries, exclusions, validation cases — not step-by-step instructions):

```md
- [ ] extend schema validation for inline SQL fields, base-anchor references, and explicit anchor-to-SQL mappings

  Implementation notes:
  - validate inline `sql` block shape, base-anchor references, and mapping shape
  - add focused valid/invalid validation tests
  - keep placeholder validation for the next checkbox
```

### Definition Of Done

Final checklist confirming acceptance criteria coverage, validation, review, and follow-up capture. For multi-task plans: confirm adjacent tasks were not silently absorbed and deferred work is recorded.

## Style

Use direct verbs: identify, document, implement, update, validate, test, review, classify, confirm. Avoid vague verbs like "look into" or "handle." Do not repeat the same item verbatim across sections. Target 1-2 screens for simple phases, 2-4 for non-trivial.
