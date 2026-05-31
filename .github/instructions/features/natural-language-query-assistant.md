---
id: natural-language-query-assistant
title: Natural-Language Query Assistant
kind: ai_ml_feature_description
category: end-user-query-assistance
status: candidate
audiences:
	- researcher
	- browser-user
	- api-client
tags:
	- ai
	- ml
	- nlp
	- intent-mapping
	- facets
	- query-assistance
system_surfaces:
	- facet metadata
	- result views
	- active imported configuration
summary: Maps free-text research questions onto valid facet selections and result-view choices.
---

# Feature: Natural-Language Query Assistant

## Purpose

Let a user describe a research question in natural language and translate it into valid facet selections, result-view choices, and optional route-aware constraints.

## Why It Fits This System

The SEAD Query API already has a structured faceted query model with explicit facet codes, anchors, and result projections. That makes it a strong target for an assistant that maps free-text intent onto a constrained query surface instead of generating unconstrained SQL.

## User Value

- reduces the learning curve for users who do not know the facet vocabulary
- helps researchers move from a domain question to a valid faceted request faster
- can expose supported filters without requiring deep UI exploration first

## Likely Inputs

- a free-text query such as "show beetle finds from Iron Age sites in northern Sweden"
- current facet state, if the user is refining an existing query
- available facet metadata, display titles, descriptions, supported anchors, and result-view options

## Expected Outputs

- a proposed facet selection set
- an explanation of which facets were chosen and why
- clarification prompts when the request is ambiguous
- a confidence score or ranked alternatives when multiple valid interpretations exist

## Suggested AI/ML Shape

- retrieval over facet metadata and route/config documentation
- intent extraction for time, geography, taxonomy, material, method, or result-type constraints
- ranking of candidate facet mappings rather than direct SQL generation

## Guardrails

- must emit structured facet choices, not raw SQL
- should stay within the currently imported active configuration
- must explain uncertainty when a term could map to several facets or values
- should fall back to clarification rather than inventing unsupported filters

## Success Signals

- higher successful-query completion for new users
- fewer abandoned sessions after the first search attempt
- high reviewer agreement between suggested and manually chosen facets