
Prompt:

I'm in need of doing an overhaul of this faceted browser system. The system is developed using C# dotnet and queries data in a relational database (PostgreSQL). The system exposes an API where clients can fetch data filtered out using a chain of "facets". A facet is a filter on a specific domain entity in database.

The current design has it's limitation, mostly due to the way the chain of facets are compiled to a single SQL query.

The design of the current query compiler is rather generic and can be configured to run against (almost) any relational database.

The current design is briefly this:

A instance of the system is configured with a **graph** consisting of valid paths between tables in the database. This graph is the most important configuration of the system. Each node in the graph is either a table, a view, or a UDF returning table data. Each edge defines how to join two nodes using source and target keys (most often PK and FK columns). Each edge is also given a weight that is used by the system when finding the "best" path between to nodes (i.e. when constructing joins).

Each (filter-) facet is defined using a node of origin and a number of filter criterias.  Any table (node) in the graph can be used when specifying a facet. These extra tables could for instance be used in the facet filter critera, specifying return columns/expressions in the final query, or in sorting criterias.

A facet has a "category", which is the property the facet (conceptually) is a filter for (e.g. a facet "Country" specifies both PK column and plain text column for countries in location table where location type is a country.)

There are different kinds of facets that uses different filtering operators and different kinds of categories:
      - Discrete facets which the user can choose values from a list, e.g. sample type, site type, taxon. The category is in this case an identity or the lookup name for the identity.
      - Range facets, when filtering numerical values using ranges e.g. sample depth, date ranges. The categories is definedn by splitting the entire range into sub-ranges.
      - GIS polygons (e.g. tests of site's coordinate is within a region), where the user select e.g. sites or places by drawing a polygon on a map. 

When compiling a chain of facets, the system finds, from the first facet to the last, all the unique shortest paths from the target facet's target table to all nodes (tables) occuring in any preceeding facet. The unique resulting nodes will form the "inner joins" of the resulting query.

For each facet in the chain, the number of items per category (filtered by previous facets in the chain) is also created by grouping by facet's category, and counting the number of unique items of a certain "result field" (specified in the facet).

There is also a "result facet" that, when compiled, fetches a final result set, filtered by the compiles SQL from the facet chain. This is basically an outer SQL that fetches the target items using the chain of facets, then adds any included tables needed for the reults, and an arbitaray number of result items.

A weakness in this design is that all involved facets are boiled down into a number of "inner joins", which doesn't work for more advanced querying. We often need to resort to adding additional views to the graph that shortcuts certain paths.  It's also complicated when we need to join the same table multiple times.

I want to change this design so that each facet filters data in isolation, and encapsulates its own logic. Each facet should contribute to an CTE (common table expression) in the final query, and all CTE is inner join in the returned SQL.

My idea is to define a number of "anchor" entities in the database representing basic concept that we want to count. When compiling a chain of facet's we would know "anchor type". The final join between all the CTEs would use the anchor entity as join condition effectivly filtering data to include only item related to an anchor that exists (in the result set) of all facets.

This means that facets need to know how to compile a CTE that include anchor keys for any anchor defined in the system. (One can also put a constraint that a facet can only be added to a chain of facets if it "knows" the "current" anchor key.) 
