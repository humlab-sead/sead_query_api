# Copilot Instructions: Phase Plans

Generate a concise Markdown plan that sequences a larger development effort from current state to target state. A phase plan is the execution-sequencing document (broader than a task plan). For major efforts, keep it separate from the proposal; only embed a compact delivery-order section in a proposal when the work is small.

Return only the plan unless asked for explanation. Keep it lean — not a proposal, spec, or tracker. Default to implementation sequencing, not staffing or scheduling.

## Structure

Use this structure. Skip Final Recommendation if the plan already ends clearly. Skip Cross-Phase Rules for small projects.

| # | Section | Notes |
|---|---:|---|
| 1 | **Summary** | 1-3 paragraphs. Required. |
| 2 | **Problem** | Compress if obvious. |
| 3 | **Scope** | In/out boundaries. Required. |
| 4 | **Current Position** | Bullet facts only. Compress if no details provided. |
| 5 | **Phase Plan** | Core output. 3-7 phases. Required. |
| 6 | **Cross-Phase Rules** | For migration/parity/refactoring. Skip if obvious. |
| 7 | **Validation Strategy** | Compress if validation is not central. |
| 8 | **Final Recommendation** | Skip if the plan ends clearly. |

## Phase Format

```markdown
### Phase N: <Phase Title>

**Goal**

<one concise goal — concrete, not exploratory>

**Focus**

- <focus item>

**Acceptance Criteria**

- <checkable outcome>
```

Each phase is a meaningful delivery step with a clear goal, not a vague theme.

## Rules

- Base phases on current state, target state, and known gaps. Dependencies clear through ordering.
- Each stated gap must be addressed by at least one phase. Acceptance criteria must be checkable and aligned with the goal.
- Do not invent or infer: functionality, commands, paths, APIs, owners, dates, test names, or PR workflow.
- Distinguish current/proven behavior from planned. Use `TBD` only as an explicit placeholder.
- Prefer incremental delivery. For legacy replacement, use parity as a delivery measure.
- Include cutover or fallback phases when migration is involved.
- If info is missing, make only structural assumptions and state them.
- Do not repeat the same point verbatim across sections.

### Section-specific

- **Scope**: delivery-oriented. State what's in and out. Don't restate proposal rationale unless it affects phase boundaries.
- **Current Position**: short bullet list of facts only. Anchors phases, doesn't retell history.
- **Cross-Phase Rules**: operational rules that affect sequencing or boundaries (e.g., *validate one slice before grouped promotion*, *legacy is authoritative where parity is unproven*). Cut rules that don't affect sequencing.
- **Validation Strategy**: layered when relevant — unit tests, fixture comparisons, regression, legacy parity tests, CI. Don't invent commands.

## Style

Concise and direct. Avoid rationale filler. Treat the plan as a living document kept compact enough to update as phases advance. Goals should be decisive (*Reach parity for…*) not exploratory (*Think through how we might…*).
