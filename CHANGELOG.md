
# SEAD Query API change log

## Release @2020.03

The following API fields are now deprecated:

| Domain field | Database field |
|:----------|:--------------|
| `ResultField.LinkUrl` | `facet.result_field.link_url` |
| `ResultField.LinkName` |  `facet.result_field.link_name` |
| `ResultAggregate.InputType` |  `facet.result_aggregate.input_type` |
| `ResultAggregate.IsApplicable` |  `facet.result_aggregate.is_applicable` |
| `ResultAggregate.HasSelector` |  `facet.result_aggregate.has_selector` |

The following new API GET methods are now avaliable:

| API | Database field |
|:----------|:--------------|
| `/api/facets/domain` | Returns list of domain facets |
| `/api/facets/domain/:id` | Returns all facets valid for domain id |

The following changes are made to existing API JSON changes i get methods are now avaliable:

| API | Database field |
|:----------|:--------------|
| `/api/facets/load` | Added field `DomainCode` to request JSON payload |
| `/api/facets/domain/:id | Returns all facets valid for domain id |

### Implicit domain facet filter

If a valid domain code os specified in the `DomainCode` requests JSON payload than the query engine will add an implicit filter that constains the succeeding data to the specified domain. What happens under the hood is that the domain facet is added at the head of the facet chain. Note that the clent should not add the domain facet to the request payload - this is done automatically.

### New facets

The following new facets have been added to the system:

### Improved automated testing

The unit and integration testing has been vastly improved. The `Moq` framework is used for SUT isolation, dependent objects are to SUT are faked or mocked  The tests uses a fixed facet database context stored as JSON files. This data can be updated from an online database. The integration tests that requires a DB backend loads the data into an in-memory Sqlite database engine.  A total of +200 tests has been added. The code coverage has not been computed, but the goal is to reach ~ 100%.

