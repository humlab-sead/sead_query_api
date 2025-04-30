# 📦 Changelog 
[![conventional commits](https://img.shields.io/badge/conventional%20commits-1.0.0-yellow.svg)](https://conventionalcommits.org)
[![semantic versioning](https://img.shields.io/badge/semantic%20versioning-2.0.0-green.svg)](https://semver.org)
> All notable changes to this project will be documented in this file


## [1.1.0](https://github.com/humlab-sead/sead_query_api/compare/v1.0.0...v1.1.0) (2025-04-30)

### 🍕 Features

* add sample database schema for testing purposes ([48c0aea](https://github.com/humlab-sead/sead_query_api/commit/48c0aea317b3eb8df0f14188dcd35ce62c0c6c78))

### 🐛 Bug Fixes

*  fixed breaking changes in CacheManager API ([a02dbfd](https://github.com/humlab-sead/sead_query_api/commit/a02dbfdea9ddc499e1e6e7bf6eed0cb64f9baac4))
* update sample schema views for accuracy and performance ([432843b](https://github.com/humlab-sead/sead_query_api/commit/432843bbaf9a1f366543b32959957fc0e560a855))

### 🧑‍💻 Code Refactoring

* Add facet_load_intersect.rest integration test and update QueryBuilderTests ([a564f9e](https://github.com/humlab-sead/sead_query_api/commit/a564f9ea6494d6acb3a049611f508fa04c16b6d4))
* Add GetEdges method and ToRoute method to IEdgeRepository ([db85c27](https://github.com/humlab-sead/sead_query_api/commit/db85c2732e052178100f7c8f855a2af8949e329f))
* Add GetRangeAsync method to DatabaseQueryProxy ([80b3972](https://github.com/humlab-sead/sead_query_api/commit/80b3972dbf0f734d69368fde128fba483b294322))
* add ToTuples extension method to TableRelation class ([2143709](https://github.com/humlab-sead/sead_query_api/commit/2143709e15cb70c9ec3d244cfeea52d94f37244a))
* added methods ([9fbb267](https://github.com/humlab-sead/sead_query_api/commit/9fbb267d4cf844388cfb5e316402854651fc04d7))
* changed folder structure ([ca2fd5e](https://github.com/humlab-sead/sead_query_api/commit/ca2fd5e8a4c8387a07018b370b69d24a8dabd2af))
* changed method name ([48bcee2](https://github.com/humlab-sead/sead_query_api/commit/48bcee21dbbbdb511c3230124a002ab3f9248cbf))
* changed method/variable names etc ([163d2d8](https://github.com/humlab-sead/sead_query_api/commit/163d2d87f8b262da93279039eb47cfd02dc4661b))
* class name change ([e198f42](https://github.com/humlab-sead/sead_query_api/commit/e198f425c8b03d73e2f8913269f22680c5003634))
* class to file, cleanups ([fa33d98](https://github.com/humlab-sead/sead_query_api/commit/fa33d9852fc7b1019c26e2d12e0ded2182fbffcd))
* class/interface name change ([eed9b24](https://github.com/humlab-sead/sead_query_api/commit/eed9b247141e71e88010eb97c75e08d1aa20bb4c))
* Code cleanup if FacetContext ([eef8714](https://github.com/humlab-sead/sead_query_api/commit/eef8714a2e8b9df473e5fa1c1672511da454146b))
* consolidated and simplified path find logic ([860314e](https://github.com/humlab-sead/sead_query_api/commit/860314e0217f68ea5045a3b32d31bd9e42145e27))
* disentangled Graph/Registry dependency from PathFinder ([18c058d](https://github.com/humlab-sead/sead_query_api/commit/18c058dc4cee12a2f3cf65f6b62f753ccd604f80))
* expose Edges ([237ac91](https://github.com/humlab-sead/sead_query_api/commit/237ac9185d3c2b8346862e53186337f7a38faa97))
* Filename / Folder structure cleanup ([5358b51](https://github.com/humlab-sead/sead_query_api/commit/5358b517df9e8c47c77af5456983b61bc4ceee37))
* improve Dijkstra's graph representation and path finding with additional constructors ([2bc86c8](https://github.com/humlab-sead/sead_query_api/commit/2bc86c8541f8242fb130ffe6b5ee7401b6b2d242))
* improve dotnet test verbosity in Makefile ([b250ef3](https://github.com/humlab-sead/sead_query_api/commit/b250ef3035aef29416fd7421d501734148e0e07f))
* Improved result services ([32678af](https://github.com/humlab-sead/sead_query_api/commit/32678af1669bc251c78cd060b30b56d698cf5138))
* major route finder refactoring ([d2ea4cb](https://github.com/humlab-sead/sead_query_api/commit/d2ea4cbacd6e00f1ef7ffb86301e7351262f4c3a))
* minor cleanups ([ff3f16d](https://github.com/humlab-sead/sead_query_api/commit/ff3f16d183583c7a73cd677e5c05951d5c0b541d))
* minor cleanups ([5352acb](https://github.com/humlab-sead/sead_query_api/commit/5352acb1d778c8ff194564b327f93b753265199c))
* minor cleanups ([93c4086](https://github.com/humlab-sead/sead_query_api/commit/93c4086f9f1882fe8444726a0b2e9461eddf8a3a))
* mobed lookup to seperate file ([7657bf1](https://github.com/humlab-sead/sead_query_api/commit/7657bf19bcbb1d6f41dd3463691694a918b58ff6))
* Moved file ([be501eb](https://github.com/humlab-sead/sead_query_api/commit/be501ebef8b02f724977f6a1cf4589beab79366c))
* moved interface ([1e176e0](https://github.com/humlab-sead/sead_query_api/commit/1e176e0dc12ae9b1ccfb0f5c5391a4c27692ea9a))
* moved interface file ([ac8c933](https://github.com/humlab-sead/sead_query_api/commit/ac8c9333aced465ef36d6caa5305f41783cefed7))
* Moved logger init code to separate file ([ab47c94](https://github.com/humlab-sead/sead_query_api/commit/ab47c94acf4ece28e2430639d6a7880b14edf6a7))
* moved logic to class ([4c4f744](https://github.com/humlab-sead/sead_query_api/commit/4c4f744532d7c2a66bddcff772e5e957577ab228))
* moved methods ([3730f09](https://github.com/humlab-sead/sead_query_api/commit/3730f0992a3711de3d31b64790b49bb7ed8e9626))
* name change ([863a412](https://github.com/humlab-sead/sead_query_api/commit/863a4127911d69e32a704b2221dae8c65b32b66f))
* Name change ([5f40918](https://github.com/humlab-sead/sead_query_api/commit/5f4091841e281a84152b4ef58abadecf97aa9035))
* Name change ([cdaad6a](https://github.com/humlab-sead/sead_query_api/commit/cdaad6afa28a4e71b929706e6abf2a36b0a9bc02))
* name changes ([3fecec7](https://github.com/humlab-sead/sead_query_api/commit/3fecec7b5af594f02aa63cc9e40357cd646fc0a9))
* name changes, variable removal ([e49c8b7](https://github.com/humlab-sead/sead_query_api/commit/e49c8b79be90b827403486e0919a589420b1acb9))
* namespace cleanup/rename ([9591458](https://github.com/humlab-sead/sead_query_api/commit/9591458d82e277fc9e7b4d7ca3132e5707fa439b))
* namespace cleanup/rename ([83c73b1](https://github.com/humlab-sead/sead_query_api/commit/83c73b180f888bcc70db5ce8e4aeb4c3a13514a7))
* namespace cleanup/rename ([0fe481b](https://github.com/humlab-sead/sead_query_api/commit/0fe481b7bd8e19a3abc0f0ecc84cbd188334ad12))
* ranmed extension method ([820a215](https://github.com/humlab-sead/sead_query_api/commit/820a215a4b27e7d1ab7ae23681baca5afc170e90))
* reduce indentation ([731b5ff](https://github.com/humlab-sead/sead_query_api/commit/731b5ff15c1a33673ac6b022f0c0b87e9f25d158))
* Remove unnecessary files.exclude settings in .vscode/settings.json ([b381658](https://github.com/humlab-sead/sead_query_api/commit/b381658e537964f3adfbf6067986b6d11f2f86ad))
* Remove unnecessary usings ([343e81a](https://github.com/humlab-sead/sead_query_api/commit/343e81a22eee7d13fe7edf0bbbd2deb21e35436c))
* Remove unnecessary usings and update test data ([08dd870](https://github.com/humlab-sead/sead_query_api/commit/08dd87018e9af95657113d0a0a8f45618cb4fb9c))
* remove unused RepositoryTests class ([60c4e73](https://github.com/humlab-sead/sead_query_api/commit/60c4e73f9a637a5d2929e70ac56ee418b9f4860b))
* Removed deprecated code ([7c11472](https://github.com/humlab-sead/sead_query_api/commit/7c114722687d5bf40800cfac0fe5b45c630eb828))
* removed private method ([9c9fb7c](https://github.com/humlab-sead/sead_query_api/commit/9c9fb7cc4923336bac4a6b1496f32e4c971dfc0b))
* Removed unused API elements ([909ca60](https://github.com/humlab-sead/sead_query_api/commit/909ca60799e2b557d63046af40976b6e69695637))
* Removed unused API elements ([90f8902](https://github.com/humlab-sead/sead_query_api/commit/90f89029ef2347f8cc6fe54815f25758df5315ae))
* rename ([16bcb9b](https://github.com/humlab-sead/sead_query_api/commit/16bcb9b6dee88f58abb69c538dc3ae61bae31842))
* rename, file sorting ([6045d68](https://github.com/humlab-sead/sead_query_api/commit/6045d684944f3ce628d2b11877b9033c250dc225))
* rename+shortcut ([ada536c](https://github.com/humlab-sead/sead_query_api/commit/ada536ce8b6af07d654acbee4fd41b5394870d06))
* Renamed and simplified interface ([68e0e86](https://github.com/humlab-sead/sead_query_api/commit/68e0e86be4685805816ce5fc1836544128093317))
* renamed facet code ([f707786](https://github.com/humlab-sead/sead_query_api/commit/f7077862626952a12617b4c30b81a7127e7cff44))
* renamed file ([5edfeea](https://github.com/humlab-sead/sead_query_api/commit/5edfeea1e4ef6e721b0145534bdbef27f5512c37))
* renamed files ([18a1302](https://github.com/humlab-sead/sead_query_api/commit/18a1302a52e7176f17aff40c4785c39cec044c48))
* renamed folder ([813952f](https://github.com/humlab-sead/sead_query_api/commit/813952ff1b18701cdc4cb198f3979e8966fb14bc))
* renamed items ([24941f6](https://github.com/humlab-sead/sead_query_api/commit/24941f643ff9eaf1897ee4cef58b5dcba4ca26b1))
* renamed namespace ([534bb12](https://github.com/humlab-sead/sead_query_api/commit/534bb1261d02db5b39d71de30fa75b1e6bcc1f09))
* renamed namespace ([71bdd2b](https://github.com/humlab-sead/sead_query_api/commit/71bdd2b8e6753bdfafbe763ebfd6a5a2de7e3f91))
* renamed parameter, moved property ([7acfda3](https://github.com/humlab-sead/sead_query_api/commit/7acfda39e60b028a2919271417bbed51a887e396))
* Renamed property ([bbde027](https://github.com/humlab-sead/sead_query_api/commit/bbde0271914451e2549638785179018312cbaea3))
* renamed repositories, expose context ([6b78105](https://github.com/humlab-sead/sead_query_api/commit/6b7810559837092391c5226712538af5aaf076d5))
* renamed repository, add medthod ([1b4f752](https://github.com/humlab-sead/sead_query_api/commit/1b4f752da1e6fa41711e9f0eff3375740bfac80c))
* renamed variable ([acb146d](https://github.com/humlab-sead/sead_query_api/commit/acb146d0e42ef48f662a88af80e635aa6e0c2fc5))
* renames ([4af1453](https://github.com/humlab-sead/sead_query_api/commit/4af1453f30e5382805c0ec1256bf14c3b4db518f))
* Renames ([a761cd5](https://github.com/humlab-sead/sead_query_api/commit/a761cd5cf09ad4f0420a1ccc61a5b9b918b654e8))
* renames & simplifications ([d2905be](https://github.com/humlab-sead/sead_query_api/commit/d2905be946fd33364f1dc3b3bac8af0bd2e5bc0d))
* renames and code cleanups ([54ab762](https://github.com/humlab-sead/sead_query_api/commit/54ab76214c5962916d0627c8ef3b1dbf1d3b1b15))
* RepositoryRegistry ([9aab597](https://github.com/humlab-sead/sead_query_api/commit/9aab597d1f514584ff311dc74bc6ab93de5fe44b))
* review/cleanup ([883b925](https://github.com/humlab-sead/sead_query_api/commit/883b9250270da912845c2153a515b111f9dbb638))
* Simplified FacetGraph logic ([c831ebd](https://github.com/humlab-sead/sead_query_api/commit/c831ebd766acf6a2df8472d1a269db2ed35cdcae))
* simplified IEdge interface ([93507f2](https://github.com/humlab-sead/sead_query_api/commit/93507f2cd0fa23dc8a2d86cab791935dae6b3381))
* Simplified ResultService ([4c8de2a](https://github.com/humlab-sead/sead_query_api/commit/4c8de2ad1a18526b584e96cd5fab9b0a8f19e59c))
* Simplify DijkstrasGraph constructor ([177faf3](https://github.com/humlab-sead/sead_query_api/commit/177faf3700324186a7e199fca8e806c32cd8a36d))
* simplify list initialization ([352834a](https://github.com/humlab-sead/sead_query_api/commit/352834ab3dbe65ef9181b9321fd495a94d8d4609))
* Split into class files ([fd053e2](https://github.com/humlab-sead/sead_query_api/commit/fd053e2339dbf910154033a8afed1bcc9b321b40))
* Split into separate files ([36ea12f](https://github.com/humlab-sead/sead_query_api/commit/36ea12fe9a45d5e69fd81c959cae6015b54aca08))
* switch operande in range expression ([8e375f7](https://github.com/humlab-sead/sead_query_api/commit/8e375f7e519a4c3e54fc6313914c906209daf8aa))
* switch range operators ([f40b48d](https://github.com/humlab-sead/sead_query_api/commit/f40b48d68d79b4f2f7b1536d79b7ae64d5efa010))
* test code cleanups ([782c3a8](https://github.com/humlab-sead/sead_query_api/commit/782c3a8f535ba7420e5970825bf47daa563de615))
* Update CountFieldCompilerTests.cs to include distinct expression test case ([2450e80](https://github.com/humlab-sead/sead_query_api/commit/2450e80def3c60d55413d24b1558834d9da43dd2))
* update editor semantic token colors ([50360b3](https://github.com/humlab-sead/sead_query_api/commit/50360b3e172bf8a46d7779fb238bc218a55a7b0a))
* Update facet configurations to remove unnecessary "textFilter" property ([16a9d50](https://github.com/humlab-sead/sead_query_api/commit/16a9d5082546f08c18cc06005056b0e4ea793f6a))
* Update FacetGraph class to use container naming convention ([b637500](https://github.com/humlab-sead/sead_query_api/commit/b637500e16f7558ce891541f8ebd381fd5801833))
* Update FacetLoadService to use UriToFacetsConfig for loading facets ([c28e789](https://github.com/humlab-sead/sead_query_api/commit/c28e789b7384030c61962dcc54b3e8d72f5dd769))
* Update FacetLoadService.cs to include missing test cases ([ec9817f](https://github.com/humlab-sead/sead_query_api/commit/ec9817f955a58f767c0002bf047485c511b1137a))
* Update IGraphNode interface and related classes ([1d89a55](https://github.com/humlab-sead/sead_query_api/commit/1d89a55005c70d7f779492a742041b756d2afa33))
* Update intervalCount default value to 50 in CategoryInfoService.cs ([b4249bf](https://github.com/humlab-sead/sead_query_api/commit/b4249bfbc41ccf0ad48a10b233b4393a34defcbc))
* Update JoinsClauseCompiler to use primary constructor ([c79bc6b](https://github.com/humlab-sead/sead_query_api/commit/c79bc6bf8c740c7c5311c62c3538faf0800726b4))
* Update repository constructors to use RepositoryRegistry instead of IFacetContext ([5dbd815](https://github.com/humlab-sead/sead_query_api/commit/5dbd81511dc244166fae0e1a4add5657377cbf18))

### ✅ Tests

* Scaffold script updates ([81db9b5](https://github.com/humlab-sead/sead_query_api/commit/81db9b5e11583300d2630d5c95ed4291fc931ad2))

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
