# Rationale: Why The Overhaul Still Matters

## Summary

The proposal is still justified because the current query pipeline remains constrained by global join discovery and facet coupling. The branch work also shows that the route-based direction is technically plausible: route parsing, route resolution, and route SQL generation have already been explored.

## Problem

The current runtime still has the same core pressure points that motivated the overhaul:

- implicit pathfinding across a shared graph
- difficult handling of repeated table use and non-trivial join semantics
- tight coupling between filtering, join discovery, facet content generation, and result generation

## Current Behavior

What has changed since the original rationale is the confidence level, not the need.

- the branch now contains route-oriented prototypes and anchor-oriented scaffolding
- the broader redesign is not integrated into compiled runtime code
- the earlier language about timelines, migration guarantees, and quantified outcomes overstated what the branch actually proves

## Final Recommendation

Keep the rationale simple: the overhaul is still needed, but the next step is not a broad modernization program. It is an incremental, compiled, testable vertical slice based on the route and anchor work already explored.
