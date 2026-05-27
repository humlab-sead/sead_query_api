# Status Assessment: Query Overhaul Branch Snapshot

## Summary

The branch contains meaningful redesign work, but not a completed redesign.

The strongest implemented direction is route-based composition. The weakest point is end-to-end integration: the core anchor-based composer is still not compiled into the active runtime.

## Implemented Or Demonstrated

- route parsing and route expansion work in `sead.query.composer/QueryComposer/RouteCompiler/`
- route graph, route resolution, and route SQL generation experiments
- matching unit-test areas for route compiler behavior
- anchor and route entity and repository scaffolding in the current working tree

## Not Implemented Or Not Integrated

- a compiled end-to-end query composer based on anchor-key predicates
- a compiled replacement for the legacy runtime path
- complete resolver contracts in `sead.query.core/QueryComposer/`
- completed facet content generation on top of the new composer path

## Deferred Or Shelved

- the broader redesign under `sead.query.composer/BackBurner/`
- tests under `sead.query.test/QueryComposer/BackBurner/`

These files still contain useful ideas, but they are not the current implementation base.

## Proposal Implications

The proposal should be treated as a focused change request, not as a near-complete migration program.

The right description of branch status is:

- route-based direction proved
- anchor-based composition partially explored
- runtime integration still pending

## Recommended Use Of This Assessment

Use this file as a branch snapshot and review note. Use `system_requirements_specification.md` as the main proposal and `implementation_and_migration_plan.md` as the implementation handoff.
