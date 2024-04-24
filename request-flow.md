
### Control Flow 📌🚩👀🦴🦷👁️✨🔔📀💡🔦🔖🏷️✂️⏳📍

#### Load facet

1. Reconstitute facets config (IReconstituteConfigService JSON => FacetsConfig).
2. Load facet content (IFacetLoadService: FacetsConfig => FacetContent)
   1. Remove bogus picks (BogusPickService: FacetsConfig => FacetsConfig)
   2. 📍Locate load content service (TargetFacet.FacetType => IFacetContentService)
   3. Load facet content (IFacetContentService: FacetsConfig => Core.FacetContent)
      1. Compile interval query
         1. Discrete facet:
            1. querySetup = QuerySetupBuilder.Build(facetsConfig, facetsConfig.TargetFacet, null, null);
            2. Interval query = IDiscreteContentSqlCompiler.Compile(querySetup, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter())
            ```sql
                SELECT cast({facet.CategoryIdExpr} AS varchar) AS category, {facet.CategoryNameExpr} AS name
                FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                 {query.Joins.Combine("")}
                WHERE 1 = 1
              {"AND ".GlueTo(CategoryNameLike(facet, categoryNameFilter)) }
              {"AND ".GlueTo(query.Criterias.Combine(" AND "))}"
                GROUP BY 1, 2
                {SortBy(facet)}";
            ```
         2. Range facet:
            ```sql
                SELECT n::text || ' => ' || (n + {interval})::text, n, n + {interval}
                FROM generate_series({min}, {max}, {interval}) as a(n)
                WHERE n < {max}
            ```
      2. Fetch category counts
      3. Fetch outer category counts
      4. Collect user selections/picks
      5. Compile facet content result



FIXME: IIntervalSqlCompiler saknas (olika interface)
📍logic split based on facet type

Category counts: results of compiled interval query
Outer category counts: results of compiled interval query (no grouping or additional fields)

### Terminology

| Concept              | Description                       |
| -------------------- | --------------------------------- |
| Filter facet         | A filter specification.           |
| Target facet         | A requests main target facet      |
| Aggregate facet      |                                   |
| View type            |                                   |
| Result specification | A specification of result fields. |

### The request's result configuration data

| Item(s)            | Description                           | Default            | Impacts | Entity               |
| ------------------ | ------------------------------------- | ------------------ | ------- | -------------------- |
| Result facet code  | Specifies aggregate facet             | `result_facet`     |         | facet                |
| Result view type   | Specifies if result is tabular or map | `map` or `tabular` |         | result_view_type     |
| Specification keys | A specification of result fields.     | (null)             |         | result_specification |

Notes:

- Fi

1. The request specifies the **result facet** (most often `result_facet`).
   1.
2. The request contains a `view type` (e.g. `map` or `tabular`) specifying the format of the resulting data.
   1. The `view type` specifies if the client wabt
3. The request also contains an optional set of **specification keys**
   1.

### URI general format

