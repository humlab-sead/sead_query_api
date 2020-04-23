
# Changelog

## Release @2020.04 (v1.2.0)

## Release @2020.03 (v1.1.0)

This is a major update with 470 changed files, 15,792 additions and 66,007 deletions.

### Enhancements

1) New API GET methods are now avaliable:

| API | Database field |
|:----------|:--------------|
| `/api/facets/domain` | Returns list of domain facets |
| `/api/facets/domain/:domainCode` | Returns all facets valid for given domain facet code. Empty code returns all facets. |

2) Request can now be constrained to a data domain.

This feature implicitally constrains the requested results to only include data within the specified data domain. Allowed domains (i.e. DomainCode values) are the facet codes of new _domain facets_ (retrieved by /api/facets/domain). Internally, the domain facets contain the specific constraints that are automatically added by the query engine. Note that the client only has to specify **the domain code** in the requests for this to happen. There is no need for the client to add, send, or display the domain facet. Also note that the result from `/api/facets/domain` gives all avaliable domain facets.

What happens under the hood is that the domain facet is added at the head of the facet chain.

If `domainCode` is an empty string then no implicit filter is applied



| API | Database field |
|:----------|:--------------|
| `/api/facets/load` | New field `domainCode` to added request's `facesConfig` JSON payload |
| `/api/results/load`| New field `domainCode` to request's `facesConfig` JSON payload |

Example: Retrive sits constrained to domain `pollen`:

```json
{
  "facetsConfig": {
    "requestId": 1,
    "requestType": "populate",
    "domainCode": "pollen",
    "targetCode": "sites",
    "triggerCode": "sites",
    "facetConfigs": [ { "facetCode": "sites", "position": 1, "picks": [], "textFilter": "" }
    ]
  },
  "resultConfig": {... }
}
```

[20200120_DML_ADD_DOMAIN_FACETS](https://github.com/humlab-sead/sead_change_control/blob/master/sead_api/deploy/20200120_DML_ADD_DOMAIN_FACETS.sql) contains the SQL that adds the new domain facets.

3) New JSON API for adding new facets

New facets can now be specified using JSON

### Breaking changes

1) The following JSON API fields are now deprecated:

| Domain field | Database field |
|:----------|:--------------|
| `ResultField.LinkUrl` | `facet.result_field.link_url` |
| `ResultField.LinkName` |  `facet.result_field.link_name` |
| `ResultAggregate.InputType` |  `facet.result_aggregate.input_type` |
| `ResultAggregate.IsApplicable` |  `facet.result_aggregate.is_applicable` |
| `ResultAggregate.HasSelector` |  `facet.result_aggregate.has_selector` |

2) Avoid using the `/api/facets` API call since it retrieves all enabled facets (except those having facet group 0 or 999). Note that some of twe new facets are only valid for specific domain(s), so the `/api/facets/domain/:domainCode` should be used instead since it only retrieves facets valid for the specified domain.


### Fixes

See [closed bugs].(https://github.com/humlab-sead/sead_query_api/issues?q=is%3Aissue+is%3Aclosed+milestone%3A%22SEAD+%402020.03+release%22+label%3Abug)

### Other noteworthy things

1) The backend has been upgraded to .NET core 3.1.

1) The magic numer `999` in column `facet_group_id` identifies domain facets.

1) The unit and integration testing has been vastly improved. The `Moq` framework is used for SUT isolation, dependent objects are to SUT are faked or mocked  The tests uses a fixed facet database context stored as JSON files. This data can be updated from an online database. The integration tests that requires a DB backend loads the data into an in-memory Sqlite database engine.  A total of +200 tests has been added. The code coverage has not been computed, but the goal is to reach ~ 100%.

1) Table `facet.facet_children` associates domain facets to their children/user facets.

### Domain facet configuration

