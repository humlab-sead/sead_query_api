# Use Cases

## Use case: Load content for a selected target facet

**Goal:** The UI needs category values and counts for one facet, for example “Country”, while respecting the user’s current facet selections.

### Main scenario

1. **The user opens or updates a facet in the UI.**
   The client sends a “populate facet” request to the API.

2. **The API receives the request.**
   The API controller accepts the request and starts the facet-content loading flow.

3. **The request payload is reconstituted.**
   `ReconstituteConfigService` turns the incoming request into a `FacetsConfig` model.

4. **The facet load service starts loading the target facet.**
   `FacetLoadService` receives the target facet and the current facet state.

5. **Invalid or stale picks are removed.**
   `BogusPickService` cleans the facet selections so downstream services work with valid input.

6. **The normalized request is sent to `FacetContentService`.**
   This service decides how the facet content should be loaded.

7. **The system checks whether the composed path supports the request.**
   `FacetContentService` asks `ComposedFacetContentService` whether the request can use the composed query path.

8. **If the request is supported, the composed path is used.**
   `ComposedFacetContentService` becomes the orchestrator for loading the facet content.

9. **A target-facet handler is selected.**
   The service chooses a handler based on the target facet type, such as discrete, range, intersect, or geo-polygon.

10. **The composed request is built.**
    `ComposedFacetContentRequestFactory` validates the request and builds the anchor/target request contract.

11. **The composed anchor filter is built.**
    `ComposedFacetContentFilterQueryFactory` creates the `composed_filter` SQL from the active predicate facets.

12. **The target facet handler builds and executes the content query.**
    The handler uses the composed filter to load category rows and counts from PostgreSQL.

13. **PostgreSQL returns facet rows and counts.**

14. **The handler maps database rows into `FacetContent`.**

15. **The result flows back through the service chain.**
    `FacetContent` is returned from the handler to `ComposedFacetContentService`, then to `FacetContentService`, then to `FacetLoadService`, then to the API.

16. **The API returns the facet response to the UI.**

17. **The UI updates the facet.**
    The user sees refreshed categories, counts, ranges, or other facet-specific content.

### Alternative scenario: unsupported composed request

At step 7, if the request is **not supported** by the composed path:

1. `FacetContentService` falls back to the legacy facet-content path.
2. The legacy facet compiler builds the older category-count SQL.
3. PostgreSQL executes the legacy query.
4. The legacy path returns category rows and counts as `FacetContent`.
5. The API returns the response to the UI.

So the key idea is:

> `FacetContentService` decides between the **new composed path** and the **legacy fallback path**. The composed path is used only when the request satisfies the current support boundary.

## Use case: Load content for a selected target facet (query-engine overhaul path)

**Goal:** The UI needs category values and counts for one facet, while the query-engine overhaul uses explicit routes, anchor-based composition, and facet-type-specific handlers to build the response.

This version describes the composed path that the overhaul is standardizing. It is the preferred path for supported requests in the redesign.

### Main scenario

1. **The user opens or updates a facet in the UI.**
   The client sends a facet-content request to the API.

2. **The API receives the request.**
   The API controller accepts the request and starts the facet-content loading flow.

3. **The request payload is reconstituted.**
   `ReconstituteConfigService` turns the incoming request into a `FacetsConfig` model.

4. **The facet load service normalizes the request.**
   `FacetLoadService` removes invalid or stale picks and forwards the normalized request to `FacetContentService`.

5. **The composed path is selected when the request is supported.**
   `FacetContentService` asks `ComposedFacetContentService` whether the request can use the composed query path.

6. **The composed orchestrator takes over.**
   `ComposedFacetContentService` becomes the orchestrator for the supported request.

7. **A target-facet handler is selected.**
   The orchestrator chooses an `IComposedFacetContentHandler` implementation based on the target facet type, such as discrete, range, intersect, or geo-polygon.

8. **The composed request contract is built.**
   `ComposedFacetContentRequestFactory` validates the request, resolves the anchor and route contract, and prepares the target join information.

9. **The anchor filter SQL is built.**
   `ComposedFacetContentFilterQueryFactory` composes the anchor-key SQL for the active predicate facets.

10. **The handler loads the target facet content.**
    The selected handler uses the composed anchor set and the target contract to query PostgreSQL for rows and counts.

11. **PostgreSQL returns the facet rows.**
    The handler maps the database rows into `FacetContent`.

12. **The result flows back through the service chain.**
    `FacetContent` returns from the handler to `ComposedFacetContentService`, then to `FacetContentService`, then to `FacetLoadService`, then to the API.

13. **The API returns the facet response to the UI.**
    The UI updates the facet with refreshed categories, counts, ranges, or other facet-specific content.

### Alternative scenario: unsupported request

If the request falls outside the supported composed contract:

1. `FacetContentService` does not use the composed orchestrator.
2. The runtime uses the explicit fallback path or raises the supported-contract error defined by the current boundary.
3. The request does not proceed through the overhaul path as a supported composed request.

So the key idea is:

> The query-engine overhaul makes route resolution, anchor composition, and target-facet rendering explicit. Supported requests use the composed orchestrator; unsupported requests remain outside that contract.