- [domain-facet://]target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])\*

### country (no picks)

Trigger: country add

```json
{
  "requestId": 1,
  "requestType": "populate",
  "targetCode": "country",
  "domainCode": "",
  "facetConfigs": [
    { "facetCode": "country", "position": 1, "picks": [], "textFilter": "" }
  ]
}
```

### country@205

Trigger: country pick value

```json
{
  "facetsConfig": {
    "requestId": 10,
    "requestType": "populate",
    "domainCode": "",
    "targetCode": "country",
    "triggerCode": "country",
    "facetConfigs": [
      {
        "facetCode": "country",
        "position": 1,
        "picks": [{ "pickType": 1, "pickValue": 205, "text": 205 }],
        "textFilter": ""
      }
    ]
  },
  "resultConfig": {
    "requestId": 10,
    "sessionId": "1",
    "viewTypeId": "map",
    "aggregateKeys": ["site_level"]
  }
}
```

### country@205 + sites

Trigger: site add

```json
{
  "requestId": 1,
  "requestType": "populate",
  "targetCode": "sites",
  "triggerCode": "country",
  "domainCode": "",
  "facetConfigs": [
    {
      "facetCode": "country",
      "position": 1,
      "picks": [{ "pickType": 1, "pickValue": 205, "text": 205 }],
      "textFilter": ""
    },
    { "facetCode": "sites", "position": 2, "picks": [], "textFilter": "" }
  ]
}
```

### country@205 + sites@3826

Trigger: site pick value

```json
{
  "facetsConfig": {
    "requestId": 11,
    "requestType": "populate",
    "domainCode": "",
    "targetCode": "sites",
    "triggerCode": "sites",
    "facetConfigs": [
      {
        "facetCode": "country",
        "position": 1,
        "picks": [{ "pickType": 1, "pickValue": 205, "text": 205 }],
        "textFilter": ""
      },
      {
        "facetCode": "sites",
        "position": 2,
        "picks": [{ "pickType": 1, "pickValue": 3826, "text": 3826 }],
        "textFilter": ""
      }
    ]
  },
  "resultConfig": {
    "requestId": 11,
    "sessionId": "1",
    "viewTypeId": "map",
    "aggregateKeys": ["site_level"]
  }
}
```

### country@205,_224_ + sites@3826

Trigger (chain): country pick value 224

Facet load [sites]

Should triggerCode actually be country in this case???
TODO: Check usage of triggerCode

```json
{
  "requestId": 2,
  "requestType": "populate",
  "targetCode": "sites",
  "triggerCode": "sites",
  "domainCode": "",
  "facetConfigs": [
    {
      "facetCode": "country",
      "position": 1,
      "picks": [
        { "pickType": 1, "pickValue": 205, "text": 205 },
        { "pickType": 1, "pickValue": 224, "text": 224 }
      ],
      "textFilter": ""
    },
    {
      "facetCode": "sites",
      "position": 2,
      "picks": [{ "pickType": 1, "pickValue": 3826, "text": 3826 }],
      "textFilter": ""
    }
  ]
}
```

Result load

```json
{
  "facetsConfig": {
    "requestId": 12,
    "requestType": "populate",
    "domainCode": "",
    "targetCode": "sites",
    "triggerCode": "sites",
    "facetConfigs": [
      {
        "facetCode": "country",
        "position": 1,
        "picks": [
          { "pickType": 1, "pickValue": 205, "text": 205 },
          { "pickType": 1, "pickValue": 224, "text": 224 }
        ],
        "textFilter": ""
      },
      {
        "facetCode": "sites",
        "position": 2,
        "picks": [{ "pickType": 1, "pickValue": 3826, "text": 3826 }],
        "textFilter": ""
      }
    ]
  },
  "resultConfig": {
    "requestId": 12,
    "sessionId": "1",
    "viewTypeId": "map",
    "aggregateKeys": ["site_level"]
  }
}
```

### country@205,224 + sites@3826,_3830_

Trigger: site@pick 3830

Facet load [sites]

```json
{
  "facetsConfig": {
    "requestId": 13,
    "requestType": "populate",
    "domainCode": "",
    "targetCode": "sites",
    "triggerCode": "sites",
    "facetConfigs": [
      {
        "facetCode": "country",
        "position": 1,
        "picks": [
          { "pickType": 1, "pickValue": 205, "text": 205 },
          { "pickType": 1, "pickValue": 224, "text": 224 }
        ],
        "textFilter": ""
      },
      {
        "facetCode": "sites",
        "position": 2,
        "picks": [
          { "pickType": 1, "pickValue": 3826, "text": 3826 },
          { "pickType": 1, "pickValue": 3830, "text": 3830 }
        ],
        "textFilter": ""
      }
    ]
  },
  "resultConfig": {
    "requestId": 13,
    "sessionId": "1",
    "viewTypeId": "map",
    "aggregateKeys": ["site_level"]
  }
}
```

### country@205,_224_ + genus@12974,12793,12932 + sites

Trigger: country@pick 224

Facet load #1 [sites]

```json
{
  "requestId": 8,
  "requestType": "populate",
  "targetCode": "sites",
  "triggerCode": "genus",
  "domainCode": "",
  "facetConfigs": [
    {
      "facetCode": "country",
      "position": 1,
      "picks": [
        { "pickType": 1, "pickValue": 205, "text": 205 },
        { "pickType": 1, "pickValue": 224, "text": 224 }
      ],
      "textFilter": ""
    },
    {
      "facetCode": "genus",
      "position": 2,
      "picks": [
        { "pickType": 1, "pickValue": 12974, "text": 12974 },
        { "pickType": 1, "pickValue": 12793, "text": 12793 },
        { "pickType": 1, "pickValue": 12932, "text": 12932 }
      ],
      "textFilter": ""
    },
    { "facetCode": "sites", "position": 3, "picks": [], "textFilter": "" }
  ]
}
```

Facet load #2 [genus]

```json
{
  "requestId": 4,
  "requestType": "populate",
  "targetCode": "genus",
  "triggerCode": "genus",
  "domainCode": "",
  "facetConfigs": [
    {
      "facetCode": "country",
      "position": 1,
      "picks": [
        { "pickType": 1, "pickValue": 205, "text": 205 },
        { "pickType": 1, "pickValue": 224, "text": 224 }
      ],
      "textFilter": ""
    },
    {
      "facetCode": "genus",
      "position": 2,
      "picks": [
        { "pickType": 1, "pickValue": 12974, "text": 12974 },
        { "pickType": 1, "pickValue": 12793, "text": 12793 },
        { "pickType": 1, "pickValue": 12932, "text": 12932 }
      ],
      "textFilter": ""
    },
    { "facetCode": "sites", "position": 3, "picks": [], "textFilter": "" }
  ]
}
```

Result load

```json
{
  "facetsConfig": {
    "requestId": 23,
    "requestType": "populate",
    "domainCode": "",
    "targetCode": "sites",
    "triggerCode": "sites",
    "facetConfigs": [
      {
        "facetCode": "country",
        "position": 1,
        "picks": [
          { "pickType": 1, "pickValue": 205, "text": 205 },
          { "pickType": 1, "pickValue": 224, "text": 224 }
        ],
        "textFilter": ""
      },
      {
        "facetCode": "genus",
        "position": 2,
        "picks": [
          { "pickType": 1, "pickValue": 12974, "text": 12974 },
          { "pickType": 1, "pickValue": 12793, "text": 12793 },
          { "pickType": 1, "pickValue": 12932, "text": 12932 }
        ],
        "textFilter": ""
      },
      { "facetCode": "sites", "position": 3, "picks": [], "textFilter": "" }
    ]
  },
  "resultConfig": {
    "requestId": 23,
    "sessionId": "1",
    "viewTypeId": "map",
    "aggregateKeys": ["site_level"]
  }
}
```

### xyz

Trigger: site

```json

```
