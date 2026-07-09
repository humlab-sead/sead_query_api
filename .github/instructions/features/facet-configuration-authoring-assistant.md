---
id: facet-configuration-authoring-assistant
title: Facet Configuration Authoring Assistant
kind: ai_ml_feature_description
category: maintainer-authoring-assistance
status: candidate
audiences:
	- maintainer
	- developer
	- configuration-author
tags:
	- ai
	- ml
	- configuration
	- yaml
	- authoring
	- validation-assistance
system_surfaces:
	- route YAML
	- facet metadata
	- import validation
	- facet schema
summary: Assists maintainers when authoring or reviewing facet and route configuration before import.
---

# Feature: Facet Configuration Authoring Assistant

## Purpose

Assist maintainers while authoring or reviewing facet and route configuration by suggesting route families, anchor bindings, facet metadata completions, and likely validation issues before import.

## Why It Fits This System

The current system depends on checked-in YAML authoring in `sead.query.composer/Templates/facet_configuration.yml` plus importer-managed normalization into the `facet` schema. That creates a strong maintainer-facing opportunity for AI assistance that accelerates configuration work without bypassing validation or review.

## User Value

- reduces repetitive authoring work for new facets and route families
- catches likely configuration mistakes before import or runtime validation
- helps maintainers navigate the large SEAD schema and existing facet inventory

## Likely Inputs

- existing route and facet definitions from `sead.query.composer/Templates/facet_configuration.yml`
- imported configuration metadata and lookup tables
- schema and relationship documentation for relevant source and anchor tables
- current semantic or offline validation errors, or importer failures

## Expected Outputs

- candidate facet or route snippets for human review
- suggested anchor choices and route-family patterns
- likely validation fixes for missing tables, mismatched anchors, or invalid bindings
- review notes that explain why a suggestion matches the current configuration style

## Suggested AI/ML Shape

- retrieval over existing YAML patterns, facet metadata, and route documentation
- structured completion rather than free-form generation
- error-to-fix suggestion ranking for validation and import failures

## Guardrails

- generated configuration must still pass the repository's semantic and offline validation plus the normal import path
- suggestions should cite nearby existing patterns rather than inventing new conventions casually
- AI assistance should support maintainer review, not silently mutate authoritative configuration
- suggestions should preserve import provenance expectations (`SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT` and `SEAD_QUERY_FACET_CONFIG_IMPORTED_BY`) when proposing import steps
- suggestions should align with the canonical workflow in `docs/FACET_CONFIGURATION_GUIDE.md`

## Success Signals

- shorter time to add or update a facet definition
- lower rate of import-time configuration failures
- increased consistency between newly authored and existing route/facet patterns