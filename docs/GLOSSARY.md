# Glossary

This glossary defines repository-specific terms that appear in the current query-composer and facet-content documentation.

## Active Route-Compiler Path

The current query-composer runtime path that builds SQL from `AnchorTemplate`, route discovery, and route compilation services in `sead.query.composer`. In this path, source-to-anchor traversal is expressed as route metadata or an explicit SQL override instead of older archived compiler code.

## Anchor Key

The identifier column for the resolved anchor entity in a composed query. Predicate builders and composed filter queries normalize their output around anchor ids so different facet predicates can be combined against the same anchor set.

## Build (in query-compose context)

In query-composer documentation, "build" means "generate SQL text for the next stage of the pipeline." It does not mean execute the SQL or return final HTTP output.

## Discrete Facet

A pick-based facet whose input is one or more selected values, optionally with an operator such as `in` or `not in`. On the composed path, discrete facet picks are converted into predicate SQL that resolves matching anchor ids.

## Predicate Facet

A facet config that constrains a composed query because it appears before the target facet in the active facet chain and has picks or enforced constraints. In the current codebase, this is a `FacetConfig2` entry collected into `PredicateConfigs`, not a separate type.

## Secondary Predicate Facet

A predicate facet used on the composed facet-content path to filter the target facet. The current factory validates these facets as discrete, routable, and able to expose a simple source key column before composing SQL.

## Predicate SQL

A SQL subquery that expresses one facet filter as data the composed pipeline can combine with other filters. For discrete facets on the current path, the predicate SQL yields normalized source-to-anchor pairs and is later merged into the composed anchor filter query.

## Source Id

The normalized column alias for the facet-side value or key emitted by a predicate query. In the discrete predicate resolver this is the selected facet value column before the composed pipeline reduces the result to matching anchors.

## Target Id

The normalized column alias for the anchor-side key emitted by a predicate query. In the discrete predicate resolver this is the anchor key that later participates in composed filter combination.
