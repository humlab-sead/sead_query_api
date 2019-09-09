using DataAccessPostgreSqlProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SeadQueryCore;
using Newtonsoft.Json;
using SeadQueryTest.Infrastructure.Scaffolding;
using System.IO;

namespace SeadQueryTest.Infrastructure
{
    public abstract class FacetContextInitializerBase
    {
        public abstract void Initialize(FacetContext context);
        public abstract void Seed(FacetContext context);
    }

    public class FacetContextDefaultInitializer : FacetContextInitializerBase
    {
        public override void Initialize(FacetContext context)
        {
            if (context.FacetDefinitions.Any()) {
                return;
            }

            Seed(context);
        }

        //public override void Seed(FacetContext context)
        //{
        //    context.FacetGroups.AddRange(FacetContextDefaultData.FacetGroups());
        //    context.FacetTypes.AddRange(FacetContextDefaultData.FacetTypes());

        //    context.SaveChanges();
        //}

        public override void Seed(FacetContext context)
        {
            throw new NotSupportedException("This class has been deprecated. Use FacetContentFixture instead.");
            //JsonSerializer serializer = new JsonSerializer {
            //    // serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //    NullValueHandling = NullValueHandling.Ignore
            //};

            //var path = Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data");
            //var reader = new ScaffoldReader();

            //context.AttachRange(reader.Deserialize<Facet>(path));
            //context.AttachRange(reader.Deserialize<FacetClause>(path));
            //context.AttachRange(reader.Deserialize<FacetGroup>(path));
            //context.AttachRange(reader.Deserialize<FacetTable>(path));
            //context.AttachRange(reader.Deserialize<FacetType>(path));
            //context.AttachRange(reader.Deserialize<GraphTable>(path));
            //context.AttachRange(reader.Deserialize<GraphTableRelation>(path));
            //context.AttachRange(reader.Deserialize<ResultAggregate>(path));
            //context.AttachRange(reader.Deserialize<ResultAggregateField>(path));
            //context.AttachRange(reader.Deserialize<ResultField>(path));
            //context.AttachRange(reader.Deserialize<ResultFieldType>(path));
            //context.AttachRange(reader.Deserialize<ResultViewType>(path));
            //context.AttachRange(reader.Deserialize<ViewState>(path));
        }

    }

    //public static class FacetContextDefaultData
    //{
    //    public static IEnumerable<FacetGroup> FacetGroups()
    //    {
    //        return new List<(int, string, string, bool, bool)>() {
    //            (0, "ROOT", "ROOT", false, false),
    //            (1, "others", "Others", true, false),
    //            (2, "space_time", "Space/Time", true, false),
    //            (3, "time", "Time", true, false),
    //            (4, "ecology", "Ecology", true, false),
    //            (5, "measured_values", "Measured values", true, false),
    //            (6, "taxonomy", "Taxonomy", true, false)
    //        }.Select(
    //            x => new FacetGroup {
    //                FacetGroupId = x.Item1,
    //                FacetGroupKey = x.Item2,
    //                DisplayTitle = x.Item3,
    //                IsApplicable = x.Item4,
    //                IsDefault = x.Item5
    //            }
    //        );
    //    }

