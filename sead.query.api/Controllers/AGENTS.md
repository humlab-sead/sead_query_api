# API Controllers Agent Guide

Use this guide when editing `sead.query.api/Controllers/`.

## Start Here

- Read the controller action, then step immediately to the called service.
- Check matching DTOs only when the request or response contract is part of the task.
- Use controller tests or API-hosted integration tests before widening scope.

## Controller Rules

- Keep actions focused on routing, binding, status codes, and response shaping.
- Do not add business rules, SQL assembly, or repository calls directly in controllers.
- If behavior is unclear, the owning logic is usually in `Services/`, not in the controller.

## Ignore Unless Asked

- Cross-project refactors starting from a controller
- Serializer changes unrelated to the touched endpoint
