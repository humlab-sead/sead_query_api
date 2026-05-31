# Status Assessment: Query Overhaul Branch Snapshot

## Summary

The branch contains meaningful redesign work and a broad proven facet-content slice, but not a completed redesign.

The strongest implemented direction is route-based composition carried through a compiled facet-content path across the current active discrete and non-discrete catalog. The weakest point is still result-set generation, but it is no longer a pure legacy-only area: the overhaul now has an explicit result-projection handoff seam and an initial composed runtime slice with legacy fallback.

## Implemented Or Demonstrated

- route parsing and route expansion work in `sead.query.composer/QueryComposer/RouteCompiler/`
- route graph, route resolution, and route SQL generation experiments
- matching unit-test areas for route compiler behavior
- anchor and route entity and repository scaffolding in the current working tree
- compiled core query-composer contracts for one active slice
- composed filter query generation for a single-anchor predicate chain
- composed facet content generation for the supported target-facet paths in the active discrete, range, intersect, and geo-polygon catalog
- runtime integration into `FacetContentService` for the supported composed facet-content surface
- live comparison coverage against the legacy runtime, including grouped regression for the supported facet-content matrix

## Not Implemented Or Not Integrated

- full result-set generation on top of the new composer path
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
- anchor-based composition compiled and integrated for the active facet-content surface
- Phase 3 facet-content widening closed for the current catalog
- Phase 4 result-set parity now active, with an explicit handoff builder and focused unit validation in place
- full migration still pending

## Recommended Use Of This Assessment

Use this file as a branch snapshot and review note. Use `QUERY_ENGINE_OVERHAL.md` as the main change request, `TASK_PLAN_PHASE_3.md` and `TASK_PLAN_PHASE_4.md` as the current execution trackers, and `docs/REQUIREMENTS.md` plus `docs/DESIGN.md` as the durable system documents.
