# Copilot Cost Analysis and Token-Saving Playbook

Date: 2026-06-04
Analysis window: last 30 days (cloud session store)
Scope: interactive sessions (excluding Copilot Code Review)

## Executive Summary

Recent usage is dominated by input token overhead from long-running VS Code Chat sessions with no compaction checkpoints.

- Total input tokens: 793,952,437
- Total output tokens: 5,795,698
- Input-to-output ratio: 136.99
- Dominant agent: VS Code Chat (27 sessions, 799,748,135 total tokens)
- Compaction checkpoints found: 0 (across 32 recent sessions)

## High-Cost Patterns Observed

### 1) Long sessions with continuously growing context

Top session:
- Session: aa1d5560-ab9d-40a7-ab39-ddf8fe3410f0
- Total tokens: 290,506,631
- Turns: 171
- Duration: 2,377 minutes
- Average input tokens per usage event: 119,968
- Max input tokens in one event: 226,384

Additional high-cost sessions were in the 34M-126M range.

### 2) Premium model dominates nearly all token volume

- gpt-5.4-2026-03-05: 765,922,974 tokens
- claude-haiku-4-5-20251001: 15,164,777 tokens
- gpt-5.3-codex: 10,320,002 tokens
- gpt-5.4-mini-2026-03-17: 8,331,607 tokens

### 3) Very large user messages in expensive sessions

The largest prompts include large terminal-output style pastes (up to 60,149 characters), which likely inflated context and then kept getting re-sent in subsequent turns.

### 4) Short sessions can still be expensive when context is already bloated

Examples (<= 11 turns):
- 89bf4ba1-3bb1-478e-891e-10415e11cf00: 12,132,772 tokens (9 turns)
- c28027f5-545f-4d93-b4e8-727bb8fdd5ed: 9,641,407 tokens (9 turns)
- f621c1bd-5b71-44a7-ac6a-2be3c8d63312: 8,356,461 tokens (11 turns)

This indicates session age/context weight is a stronger driver than raw turn count alone.

## Repository Concentration

Most cost is concentrated in a few repos:

- sead_query_api: 526,612,518 tokens
- sead_shape_shifter: 248,144,824 tokens
- sead_change_control: 13,870,886 tokens
- swedeb-api: 11,119,907 tokens

## Cost-Saving Playbook

Use this as a default operating protocol for coding sessions.

### A) Session Lifecycle Rules

1. Start a fresh chat on scope change.
   - Trigger reset when changing phase, subsystem, repository, or architecture layer.
   - Do not carry one chat across multiple unrelated work packages.

2. Add compaction checkpoints on a fixed cadence.
   - Compact around turn 20-30.
   - Compact every 15-20 turns after that.
   - Compact before asking for broad refactors or test sweeps.

3. End and restart long sessions proactively.
   - If a session crosses ~90 minutes or ~40 turns, checkpoint and start fresh unless continuity is critical.

### B) Prompt Hygiene Rules

1. Never paste large raw terminal logs by default.
   - Provide a short summary plus key failing lines.
   - Reference specific files/commands rather than full dumps.

2. Replace repeated micro-steering prompts with one execution contract.
   - Instead of many "continue" or "do 1 then 2" turns, define one bounded instruction:
     - objective
     - constraints
     - files in scope
     - definition of done
     - stop condition

3. Keep context payloads intentionally small.
   - Share only the minimum snippets needed for the current decision.
   - Split broad work into narrow slices.

### C) Model Routing Rules

1. Default to lower-cost models for routine work.
   - Mechanical edits, renames, formatting, straightforward test fixes, status checks.

2. Escalate to premium models only for high-leverage tasks.
   - Deep architecture tradeoffs, complex debugging, cross-cutting refactors, ambiguous root cause work.

3. Downshift after the hard part is solved.
   - Return to lower-cost model for implementation, cleanup, and follow-up edits.

### D) Work Decomposition Rules

1. Use package-sized task framing.
   - Ask for one focused slice at a time (single feature chunk or single validation step).

2. Require concise progress summaries.
   - Ask for short delta updates and decisions made.
   - Avoid re-sending full historical context.

3. Keep expensive investigation isolated.
   - Delegate broad exploration to subagent workflows where possible, then bring back only summarized findings.

### E) Weekly Governance Checklist (10 minutes)

Run once per week:

1. Identify top 5 sessions by total tokens.
2. For each, verify:
   - Was compaction used by turn 30?
   - Were oversized pastes used?
   - Was premium model necessary for the full session?
3. Record one corrective action per session pattern.
4. Update project instructions to reduce repeated prompting overhead.

## Suggested Immediate Changes

1. Adopt a hard compaction policy for every session above 20 turns.
2. Enforce fresh-chat boundaries between phase transitions.
3. Move routine edits to lower-cost model by default.
4. Ban raw terminal dump pastes except when explicitly required.
5. Add a lightweight custom agent profile for recurring migration workflows.

## Notes

- Data source used cloud session-store event usage records.
- Token estimates are direct sums of assistant.usage event fields.
- Findings are personalized to current observed behavior and should be re-measured after 1-2 weeks of playbook usage.