    //    public static IEnumerable<FacetType> FacetTypes()
    //    {
    //        return new List<(int, string, bool)>() {
    //            (0, "undefined", false),
    //            (1, "discrete", false),
    //            (2, "range", true),
    //            (3, "geo", true)
    //        }.Select(
    //            x => new FacetType {
    //                FacetTypeId = (EFacetType)x.Item1,
    //                FacetTypeName = x.Item2,
    //                ReloadAsTarget = x.Item3
    //            }
    //        );
    //    }
    //    public static IEnumerable<Facet> Facets()
    //    {
    //        return new List<(int, string, string, int, int, string, string, string, string, bool, bool, string, string, int)>() {
    //            (1, "result_facet", "Analysis entities", 0, 1, "tbl_analysis_entities.analysis_entity_id", "tbl_physical_samples.sample_name||'' ''||tbl_datasets.dataset_name", "tbl_analysis_entities.analysis_entity_id", "tbl_datasets.dataset_name", false, false, "count", "Number of samples", 0),
    //            (2, "dataset_helper", "dataset_helper", 0, 1, "tbl_datasets.dataset_id", "tbl_datasets.dataset_id", "tbl_dataset.dataset_id", "tbl_dataset.dataset_id", false, false, "count", "Number of samples", 1),
    //            //(25, "species", "Taxa", 6, 1, "tbl_taxa_tree_master.taxon_id", "concat_ws('' '", tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)", "tbl_taxa_tree_master.taxon_id", "tbl_taxa_tree_genera.genus_name||'' ''||tbl_taxa_tree_master.species", true, false, "sum", "sum of Abundance", 32),
    //            (35, "tbl_biblio_modern", "Bibligraphy modern", 1, 1, "metainformation.view_taxa_biblio.biblio_id", "tbl_biblio.title||''  ''||tbl_biblio.author ", "tbl_biblio.biblio_id", "tbl_biblio.author", true, false, "count", "count of species", 19),
    //            (3, "tbl_denormalized_measured_values_33_0", "MS ", 5, 2, "metainformation.tbl_denormalized_measured_values.value_33_0", "metainformation.tbl_denormalized_measured_values.value_33_0", "metainformation.tbl_denormalized_measured_values.value_33_0", "metainformation.tbl_denormalized_measured_values.value_33_0", true, false, "", "Number of samples", 1),
    //            (4, "tbl_denormalized_measured_values_33_82", "MS Heating 550", 5, 2, "metainformation.tbl_denormalized_measured_values.value_33_82", "metainformation.tbl_denormalized_measured_values.value_33_82", "metainformation.tbl_denormalized_measured_values.value_33_82", "metainformation.tbl_denormalized_measured_values.value_33_82", true, false, "", "Number of samples", 1),
    //            (5, "tbl_denormalized_measured_values_32", "LOI", 5, 2, "metainformation.tbl_denormalized_measured_values.value_32_0", "metainformation.tbl_denormalized_measured_values.value_32_0", "metainformation.tbl_denormalized_measured_values.value_32", "metainformation.tbl_denormalized_measured_values.value_32", true, false, "", "Number of samples", 1),
    //            (6, "tbl_denormalized_measured_values_37", " P┬░", 5, 2, "metainformation.tbl_denormalized_measured_values.value_37_0", "metainformation.tbl_denormalized_measured_values.value_37_0", "metainformation.tbl_denormalized_measured_values.value_37", "metainformation.tbl_denormalized_measured_values.value_37", true, false, "", "Number of samples", 1),
    //            (7, "measured_values_helper", "values", 0, 1, "tbl_measured_values.measured_value", "tbl_measured_values.measured_value", "tbl_measured_values.measured_value", "tbl_measured_values.measured_value", false, false, "count", "Number of samples", 1),
    //            (8, "taxon_result", "taxon_id", 0, 1, "tbl_abundances.taxon_id", "tbl_abundances.taxon_id", "tbl_abundances.taxon_id", "tbl_abundances.taxon_id", false, false, "count", "Number of samples", 1),
    //            (9, "map_result", "Site", 0, 1, "tbl_sites.site_id", "tbl_sites.site_name", "tbl_sites.site_id", "tbl_sites.site_name", false, false, "count", "Number of samples", 1),
    //            (12, "record_types", "Proxy types", 1, 1, "tbl_record_types.record_type_id", "tbl_record_types.record_type_name", "tbl_record_types.record_type_id", "tbl_record_types.record_type_name", true, false, "count", "Number of samples", 1),
    //            (13, "sample_groups", "Sample group", 2, 1, "tbl_sample_groups.sample_group_id", "tbl_sample_groups.sample_group_name", "tbl_sample_groups.sample_group_id", "tbl_sample_groups.sample_group_name", true, true, "count", "Number of samples", 1),
    //            (14, "places", "Places", 2, 1, "tbl_locations.location_id", "tbl_locations.location_name", "tbl_locations.location_id", "tbl_locations.location_name", false, true, "count", "Number of samples", 1),
    //            (15, "places_all2", "view_places_relations", 2, 1, "tbl_locations.location_id", "tbl_locations.location_name", "view_places_relations.rel_id", "tbl_locations.location_name", false, true, "count", "Number of samples", 1),
    //            (16, "sample_groups_helper", "Sample group", 2, 1, "tbl_sample_groups.sample_group_id", "tbl_sample_groups.sample_group_name", "tbl_sample_groups.sample_group_id", "tbl_sample_groups.sample_group_name", false, true, "count", "Number of samples", 1),
    //            (17, "physical_samples", "physical samples", 2, 1, "tbl_physical_samples.physical_sample_id", "tbl_physical_samples.sample_name", "tbl_physical_samples.physical_sample_id", "tbl_physical_samples.sample_name", false, true, "count", "Number of samples", 1),
    //            (18, "sites", "Site", 2, 1, "tbl_sites.site_id", "tbl_sites.site_name", "tbl_sites.site_id", "tbl_sites.site_name", true, true, "count", "Number of samples", 1),
    //            (19, "sites_helper", "Site", 2, 1, "tbl_sites.site_id", "tbl_sites.site_name", "tbl_sites.site_id", "tbl_sites.site_name", false, true, "count", "Number of samples", 1),
    //            (21, "country", "Country", 2, 1, "countries.location_id", "countries.location_name ", "countries.location_id", "countries.location_name", true, false, "count", "Number of samples", 1),
    //            (22, "ecocode", "Eco code", 4, 1, "tbl_ecocode_definitions.ecocode_definition_id", "tbl_ecocode_definitions.label", "tbl_ecocode_definitions.ecocode_definition_id", "tbl_ecocode_definitions.label", true, false, "count", "Number of samples", 1),
    //            (23, "family", "Family", 6, 1, "tbl_taxa_tree_families.family_id", "tbl_taxa_tree_families.family_name ", "tbl_taxa_tree_families.family_id", "tbl_taxa_tree_families.family_name ", true, false, "count", "Number of samples", 1),
    //            (24, "genus", "Genus", 6, 1, "tbl_taxa_tree_genera.genus_id", "tbl_taxa_tree_genera.genus_name", "tbl_taxa_tree_genera.genus_id", "tbl_taxa_tree_genera.genus_name", true, false, "count", "Number of samples", 1),
    //            (26, "species_helper", "Species", 6, 1, "tbl_taxa_tree_master.taxon_id", "tbl_taxa_tree_master.taxon_id", "tbl_taxa_tree_master.taxon_id", "tbl_taxa_tree_master.species", false, false, "count", "Number of samples", 1),
    //            (27, "abundance_helper", "abundance_id", 6, 1, "tbl_abundances.abundance_id", "tbl_abundances.abundance_id", "tbl_abundances.abundance_id", "tbl_abundances.abundance_id", false, false, "count", "Number of samples", 1),
    //            (28, "species_author", "Author", 6, 1, "tbl_taxa_tree_authors.author_id ", "tbl_taxa_tree_authors.author_name ", "tbl_taxa_tree_authors.author_id ", "tbl_taxa_tree_authors.author_name ", true, false, "count", "Number of samples", 1),
    //            (29, "feature_type", "Feature type", 1, 1, "tbl_feature_types.feature_type_id ", "tbl_feature_types.feature_type_name", "tbl_feature_types.feature_id ", "tbl_feature_types.feature_type_name", true, false, "count", "Number of samples", 1),
    //            (30, "ecocode_system", "Eco code system", 4, 1, "tbl_ecocode_systems.ecocode_system_id ", "tbl_ecocode_systems.name", "tbl_ecocode_systems.ecocode_system_id ", "tbl_ecocode_systems.definition", true, false, "count", "Number of samples", 1),
    //            (31, "abundance_classification", "abundance classification", 4, 1, "metainformation.view_abundance.elements_part_mod ", "metainformation.view_abundance.elements_part_mod ", "metainformation.view_abundance.elements_part_mod ", "metainformation.view_abundance.elements_part_mod ", true, false, "count", "Number of samples", 1),
    //            (10, "geochronology", "Geochronology", 2, 2, "tbl_geochronology.age", "tbl_geochronology.age", "tbl_geochronology.age", "tbl_geochronology.age", true, false, "", "Number of samples", 1),
    //            (11, "relative_age_name", "Time periods", 2, 1, "tbl_relative_ages.relative_age_id", "tbl_relative_ages.relative_age_name", "tbl_relative_ages.relative_age_id", "tbl_relative_ages.relative_age_name", true, false, "count", "Number of samples", 1),
    //            (20, "tbl_relative_dates_helper", "tbl_relative_dates", 2, 1, "tbl_relative_dates.relative_age_id", "tbl_relative_dates.relative_age_name ", "tbl_relative_dates.relative_age_name", "tbl_relative_dates.relative_age_name ", false, false, "count", "Number of samples", 1),
    //            (34, "activeseason", "Seasons", 2, 1, "tbl_seasons.season_id", "tbl_seasons.season_name ", "tbl_seasons.season_id", "tbl_seasons.season_type ", true, false, "count", "Number of samples", 1),
    //            (32, "abundances_all_helper", "Abundances", 4, 2, "metainformation.view_abundance.abundance ", "metainformation.view_abundance.abundance ", "metainformation.view_abundance.abundance ", "metainformation.view_abundance.abundance", false, false, "", "Number of samples", 1),
    //            (33, "abundances_all", "Abundances", 4, 2, "metainformation.view_abundance.abundance ", "metainformation.view_abundance.abundance ", "metainformation.view_abundance.abundance ", "metainformation.view_abundance.abundance", true, false, "", "Number of samples", 1),
    //            (36, "tbl_biblio_sample_groups", "Bibligraphy sites/Samplegroups", 1, 1, "tbl_biblio.biblio_id", "tbl_biblio.title||''  ''||tbl_biblio.author", "tbl_biblio.biblio_id", "tbl_biblio.author", true, false, "count", "Number of samples", 1),
    //            (37, "tbl_biblio_sites", "Bibligraphy sites", 1, 1, "tbl_biblio.biblio_id", "tbl_biblio.title||''  ''||tbl_biblio.author", "tbl_biblio.biblio_id", "tbl_biblio.author", false, false, "count", "Number of samples", 1)
    //        }.Select(
    //            x => new Facet {
    //                FacetId = x.Item1,
    //                FacetCode = x.Item2,
    //                DisplayTitle = x.Item3,
    //                FacetGroupId = x.Item4,
    //                FacetTypeId = (EFacetType)x.Item5,
    //                CategoryIdExpr = x.Item6,
    //                CategoryNameExpr = x.Item7,
    //                IconIdExpr = x.Item8,
    //                SortExpr = x.Item9,
    //                IsApplicable = x.Item10,
    //                IsDefault = x.Item11,
    //                AggregateType = x.Item12,
    //                AggregateTitle = x.Item13,
    //                AggregateFacetId = x.Item14
    //            }
    //        );
    //    }
    //}
}
