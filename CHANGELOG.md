
# Changelog

## Release @2024.05

### Enhancement

-  The SEAD backend now supports map facets (#94, #112)
A "geopolygon_sites"
-   and intersecting ranges facets.
- New improved adaptive range interval calculator.
  
Closed issues:

- Upgrade to dotnet8	maintenance (#128) CLOSED	
- Map facet	new feature (#112)
- Geographic/map filter	new feature (#94)
- Regression in range facets after Geo Facet deploy	bug (#132)
- Disable FullCategoryCounts in result Load for "map" results	enhancement (#130)
- Incorrect amount of analysis_entities returned for facet query (#129)
- Wrong number of analysis entities returned for a site? (#113)

	Add number of samples to result load	enhancement (#133) OPEN	
	Can't use Region and Country filter in combination	data-error (#127) CLOSED	
	Genus filter innehåller 'null'-poster	 (#122) OPEN	
	Geochronology för dendro	dendro (#118) OPEN	
	Mer avancerat region filter?	enhancement, dendro (#117) CLOSED	
	No sites for tabular view within isotope domain	bug (#116) OPEN	
	Filter for tbl_analysis_entity_ages	important (#115) OPEN	
	Taxa filter is hiding sp. and spp. taxa	bug (#114) OPEN	
	Filtret "Region" beter sig på ett oväntat sätt	data-error (#111) CLOSED	
	Problem with "FullExtent" values returned for range filter	 (#107) CLOSED	
	Abundance classification filter: nothing is retrieved	supersead (#106) OPEN	
	Construction types filter har många dubbletter, plus output saknas	dendro (#105) CLOSED	
	Bibliography filters display empty rows	 (#104) OPEN	
	Countries and Region facets fails when used together	 (#103) OPEN	
	Region facet provides non-unique ID's	bug (#102) OPEN	
	API returns an empty list for domain facets	data-error (#100) CLOSED	
	500 Internal Server Error	 (#98) CLOSED	
	SEAD Query API service goes down intermittently	bug (#91) CLOSED	
	Upgrade to dotnet 6.0	maintenance (#90) CLOSED	
	Taxonomy + dating queries not working	bug (#89) OPEN	
	Test should not call online database	bug (#88) CLOSED	
	Faulty facet: Restricted facet with non-enforced constraint	bug, investigation, data-error (#87) CLOSED	
	Facet group data inte tillgänlig	enhancement (#86) CLOSED	
	Facet "Dataset methods" fails to load	data-error (#85) CLOSED	
	Facet "abundance classification" fails to load	bug, help wanted, data-error (#81) OPEN	
	Error in REST API call: NpgsqlOperationInProgressException	bug (#80) CLOSED	
	NullReference in REST API call	bug (#79) CLOSED	
	Outdated repositories in Dockerfile	bug (#78) CLOSED	
	Expose version of API	new feature (#77) CLOSED	
	Quality Check: Examine facet graph relations	investigation (#74) CLOSED	
	Integration Test: syntax error at or near "AND"'	bug (#70) CLOSED	
	Deprecate API fields	documentation (#69) CLOSED	
	Domain facets API returns empty set	bug (#68) CLOSED	
	Domain facet children API call returns empty set	bug (#67) CLOSED	
	Domain facets are ignored in facet chain query compile	bug, enhancement (#66) CLOSED	
	Facet that has a table with alias equal to NULL raises exception	bug (#65) CLOSED	
	Facet filter criteria not included in compiled SQL query	bug (#64) CLOSED	
	Add optional additional join criteria in facet graph	enhancement (#63) CLOSED	
	Result load of "tabular" isotope data fails	data-error (#62) CLOSED	
	Deploy SEAD Query API @2020-03	maintenance (#60) CLOSED	
	Incorrect data points showing when selecting Master datasets second time	bug (#59) CLOSED	
	Guide: How to create new facets	process, documentation (#57) CLOSED	
	Upgrade to .net core 3.1	enhancement (#56) CLOSED	
	Ability to switch backend	enhancement (#55) CLOSED	
	Add filter descriptions	enhancement (#54) CLOSED	
	Facet groups needs titles	enhancement (#53) CLOSED	
	Very poor performance for multi-select discrete facets	bug (#52) CLOSED	
	Use inner joins as default in query builder	enhancement, policy change (#51) CLOSED	
	Syntax error in aggregated SQL Query	bug (#50) CLOSED	
	Server failure on multi-country filter select + LOI result load	bug (#49) CLOSED	
	Inclusion of data spans with zero data?	bug (#48) CLOSED	
	Verify that Redis cache works as expected.	investigation (#47) CLOSED	
	docker-compose throws "Segmentation fault" at startup	bug (#46) CLOSED	
	Add volume binding to log folder	enhancement (#45) CLOSED	
	Redis data folder is missing	bug (#44) CLOSED	
	Missing time data	duplicate (#42) CLOSED	
	Result load fails	bug (#41) CLOSED	
	Iconsistent counts for value spans	bug, question (#40) CLOSED	
	Multiple facets having same alias causes exception	bug (#39) CLOSED	
	MS filter with picks + Site filter results in error	bug (#37) CLOSED	
	MS filter internal server error	bug, duplicate, wontfix (#36) CLOSED	
	Sample group filter error	bug (#35) CLOSED	
	Country filter error	bug (#34) CLOSED	
	Allow multiple instances of a filter	enhancement (#33) OPEN	
	Add bibliography filters	enhancement (#32) OPEN	
	Raä-IDn	enhancement (#31) CLOSED	
	Internal server error on LOI-filter request with picks	wontfix (#30) CLOSED	
	Internal server error (500) on load of MS facet	bug (#29) CLOSED	
	Reconstitute of FacetsConfig fails if TriggerCode when null.	bug (#28) CLOSED	
	Reconstitute of facet configuration fails when /load is called	bug (#27) CLOSED	
	Improve application logging	enhancement (#26) CLOSED	
	Error on facet load	bug (#25) CLOSED	
	Update measured values facets to use UDF.	enhancement (#24) CLOSED	
	Missing data in biblio	duplicate, data-error (#23) CLOSED	
	Result load returns 500.	bug (#22) CLOSED	
	404 on OPTIONS request	SLA (#21) CLOSED	
	Facet table is assigned wrong alias.	bug (#19) CLOSED	
	Graph search returns faulty routes	bug (#18) CLOSED	
	Facets with alias name give erroneous SQL query	bug, duplicate (#17) CLOSED	
	Add support to use UDF in facet specification	enhancement (#14) CLOSED	
	Stöd för metod-filtrerade filter	enhancement (#13) CLOSED	
	Adapt to schema changes (up to SEAD CCS v0.1)	enhancement (#10) CLOSED	


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
