# QueryComposer Scenario Tests Agent Guide

Use this guide when editing `sead.query.test/QueryComposer/`.

## Start Here

- Use this folder for broader composer scenarios that span multiple route or query-building components.
- Start with the nearest existing scenario file before creating a new one.
- Check `Strategies/` only when the behavior is explicitly strategy-specific.

## Working Rules

- Keep these tests focused on composed-query behavior, not generic API or repository wiring.
- Assert meaningful query structure and route outcomes rather than incidental formatting.
- If a failing behavior can be proven in `UnitTests/QueryComposer/`, prefer that cheaper slice first.
