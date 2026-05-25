---
applyTo: '*'
---

# Conventional Commits Instructions

Use the [Conventional Commits](https://www.conventionalcommits.org/) format for every commit message.
This keeps history readable and makes release notes, review, and automation easier.

## Main Rules

- Format the subject line as:
  ```
  <type>[optional scope]: <description>
  ```
- Keep the first line at or under 72 characters.
- Write the description in imperative mood.
- Do not capitalize the first word unless it is a proper noun or identifier.
- Do not end the subject with a period.

## Allowed Types

- `feat`: new user-facing or developer-facing functionality
- `fix`: bug fix or behavior correction
- `docs`: documentation only
- `refactor`: internal restructuring without behavior change
- `perf`: measurable performance improvement
- `test`: test-only changes
- `build`: build, package, SDK, or dependency changes
- `ci`: pipeline or automation changes
- `chore`: maintenance work that does not fit another type
- `style`: formatting-only changes with no behavior effect
- `revert`: reverts a previous commit

## Recommended Scopes For This Repo

- Use a narrow scope when it makes the change easier to understand.
- Good scopes in this repository include: `api`, `core`, `infra`, `composer`, `tests`, `docs`, `build`, `config`.
- Omit the scope when the change spans the whole solution or the scope adds no value.

- Examples:
  - `feat(composer): add arrow route graph resolution`
  - `fix(api): preserve facet filter order`
  - `test(tests): add route parser unit coverage`
  - `build: update dotnet sdk and test packages`

## Best Practices

- Use English for all commit messages.
- Keep one commit focused on one logical change.
- Put rationale, tradeoffs, or follow-up notes in the body when the subject alone is not enough.
- For breaking changes, add `!` after the type or scope and explain the break in the body.
- Reference issues or work items in the body when helpful.

## Breaking Change Example

```text
feat(core)!: rename facet route contract

BREAKING CHANGE: existing route configuration must use the new contract name.
```

---

Follow this convention for all project commits.