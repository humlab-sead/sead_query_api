# QueryComposer Unit Tests Agent Guide

Use this guide when editing `sead.query.test/UnitTests/QueryComposer/`.

## Start Here

- Mirror the owning class in `sead.query.composer/QueryComposer/`.
- Start with the nearest existing test file for the same compiler, service, or route component.
- Keep assertions focused on behavior and meaningful query fragments.

## Working Rules

- Test one route, parser, graph, or compiler behavior at a time.
- Prefer asserting meaningful SQL clauses or route outcomes over full-string snapshots.
- Keep setup local unless a shared helper clearly reduces noise.
