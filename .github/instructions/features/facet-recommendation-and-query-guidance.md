---
id: facet-recommendation-and-query-guidance
title: Facet Recommendation And Query Guidance
kind: ai_ml_feature_description
category: end-user-guidance
status: candidate
audiences:
	- researcher
	- browser-user
tags:
	- ai
	- ml
	- recommendation
	- ranking
	- facets
	- query-guidance
system_surfaces:
	- facet state
	- anchor support
	- result refinement
summary: Suggests useful next facets or refinement steps based on the current query state.
---

# Feature: Facet Recommendation And Query Guidance

## Purpose

Recommend the next useful facet, filter, or result refinement step based on the current query state, available anchors, and observed narrowing patterns.

## Why It Fits This System

The SEAD Query API already exposes a rich faceted model with explicit relationships between source facets, target facets, and result views. That structure is well suited to guidance features that help users refine large result spaces without guessing which facet to try next.

## User Value

- helps users narrow large result sets efficiently
- surfaces high-value follow-up facets that are easy to miss in large UIs
- improves exploratory analysis by suggesting sensible next refinements

## Likely Inputs

- current facet state and target/result context
- facet metadata such as type, applicability, defaults, and anchor support
- optional aggregate usage patterns from historical anonymous query telemetry

## Expected Outputs

- ranked next-step facet suggestions
- rationale such as "commonly paired with your current filters" or "likely to reduce the result set materially"
- suggestions for widening or relaxing an over-constrained query

## Suggested AI/ML Shape

- learning-to-rank or heuristic ranking over candidate next facets
- query-path analysis from anonymized usage sequences
- optional bandit-style optimization for recommendation order in the UI

## Guardrails

- recommendations must stay within the valid current runtime configuration
- avoid reinforcing only the most common workflows if that hides rarer but important research paths
- do not imply scientific relevance solely from popularity

## Success Signals

- faster path from first query to a stable refined query
- increased use of deeper facet combinations
- reduced dead-end or repeatedly reset query sessions