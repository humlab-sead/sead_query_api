---
title: AI/ML Feature Descriptions
kind: ai_ml_feature_catalog
description: Machine-readable entry point for candidate AI/ML features relevant to the SEAD Query API.
tags:
	- ai
	- ml
	- feature-catalog
	- discovery
catalog: catalog.json
---

# AI/ML Feature Descriptions

This folder captures candidate AI/ML features that fit the SEAD Query API and its surrounding authoring and analysis workflows.

These are product feature descriptions, not active runtime behavior and not Copilot instruction files.

Agent discovery notes:

- use `catalog.json` for deterministic discovery
- use frontmatter on each Markdown file for lightweight classification
- treat these files as candidate feature descriptions, not shipped behavior

Each document is intentionally short and grounded in the current system:

- faceted browsing over the SEAD PostgreSQL database
- route-based, anchor-centered query composition
- database-backed imported facet configuration
- HTTP/JSON result delivery for browser and tooling clients

Current candidate features:

- `natural-language-query-assistant.md`
- `semantic-term-normalization.md`
- `facet-recommendation-and-query-guidance.md`
- `result-summary-and-explanation.md`
- `facet-configuration-authoring-assistant.md`