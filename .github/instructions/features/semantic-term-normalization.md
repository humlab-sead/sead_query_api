---
id: semantic-term-normalization
title: Semantic Term Normalization And Entity Linking
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
	- entity-linking
	- synonyms
	- term-normalization
	- vocabulary
system_surfaces:
	- facet metadata
	- category labels
	- controlled vocabulary
summary: Links user language and synonyms to the controlled facet and result vocabulary used by the API.
---

# Feature: Semantic Term Normalization And Entity Linking

## Purpose

Normalize user-entered archaeological, environmental, taxonomic, chronological, and geographic language into the controlled facet and result vocabulary used by the API.

## Why It Fits This System

The system depends on explicit facet codes, category expressions, and imported configuration. Users and downstream clients often think in synonyms, historical spellings, broader concepts, or multilingual terms that do not match the exact stored labels.

## User Value

- improves recall when users search with synonyms or variant terminology
- reduces missed results caused by spelling, casing, or naming drift
- makes the faceted interface more tolerant of domain-specific vocabulary differences

## Likely Inputs

- free-text search terms or query phrases
- facet metadata and known category labels
- optional controlled vocabularies or external domain thesauri

## Expected Outputs

- normalized candidate terms
- linked facet candidates and category candidates
- broader, narrower, and equivalent-term suggestions
- warnings when a term maps to multiple incompatible interpretations

## Suggested AI/ML Shape

- embedding-based similarity between user terms and facet/category metadata
- optional dictionary-plus-model hybrid matching for high-value vocabularies
- entity linking that ranks likely facet targets rather than forcing a single silent match

## Guardrails

- do not silently rewrite high-impact filters without user visibility
- preserve exact-match options when available
- treat domain ambiguity as a first-class outcome
- keep a clear distinction between synonym expansion and authoritative stored labels

## Success Signals

- improved successful matches for synonym-rich searches
- lower rate of zero-result queries caused by vocabulary mismatch
- measurable user uptake of suggested normalized terms