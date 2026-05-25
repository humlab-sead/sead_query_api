---
description: "Use when asked to create a GitHub issue and commit changes, prepare commit messages, or document a change for issue plus commit workflow."
---
# GitHub Workflow

- Use the fast path by default: check status once, understand the scoped files, and avoid extra repo scans unless the change is unclear.
- Stage only files relevant to the requested task; never use `git add .`.
- Leave unrelated user changes unstaged and mention them in the handoff.
- Prefer one atomic Conventional Commit unless the user explicitly asks to split commits.
- Use commit format `<type>(<scope>): <description>` with imperative mood, lowercase description, no trailing period, and subject under 72 characters.
- Include `Closes #<issue>` or `Fixes #<issue>` when the commit should close the related issue.
- For issue creation, keep the body concise with three sections: `Problem`, `Solution`, and `Files`.
- If `gh issue create` is needed, prefer `--body-file - <<'EOF'` to avoid shell substitution problems.