| "domain\_code"     | "facet\_code"                                 | "display\_title"     | "position" | "case"  |
|--------------------|-----------------------------------------------|----------------------|------------|---------|
| "archaeobotany"    | "ecocode\_system"                             | "Eco code system"    | 1          |         |
| "archaeobotany"    | "ecocode"                                     | "Eco code"           | 2          |         |
| "archaeobotany"    | "abundances\_all"                             | "Abundances"         | 3          |         |
| "archaeobotany"    | "geochronology"                               | "Geochronology"      | 4          |         |
| "archaeobotany"    | "relative\_age\_name"                         | "Time periods"       | 5          |         |
| "archaeobotany"    | "activeseason"                                | "Seasons"            | 6          |         |
| "archaeobotany"    | "family"                                      | "Family"             | 7          |         |
| "archaeobotany"    | "genus"                                       | "Genus"              | 8          |         |
| "archaeobotany"    | "species"                                     | "Taxon"              | 9          |         |
| "archaeobotany"    | "species\_author"                             | "Author"             | 10         |         |
| "archaeobotany"    | "feature\_type"                               | "Feature type"       | 11         |         |
| "archaeobotany"    | "tbl\_biblio\_modern"                         | "Bibligraphy modern" | 12         |         |
| "archaeobotany"    | "country"                                     | "Countries"          | 14         |         |
| "archaeobotany"    | "sites"                                       | "Site"               | 15         |         |
| "archaeobotany"    | "sample\_groups"                              | "Sample groups"      | 16         |         |
| "archaeobotany"    | "sample\_group\_sampling\_contexts"           | "Sampling Contexts"  | 20         | "NEW\!" |
| "archaeobotany"    | "data\_types"                                 | "Data types"         | 21         | "NEW\!" |
| "archaeobotany"    | "modification\_types"                         | "Modification Types" | 22         | "NEW\!" |
| "archaeobotany"    | "abundance\_elements"                         | "Abundance Elements" | 23         | "NEW\!" |
| "ceramic"          | "geochronology"                               | "Geochronology"      | 1          |         |
| "ceramic"          | "relative\_age\_name"                         | "Time periods"       | 2          |         |
| "ceramic"          | "feature\_type"                               | "Feature type"       | 7          |         |
| "ceramic"          | "tbl\_biblio\_modern"                         | "Bibligraphy modern" | 8          |         |
| "ceramic"          | "country"                                     | "Countries"          | 10         |         |
| "ceramic"          | "sites"                                       | "Site"               | 11         |         |
| "ceramic"          | "sample\_groups"                              | "Sample groups"      | 12         |         |
| "ceramic"          | "sample\_group\_sampling\_contexts"           | "Sampling Contexts"  | 20         | "NEW\!" |
| "ceramic"          | "data\_types"                                 | "Data types"         | 21         | "NEW\!" |
| "dendrochronology" | "geochronology"                               | "Geochronology"      | 1          |         |
| "dendrochronology" | "relative\_age\_name"                         | "Time periods"       | 2          |         |
| "dendrochronology" | "family"                                      | "Family"             | 3          |         |
| "dendrochronology" | "genus"                                       | "Genus"              | 4          |         |
| "dendrochronology" | "species"                                     | "Taxon"              | 5          |         |
| "dendrochronology" | "species\_author"                             | "Author"             | 6          |         |
| "dendrochronology" | "feature\_type"                               | "Feature type"       | 7          |         |
| "dendrochronology" | "tbl\_biblio\_modern"                         | "Bibligraphy modern" | 8          |         |
| "dendrochronology" | "country"                                     | "Countries"          | 10         |         |
| "dendrochronology" | "sites"                                       | "Site"               | 11         |         |
| "dendrochronology" | "sample\_groups"                              | "Sample groups"      | 12         |         |
| "dendrochronology" | "sample\_group\_sampling\_contexts"           | "Sampling Contexts"  | 20         | "NEW\!" |
| "dendrochronology" | "data\_types"                                 | "Data types"         | 21         | "NEW\!" |
| "geoarchaeology"   | "tbl\_denormalized\_measured\_values\_33\_82" | "MS Heating 550"     | 1          |         |
| "geoarchaeology"   | "tbl\_denormalized\_measured\_values\_37"     | "Phosphates"         | 2          |         |
| "geoarchaeology"   | "geochronology"                               | "Geochronology"      | 3          |         |
| "geoarchaeology"   | "relative\_age\_name"                         | "Time periods"       | 4          |         |
| "geoarchaeology"   | "feature\_type"                               | "Feature type"       | 5          |         |
| "geoarchaeology"   | "tbl\_biblio\_modern"                         | "Bibligraphy modern" | 6          |         |
| "geoarchaeology"   | "country"                                     | "Countries"          | 8          |         |
| "geoarchaeology"   | "sites"                                       | "Site"               | 9          |         |
| "geoarchaeology"   | "sample\_groups"                              | "Sample groups"      | 10         |         |
| "geoarchaeology"   | "sample\_group\_sampling\_contexts"           | "Sampling Contexts"  | 12         | "NEW\!" |
| "geoarchaeology"   | "data\_types"                                 | "Data types"         | 13         | "NEW\!" |
| "geoarchaeology"   | "tbl\_denormalized\_measured\_values\_32"     | "Loss of Ignition"   | 14         |         |
| "geoarchaeology"   | "tbl\_denormalized\_measured\_values\_33\_0"  | "Magnetic sus\."     | 15         |         |
| "palaeoentomology" | "ecocode\_system"                             | "Eco code system"    | 1          |         |
| "palaeoentomology" | "ecocode"                                     | "Eco code"           | 2          |         |
| "palaeoentomology" | "abundances\_all"                             | "Abundances"         | 3          |         |
| "palaeoentomology" | "geochronology"                               | "Geochronology"      | 4          |         |
| "palaeoentomology" | "relative\_age\_name"                         | "Time periods"       | 5          |         |
| "palaeoentomology" | "activeseason"                                | "Seasons"            | 6          |         |
| "palaeoentomology" | "family"                                      | "Family"             | 7          |         |
| "palaeoentomology" | "genus"                                       | "Genus"              | 8          |         |
| "palaeoentomology" | "species"                                     | "Taxon"              | 9          |         |
| "palaeoentomology" | "species\_author"                             | "Author"             | 10         |         |
| "palaeoentomology" | "feature\_type"                               | "Feature type"       | 11         |         |
| "palaeoentomology" | "tbl\_biblio\_modern"                         | "Bibligraphy modern" | 12         |         |
| "palaeoentomology" | "country"                                     | "Countries"          | 14         |         |
| "palaeoentomology" | "sites"                                       | "Site"               | 15         |         |
| "palaeoentomology" | "sample\_groups"                              | "Sample groups"      | 16         |         |
| "palaeoentomology" | "rdb\_systems"                                | "RDB system"         | 17         | "NEW\!" |
| "palaeoentomology" | "rdb\_codes"                                  | "RDB Code"           | 18         | "NEW\!" |
| "palaeoentomology" | "sample\_group\_sampling\_contexts"           | "Sampling Contexts"  | 20         | "NEW\!" |
| "palaeoentomology" | "data\_types"                                 | "Data types"         | 21         | "NEW\!" |
| "pollen"           | "ecocode\_system"                             | "Eco code system"    | 1          |         |
| "pollen"           | "ecocode"                                     | "Eco code"           | 2          |         |
| "pollen"           | "abundances\_all"                             | "Abundances"         | 3          |         |
| "pollen"           | "geochronology"                               | "Geochronology"      | 4          |         |
| "pollen"           | "relative\_age\_name"                         | "Time periods"       | 5          |         |
| "pollen"           | "activeseason"                                | "Seasons"            | 6          |         |
| "pollen"           | "family"                                      | "Family"             | 7          |         |
| "pollen"           | "genus"                                       | "Genus"              | 8          |         |
| "pollen"           | "species"                                     | "Taxon"              | 9          |         |
| "pollen"           | "species\_author"                             | "Author"             | 10         |         |
| "pollen"           | "feature\_type"                               | "Feature type"       | 11         |         |
| "pollen"           | "tbl\_biblio\_modern"                         | "Bibligraphy modern" | 12         |         |
| "pollen"           | "country"                                     | "Countries"          | 14         |         |
| "pollen"           | "sites"                                       | "Site"               | 15         |         |
| "pollen"           | "sample\_groups"                              | "Sample groups"      | 16         |         |
| "pollen"           | "sample\_group\_sampling\_contexts"           | "Sampling Contexts"  | 20         | "NEW\!" |
| "pollen"           | "data\_types"                                 | "Data types"         | 21         | "NEW\!" |
| "pollen"           | "abundance\_elements"                         | "Abundance Elements" | 23         | "NEW\!" |

### Pending / Not implemented

- Bibligraphy fossil
- Analysis entity ages
- Everything else not specified in #13.
- Renaming of facets

### Orphan facets

"facet_code"
"record_types"
"abundance_classification"
"tbl_biblio_sample_groups"
"tbl_biblio_sites"
"dataset_master"
"dataset_methods"
"region"
