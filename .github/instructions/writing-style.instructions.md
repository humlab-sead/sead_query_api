# Writing Style Instructions

Use this file when generating or editing:

* documentation
* comments and docstrings
* PR descriptions and issue text
* proposals and user-facing messages
* AI coding-agent instructions

## Standard

Write for developers, technical project members, domain experts, and AI coding agents that need accurate implementation or review context.

Prefer clear technical writing over simplification. Preserve terminology, structure, tone, and intent unless a change clearly improves readability.

Readers should understand:

* what is described
* what input is required
* what output or side effect is produced
* what rule, constraint, tradeoff, or failure case applies
* what to do next

Use concrete, behavior-first language:

* name the actual thing, action, rule, input, output, or result
  -`# Increment counter.`

## Docstrings

Start with what the function, class, or module does.

Prefer:

* `Reads sample rows from a CSV file.`
* `Returns validation errors for missing required fields.`
* `Does not write changes to the database.`

Avoid vague or overloaded wording when a more precise description is available.

Do not avoid technical terms when they are the correct project terms.

## Documentation

For guides, design notes, proposals, and reference pages:

* state the purpose before details
* preserve the document’s register and technical level
* use numbered steps for procedures
* state defaults, required fields, constraints, tradeoffs, and error cases explicitly
* keep examples, identifiers, paths, YAML, code blocks, and section order unless a small change improves readability

Do not normalize the whole document into a new writing style. Improve confusing, dense, repetitive, or vague passages in place.

## PRs and Issues

Include:

* what changed
* why it changed
* expected impact
* testing performed

Avoid vague summaries such as `improved architecture`, `enhanced functionality`, and `optimized workflow`.

Describe the actual behavior or code change instead.
