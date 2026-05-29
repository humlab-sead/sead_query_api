# Status Assessment: Query Overhaul Branch Snapshot

## Summary

The branch contains meaningful redesign work and one proven runtime slice, but not a completed redesign.

The strongest implemented direction is route-based composition carried through a compiled facet-content path. The weakest points are breadth and completion: result-set generation is not yet on the new composer, and the overhaul is still widening through adjacent discrete and range targets.

## Implemented Or Demonstrated

- route parsing and route expansion work in `sead.query.composer/QueryComposer/RouteCompiler/`
- route graph, route resolution, and route SQL generation experiments
- matching unit-test areas for route compiler behavior
- anchor and route entity and repository scaffolding in the current working tree
- compiled core query-composer contracts for one active slice
- composed filter query generation for a single-anchor predicate chain
- composed facet content generation for a supported target-facet path
- runtime integration into `FacetContentService` for the supported composed slice
- live comparison coverage against the legacy runtime, plus widening across initial range and adjacent discrete targets

## Not Implemented Or Not Integrated

- full facet-family coverage on the composed runtime path
- final result-set generation on top of the new composer path
- a complete replacement for the legacy runtime path
- final route-definition governance and cutover rules

## Deferred Or Shelved

- the broader redesign under `sead.query.composer/BackBurner/`
- tests under `sead.query.test/QueryComposer/BackBurner/`

These files still contain useful ideas, but they are not the current implementation base.

## Proposal Implications

The proposal should be treated as a focused change request, not as a near-complete migration program.

The right description of branch status is:

- route-based direction proved
- anchor-based composition compiled and integrated for one runtime slice
- widening in progress
- full migration still pending

## Recommended Use Of This Assessment

Use this file as a branch snapshot and review note. Use `QUERY_ENGINE_OVERHAL.md` as the main change request, `implementation_and_migration_plan.md` as the implementation handoff, and `docs/REQUIREMENTS.md` plus `docs/DESIGN.md` as the durable system documents.
