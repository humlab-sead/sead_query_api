# Problem Statement: Why The Current Query Model Must Change

## Summary

The SEAD faceted query pipeline is difficult to extend because it compiles a chain of facets into one global join shape. The desired change is to make each facet responsible for its own filtering logic and compose facets through shared anchor keys instead of implicit graph pathfinding.

## Problem

The current design has four practical limits.

- Join discovery is global and implicit.
- Reusing the same table through different semantic paths is hard.
- Advanced facet logic leaks into shared query-builder behavior.
- Facet content generation and final result generation depend on the same brittle join model.

These limits create workarounds, especially extra views and graph shortcuts, instead of letting the facet model express the behavior directly.

## Current Behavior

Today the system:

- configures valid paths between tables in a graph
- uses graph traversal to derive joins across the active facet chain
- builds facet content and final result queries on top of that shared join structure

That model works for simpler cases, but it does not scale well to repeated joins, explicit route semantics, or isolated facet behavior.

## Final Recommendation

The problem to solve is not generic SQL generation. It is the lack of an explicit, anchor-aware facet composition contract. The proposal should therefore stay focused on anchor keys, route definitions, isolated facet predicates, and composed filtering.
