# Project Rationale: SEAD Query System Overhaul

**Document Version:** 1.0  
**Date:** August 28, 2025  
**Project:** SEAD Query API Modernization  
**Branch:** query-engine-overhaul  
**Related Document:** [System Requirements Specification](./system_requirements_specification.md)

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [Current System Limitations](#2-current-system-limitations)
3. [Business Case](#3-business-case)
4. [Strategic Objectives](#4-strategic-objectives)

---

## **Architectural Proposal: A Composable Predicate Framework for Faceted Search**


This document proposes a new query generation architecture to replace the current monolithic join-based system. The existing approach, which flattens all filter criteria into a single SQL query with a complex set of joins, has proven to be brittle and difficult to extend. We propose a shift to a composable, predicate-based model. In this model, each facet is an encapsulated, self-contained logical unit (a "Predicate") that resolves to a set of primary entity identifiers (the "Anchor Keys"). A central "Query Composer" then assembles these predicate results to satisfy client requests, dramatically increasing modularity, maintainability, and query capability.

### **1. Problem Statement: Limitations of the Monolithic Query Model**

The current system dynamically constructs a single, large SQL query by merging the table paths and constraints of all active facets. This leads to several critical limitations:

*   **High Coupling:** Every facet's implementation is implicitly aware of the global table structure, making changes to one facet a risk to all others.
*   **Join Ambiguity:** The logic for determining join types (`INNER` vs. `LEFT`) based on user selections is complex and error-prone, as noted in existing source code (`QuerySetupHelper.cs`).
*   **Limited Expressiveness:** The model struggles with advanced scenarios, such as joining the same table multiple times with different aliases or handling disjunctive (`OR`) logic between complex conditions.
*   **Maintenance Overhead:** The need to create specialized database views as workarounds indicates that the abstraction is insufficient. The cognitive load required to understand the final generated query is excessively high.

## 1. Executive Summary

The SEAD Query System requires a complete architectural overhaul to address performance, maintainability, and scalability issues in the current monolithic join-based design. The new system will implement a modular CTE (Common Table Expression) + INTERSECT architecture using the "Anchor-Predicate" model.

### Key Benefits of New Design:
- **Performance**: Eliminate complex Dijkstra-based pathfinding and expensive joins
- **Maintainability**: Modular facet design with encapsulated logic
- **Scalability**: Strategy pattern enables easy addition of new facet types
- **Debuggability**: Clear SQL structure with readable CTEs
- **Flexibility**: Route-based system reduces template complexity from 400+ to manageable numbers

### Success Criteria:
- 90% reduction in query complexity
- 50% improvement in query performance
- Zero downtime migration
- Full backward compatibility during transition
- Comprehensive test coverage (>85%)

---

## 2. Current System Limitations

### 2.1 Monolithic Join Architecture Problems
- **Complex Pathfinding**: Uses Dijkstra's algorithm to find join paths between 50+ tables
- **Performance Degradation**: Complex multi-table joins with suboptimal execution plans
- **Debugging Difficulty**: Generated SQL is nearly impossible to debug manually
- **Rigid Structure**: Adding new facet types requires extensive core system changes

### 2.2 Code Maintainability Issues
- **Tight Coupling**: Facet logic embedded in central query builder
- **No Encapsulation**: Facet-specific logic scattered across multiple files
- **Limited Extensibility**: Strategy pattern not implemented
- **Testing Complexity**: Monolithic structure makes unit testing difficult

### 2.3 Template Explosion Problem
- **Scale Challenge**: 50 facets × 8 anchors = 400+ SQL templates to maintain
- **Maintenance Burden**: Each new facet type requires templates for all anchor combinations
- **Code Duplication**: Similar join logic repeated across multiple templates
- **Error Prone**: Manual template management leads to inconsistencies

---

## 3. Business Case

### 3.1 Current Pain Points

#### User Experience Issues:
- **Slow Query Response**: Complex queries can take 10+ seconds to execute
- **Limited Functionality**: Difficult to add new filter types or data dimensions
- **System Instability**: Memory issues and timeouts during peak usage
- **Poor Error Messages**: Users receive cryptic database errors

#### Development Team Challenges:
- **High Maintenance Cost**: Significant developer time spent debugging and maintaining legacy code
- **Feature Development Bottleneck**: New features require weeks of development due to system complexity
- **Knowledge Silos**: Few developers understand the complex pathfinding algorithms
- **Testing Difficulties**: Integration tests are slow and unreliable

#### Operational Concerns:
- **Database Performance**: Complex joins cause high CPU usage and lock contention
- **Scalability Limits**: System struggles with concurrent users and large datasets
- **Monitoring Blind Spots**: Difficult to identify performance bottlenecks in generated queries
- **Recovery Complexity**: System failures are difficult to diagnose and resolve

### 3.2 Cost of Inaction

#### Short-term Risks (6-12 months):
- Continued user frustration with slow query performance
- Increased support burden for system instability issues
- Developer productivity loss due to complex debugging
- Missed opportunities for new research features

#### Long-term Risks (1-3 years):
- System becomes unmaintainable as complexity grows
- Unable to scale to meet growing user base demands
- Loss of competitive advantage in archaeological data analysis
- Risk of system failure requiring emergency redesign

### 3.3 Expected Return on Investment

#### Quantifiable Benefits:
- **50% reduction in query execution time** → Improved user productivity
- **70% reduction in development time** for new features → Faster time to market
- **90% reduction in support tickets** related to query issues → Lower operational costs
- **100% improvement in system reliability** → Better user experience

#### Strategic Benefits:
- **Future-proof architecture** enables new research capabilities
- **Improved developer experience** attracts and retains talent
- **Enhanced system reputation** increases user adoption
- **Platform for innovation** enables advanced analytics features

---

## 4. Strategic Objectives

### 4.1 Technical Excellence
- **Clean Architecture**: Implement industry best practices for maintainable code
- **Performance Optimization**: Achieve sub-second response times for typical queries
- **Scalability**: Support 10x increase in concurrent users without degradation
- **Reliability**: Achieve 99.9% uptime with robust error handling

### 4.2 Development Efficiency
- **Reduced Complexity**: Simplify codebase to enable faster feature development
- **Enhanced Testability**: Implement comprehensive automated testing
- **Improved Documentation**: Provide clear technical documentation for future developers
- **Knowledge Transfer**: Reduce dependency on individual developer expertise

### 4.3 User Experience
- **Responsive Interface**: Eliminate user wait times and timeouts
- **Intuitive Functionality**: Make complex filtering operations simple and predictable
- **Reliable Results**: Ensure query results are always accurate and consistent
- **Enhanced Capabilities**: Enable new types of data exploration and analysis

### 4.4 Operational Excellence
- **Monitoring & Observability**: Implement comprehensive performance monitoring
- **Graceful Degradation**: System continues to function even under high load
- **Predictable Performance**: Consistent response times regardless of query complexity
- **Simplified Operations**: Reduce complexity of system administration and maintenance

---

**Document Control**
- **Author**: SEAD Development Team
- **Reviewers**: [To be assigned]
- **Approval**: [To be assigned]
- **Next Review**: [To be scheduled]

*This document explains why the SEAD Query System overhaul is necessary and what benefits it will deliver. For technical details, see the [System Requirements Specification](./system_requirements_specification.md).*
