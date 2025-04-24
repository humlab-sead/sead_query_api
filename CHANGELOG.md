
# Changelog

## Release @2024.05

### Enhancement

-  The SEAD backend now supports map facets (#94, #112)
A "geopolygon_sites"
-   and intersecting ranges facets.
- New improved adaptive range interval calculator.
  
Closed issues:

- Upgrade to dotnet8 maintenance (#128) CLOSED	
- Map facet	new feature (#112)
- Geographic/map filter	new feature (#94)
- Regression in range facets after Geo Facet deploy	bug (#132)
- Disable FullCategoryCounts in result Load for "map" results	enhancement (#130)
- Incorrect amount of analysis_entities returned for facet query (#129)
- Wrong number of analysis entities returned for a site? (#113)


## Release @2023.12

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
  facetsConfig: {
    requestId: 1,
    requestType: populate,
    domainCode: pollen,
    targetCode: sites,
    triggerCode: sites,
    facetConfigs: [ { facetCode: sites, position: 1, picks: [], textFilter:  }
    ]
  },
  resultConfig: {... }
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

1) A `SqlQuery` field has been added to the `/api/facets/load`. The field contains query that produced the result items. The prupose is to simplify debugging (and integration testing).

### Domain facet configuration

 | domain_code | doman_display_name | facet_code | display_title | position | new? |
 |:-------------|:-------------|:-------------|:-------------|:--|---|
 | archaeobotany | Archaeobotany | ecocode_system | Eco code system | 1 |  |
 | archaeobotany | Archaeobotany | ecocode | Eco code | 2 |  |
 | archaeobotany | Archaeobotany | abundances_all | Abundances | 3 |  |
 | archaeobotany | Archaeobotany | geochronology | Geochronology | 4 |  |
 | archaeobotany | Archaeobotany | relative_age_name | Time periods | 5 |  |
 | archaeobotany | Archaeobotany | activeseason | Seasons | 6 |  |
 | archaeobotany | Archaeobotany | family | Family | 7 |  |
 | archaeobotany | Archaeobotany | genus | Genus | 8 |  |
 | archaeobotany | Archaeobotany | species | Taxon | 9 |  |
 | archaeobotany | Archaeobotany | species_author | Author | 10 |  |
 | archaeobotany | Archaeobotany | feature_type | Feature type | 11 |  |
 | archaeobotany | Archaeobotany | tbl_biblio_modern | Bibligraphy modern | 12 |  |
 | archaeobotany | Archaeobotany | country | Countries | 14 |  |
 | archaeobotany | Archaeobotany | sites | Site | 15 |  |
 | archaeobotany | Archaeobotany | sample_groups | Sample groups | 16 |  |
 | archaeobotany | Archaeobotany | sample_group_sampling_contexts | Sampling Contexts | 20 | NEW! |
 | archaeobotany | Archaeobotany | data_types | Data types | 21 | NEW! |
 | archaeobotany | Archaeobotany | modification_types | Modification Types | 22 | NEW! |
 | archaeobotany | Archaeobotany | abundance_elements | Abundance Elements | 23 | NEW! |
 | ceramic | Ceramic | geochronology | Geochronology | 1 |  |
 | ceramic | Ceramic | relative_age_name | Time periods | 2 |  |
 | ceramic | Ceramic | feature_type | Feature type | 7 |  |
 | ceramic | Ceramic | tbl_biblio_modern | Bibligraphy modern | 8 |  |
 | ceramic | Ceramic | country | Countries | 10 |  |
 | ceramic | Ceramic | sites | Site | 11 |  |
 | ceramic | Ceramic | sample_groups | Sample groups | 12 |  |
 | ceramic | Ceramic | sample_group_sampling_contexts | Sampling Contexts | 20 | NEW! |
 | ceramic | Ceramic | data_types | Data types | 21 | NEW! |
 | dendrochronology | Dendrochronology | geochronology | Geochronology | 1 |  |
 | dendrochronology | Dendrochronology | relative_age_name | Time periods | 2 |  |
 | dendrochronology | Dendrochronology | family | Family | 3 |  |
 | dendrochronology | Dendrochronology | genus | Genus | 4 |  |
 | dendrochronology | Dendrochronology | species | Taxon | 5 |  |
 | dendrochronology | Dendrochronology | species_author | Author | 6 |  |
 | dendrochronology | Dendrochronology | feature_type | Feature type | 7 |  |
 | dendrochronology | Dendrochronology | tbl_biblio_modern | Bibligraphy modern | 8 |  |
 | dendrochronology | Dendrochronology | country | Countries | 10 |  |
 | dendrochronology | Dendrochronology | sites | Site | 11 |  |
 | dendrochronology | Dendrochronology | sample_groups | Sample groups | 12 |  |
 | dendrochronology | Dendrochronology | sample_group_sampling_contexts | Sampling Contexts | 20 | NEW! |
 | dendrochronology | Dendrochronology | data_types | Data types | 21 | NEW! |
 | geoarchaeology | Geoarchaeology | tbl_denormalized_measured_values_33_82 | MS Heating 550 | 1 |  |
 | geoarchaeology | Geoarchaeology | tbl_denormalized_measured_values_37 | Phosphates | 2 |  |
 | geoarchaeology | Geoarchaeology | geochronology | Geochronology | 3 |  |
 | geoarchaeology | Geoarchaeology | relative_age_name | Time periods | 4 |  |
 | geoarchaeology | Geoarchaeology | feature_type | Feature type | 5 |  |
 | geoarchaeology | Geoarchaeology | tbl_biblio_modern | Bibligraphy modern | 6 |  |
 | geoarchaeology | Geoarchaeology | country | Countries | 8 |  |
 | geoarchaeology | Geoarchaeology | sites | Site | 9 |  |
 | geoarchaeology | Geoarchaeology | sample_groups | Sample groups | 10 |  |
 | geoarchaeology | Geoarchaeology | sample_group_sampling_contexts | Sampling Contexts | 12 | NEW! |
 | geoarchaeology | Geoarchaeology | data_types | Data types | 13 | NEW! |
 | geoarchaeology | Geoarchaeology | tbl_denormalized_measured_values_32 | Loss of Ignition | 14 |  |
 | geoarchaeology | Geoarchaeology | tbl_denormalized_measured_values_33_0 | Magnetic sus. | 15 |  |
 | isotope | Isotope | relative_age_name | Time periods | 2 |  |
 | isotope | Isotope | feature_type | Feature type | 7 |  |
 | isotope | Isotope | tbl_biblio_modern | Bibligraphy modern | 8 |  |
 | isotope | Isotope | country | Countries | 10 |  |
 | isotope | Isotope | sites | Site | 11 |  |
 | isotope | Isotope | sample_groups | Sample groups | 12 |  |
 | isotope | Isotope | sample_group_sampling_contexts | Sampling Contexts | 20 | NEW! |
 | isotope | Isotope | data_types | Data types | 21 | NEW! |
 | palaeoentomology | Palaeoentomology | ecocode_system | Eco code system | 1 |  |
 | palaeoentomology | Palaeoentomology | ecocode | Eco code | 2 |  |
 | palaeoentomology | Palaeoentomology | abundances_all | Abundances | 3 |  |
 | palaeoentomology | Palaeoentomology | geochronology | Geochronology | 4 |  |
 | palaeoentomology | Palaeoentomology | relative_age_name | Time periods | 5 |  |
 | palaeoentomology | Palaeoentomology | activeseason | Seasons | 6 |  |
 | palaeoentomology | Palaeoentomology | family | Family | 7 |  |
 | palaeoentomology | Palaeoentomology | genus | Genus | 8 |  |
 | palaeoentomology | Palaeoentomology | species | Taxon | 9 |  |
 | palaeoentomology | Palaeoentomology | species_author | Author | 10 |  |
 | palaeoentomology | Palaeoentomology | feature_type | Feature type | 11 |  |
 | palaeoentomology | Palaeoentomology | tbl_biblio_modern | Bibligraphy modern | 12 |  |
 | palaeoentomology | Palaeoentomology | country | Countries | 14 |  |
 | palaeoentomology | Palaeoentomology | sites | Site | 15 |  |
 | palaeoentomology | Palaeoentomology | sample_groups | Sample groups | 16 |  |
 | palaeoentomology | Palaeoentomology | rdb_systems | RDB system | 17 | NEW! |
 | palaeoentomology | Palaeoentomology | rdb_codes | RDB Code | 18 | NEW! |
 | palaeoentomology | Palaeoentomology | sample_group_sampling_contexts | Sampling Contexts | 20 | NEW! |
 | palaeoentomology | Palaeoentomology | data_types | Data types | 21 | NEW! |
 | pollen | Pollen | ecocode_system | Eco code system | 1 |  |
 | pollen | Pollen | ecocode | Eco code | 2 |  |
 | pollen | Pollen | abundances_all | Abundances | 3 |  |
 | pollen | Pollen | geochronology | Geochronology | 4 |  |
 | pollen | Pollen | relative_age_name | Time periods | 5 |  |
 | pollen | Pollen | activeseason | Seasons | 6 |  |
 | pollen | Pollen | family | Family | 7 |  |
 | pollen | Pollen | genus | Genus | 8 |  |
 | pollen | Pollen | species | Taxa | 9 |  |
 | pollen | Pollen | species_author | Author | 10 |  |
 | pollen | Pollen | feature_type | Feature type | 11 |  |
 | pollen | Pollen | tbl_biblio_modern | Bibligraphy modern | 12 |  |
 | pollen | Pollen | country | Countries | 14 |  |
 | pollen | Pollen | sites | Site | 15 |  |
 | pollen | Pollen | sample_groups | Sample groups | 16 |  |
 | pollen | Pollen | sample_group_sampling_contexts | Sampling Contexts | 20 | NEW! |
 | pollen | Pollen | data_types | Data types | 21 | NEW! |
 | pollen | Pollen | abundance_elements | Abundance Elements | 23 | NEW! |

### Renamed facet display names

1) "Loss of Ignition" => "Loss on Ignition"
2) "Taxon" => "Taxa"

### Pending / Not implemented

- Bibligraphy fossil
- Analysis entity ages
- [Any|Every]thing else not specified in #13.
- Additonal Ceramics user facets not specified (apart from #13)
- Isotope user facets not specified in #13 (set to those displaying data in broswer.sead.se)

### Orphaned facets

These facets have not been assoiciated to any domain facet, although, they are returned by `api/facets` and `api/facets/domain/general`:

- record_types
- abundance_classification
- tbl_biblio_sample_groups
- tbl_biblio_sites
- dataset_master
- dataset_methods
- region

# git-chlog -o FILENAME

<a name="@2024-05"></a>
## [@2024-05](https://github.com/humlab-sead/sead_query_api/compare/2024.02.RC1...@2024-05) (2025-03-28)

### Chore

* added test files

### Refactor

* moved interface file
* renamed folder
* renamed files
* Moved file
* disentangled Graph/Registry dependency from PathFinder
* update editor semantic token colors
* improve dotnet test verbosity in Makefile
* consolidated and simplified path find logic
* remove unused RepositoryTests class
* improve Dijkstra's graph representation and path finding with additional constructors
* add ToTuples extension method to TableRelation class
* expose Edges
* Add GetEdges method and ToRoute method to IEdgeRepository
* major route finder refactoring
* renamed facet code
* added methods
* renamed repository, add medthod
* renamed repositories, expose context
* renames & simplifications
* Renamed and simplified interface
* Update repository constructors to use RepositoryRegistry instead of IFacetContext
* moved methods
* renamed parameter, moved property
* mobed lookup to seperate file
* Update IGraphNode interface and related classes
* simplified IEdge interface
* simplify list initialization
* reduce indentation
* Update FacetGraph class to use container naming convention
* Simplify DijkstrasGraph constructor
* Update JoinsClauseCompiler to use primary constructor
* switch range operators
* switch operande in range expression
* Add facet_load_intersect.rest integration test and update QueryBuilderTests
* Add GetRangeAsync method to DatabaseQueryProxy
* Update FacetLoadService to use UriToFacetsConfig for loading facets
* Remove unnecessary usings and update test data
* Remove unnecessary usings
* Remove unnecessary files.exclude settings in .vscode/settings.json
* Update facet configurations to remove unnecessary "textFilter" property
* Update CountFieldCompilerTests.cs to include distinct expression test case
* Update FacetLoadService.cs to include missing test cases
* Update intervalCount default value to 50 in CategoryInfoService.cs

### Pull Requests

* Merge pull request [#149](https://github.com/humlab-sead/sead_query_api/issues/149) from humlab-sead:dev
* Merge pull request [#126](https://github.com/humlab-sead/sead_query_api/issues/126) from humlab-sead/dev
* Merge pull request [#124](https://github.com/humlab-sead/sead_query_api/issues/124) from humlab-sead:dev
* Merge pull request [#123](https://github.com/humlab-sead/sead_query_api/issues/123) from humlab-sead:dev


<a name="2024.02.RC1"></a>
## [2024.02.RC1](https://github.com/humlab-sead/sead_query_api/compare/2023.12...2024.02.RC1) (2024-04-25)

### Pull Requests

* Merge pull request [#131](https://github.com/humlab-sead/sead_query_api/issues/131) from humlab-sead:geo-facets
* Merge pull request [#125](https://github.com/humlab-sead/sead_query_api/issues/125) from humlab-sead:issue-106


<a name="2023.12"></a>
## [2023.12](https://github.com/humlab-sead/sead_query_api/compare/@2023.12...2023.12) (2024-03-22)

### Pull Requests

* Merge pull request [#123](https://github.com/humlab-sead/sead_query_api/issues/123) from humlab-sead:dev


<a name="@2023.12"></a>
## [@2023.12](https://github.com/humlab-sead/sead_query_api/compare/2023.12.rc1...@2023.12) (2024-03-22)


<a name="2023.12.rc1"></a>
## [2023.12.rc1](https://github.com/humlab-sead/sead_query_api/compare/2022.04.16.dotnet5...2023.12.rc1) (2024-03-21)

### Pull Requests

* Merge pull request [#121](https://github.com/humlab-sead/sead_query_api/issues/121) from humlab-sead/bugg-106
* Merge pull request [#120](https://github.com/humlab-sead/sead_query_api/issues/120) from humlab-sead:fix-rest-test-calls
* Merge pull request [#119](https://github.com/humlab-sead/sead_query_api/issues/119) from humlab-sead/net6.to.net8


<a name="2022.04.16.dotnet5"></a>
## [2022.04.16.dotnet5](https://github.com/humlab-sead/sead_query_api/compare/2022.04.16...2022.04.16.dotnet5) (2022-04-16)


<a name="2022.04.16"></a>
## [2022.04.16](https://github.com/humlab-sead/sead_query_api/compare/2022.04.14.rc3...2022.04.16) (2022-04-16)


<a name="2022.04.14.rc3"></a>
## [2022.04.14.rc3](https://github.com/humlab-sead/sead_query_api/compare/2022.04.14.rc2...2022.04.14.rc3) (2022-04-14)


<a name="2022.04.14.rc2"></a>
## [2022.04.14.rc2](https://github.com/humlab-sead/sead_query_api/compare/2022.04.14.rc1...2022.04.14.rc2) (2022-04-14)


<a name="2022.04.14.rc1"></a>
## [2022.04.14.rc1](https://github.com/humlab-sead/sead_query_api/compare/v1.1.0.RC.02...2022.04.14.rc1) (2022-04-14)

### Refactor

* minor cleanups


<a name="v1.1.0.RC.02"></a>
## [v1.1.0.RC.02](https://github.com/humlab-sead/sead_query_api/compare/v1.1.0.RC.01...v1.1.0.RC.02) (2020-05-08)

### Refactor

* renames and code cleanups
* renames
* Simplified ResultService Removed inheritance hiearchy
* class to file, cleanups
* moved interface
* Improved result services
* minor cleanups
* moved logic to class
* renamed items
* review/cleanup
* rename
* rename+shortcut
* ranmed extension method
* Renamed property
* test code cleanups
* Renames


<a name="v1.1.0.RC.01"></a>
## [v1.1.0.RC.01](https://github.com/humlab-sead/sead_query_api/compare/v1.0.0...v1.1.0.RC.01) (2020-04-23)

### Docker

* Dotnet core base image update

### EntityFramework

* Adapted to API changes

### Refactor

* namespace cleanup/rename
* namespace cleanup/rename
* namespace cleanup/rename
* rename, file sorting
* Filename / Folder structure cleanup
* renamed variable
* Simplified FacetGraph logic
* RepositoryRegistry
* class name change
* Name change
* Split into class files
* renamed file
* Split into separate files
* name changes, variable removal
* Name change
* changed method/variable names etc
* changed method name
* name changes
* name change
* renamed namespace
* Removed unused API elements
* Removed unused API elements
* renamed namespace
* removed private method
* Removed deprecated code
* minor cleanups
* Code cleanup if FacetContext
* class/interface name change
* Moved logger init code to separate file
* changed folder structure

### Test

* Scaffold script updates

### Pull Requests

* Merge pull request [#76](https://github.com/humlab-sead/sead_query_api/issues/76) from humlab-sead/2.2-to-3.1


<a name="v1.0.0"></a>
## v1.0.0 (2020-04-23)

### Refacorings

* Rename of config elements (Settings)

### Refactor

* Schema change (relation to Table)
* IFacetGraphFactory + class names changes
* name change to Table, TableRelation
* Name change ObjectName => TableOrUdfName
* Rename DeletePick to ClearPick
* Test refactorings and improvements
* Name changes (*Builder => *Compiler) + Some small code tidy ups...
* Moved interfaces to individual files

### Refactoring

* Renamed Graph entities property names to more appropriate, less confusing names
* Renamed FacetDefinitions -> Facets (IFacetContect)

### Refactorings

* Tests adapted to changes
* More consistent class naming scheme
* One class per file, removed static classes
* Name changes facet attributes
* Renamed file GraphTableRelations.cs -> GraphEdge.cs
* Renamed GraphTableRelation -> GraphEdge
* Renamed file GraphTable.cs -> GraphNode.cs
* Renamed GraphTable -> GraphNode
* setting given more appropriate names
* Class renames (Facet, IFacetContext, GraphEdge etc)

### Pull Requests

* Merge pull request [#20](https://github.com/humlab-sead/sead_query_api/issues/20) from humlab-sead/adapt-to-ccs-v0.1

