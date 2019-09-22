using System;
using System.Collections.Generic;
using System.Text;
using SeadQueryCore;

namespace SeadQueryTest.Infrastructure.Scaffolds
{
    public static class FacetInstances
    {
        public static Dictionary<string, Facet> Store = new Dictionary<string, Facet>() {
            { "result_facet", new Facet() {
                FacetId = 1,
                FacetCode = "result_facet",
                DisplayTitle = "Analysis entities",
                FacetGroupId = 999,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                CategoryNameExpr = "tbl_physical_samples.sample_name||\' \'||tbl_datasets.dataset_name",
                IconIdExpr = "tbl_analysis_entities.analysis_entity_id",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 0,
                SortExpr = "tbl_datasets.dataset_name",
                FacetType = new FacetType {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup {
                    FacetGroupId = 999,
                    DisplayTitle = "ROOT",
                    IsApplicable = false,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 1,
                        FacetId = 1,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_analysis_entities",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 38,
                        FacetId = 1,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_physical_samples",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 53,
                        FacetId = 1,
                        SequenceId = 3,
                        SchemaName = "",
                        ObjectName = "tbl_datasets",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> { }
                } },
            { "dataset_helper", new Facet() {

                FacetId = 2,
                FacetCode = "dataset_helper",
                DisplayTitle = "dataset_helper",
                FacetGroupId = 999,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_datasets.dataset_id",
                CategoryNameExpr = "tbl_datasets.dataset_id",
                IconIdExpr = "tbl_dataset.dataset_id",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_dataset.dataset_id",
                FacetType = new FacetType {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup {
                    FacetGroupId = 999,
                    DisplayTitle = "ROOT",
                    IsApplicable = false,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 2,
                        FacetId = 2,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_datasets",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> {
                }
                } },
            { "tbl_denormalized_measured_values_33_0", new Facet() {

                FacetId = 3,
                FacetCode = "tbl_denormalized_measured_values_33_0",
                DisplayTitle = "MS ",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "method_values.measured_value",
                CategoryNameExpr = "method_values.measured_value",
                IconIdExpr = "method_values.measured_value",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "method_values.measured_value",
                FacetType = new FacetType {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 3,
                        FacetId = 3,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "facet.method_measured_values",
                        ObjectArgs = "(33, 0)",
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> {
                }
                } },
            { "tbl_denormalized_measured_values_33_82", new Facet() {

                FacetId = 4,
                FacetCode = "tbl_denormalized_measured_values_33_82",
                DisplayTitle = "MS Heating 550",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "metainformation.tbl_denormalized_measured_values.value_33_82",
                CategoryNameExpr = "metainformation.tbl_denormalized_measured_values.value_33_82",
                IconIdExpr = "metainformation.tbl_denormalized_measured_values.value_33_82",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "metainformation.tbl_denormalized_measured_values.value_33_82",
                FacetType = new FacetType {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 4,
                        FacetId = 4,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "metainformation.tbl_denormalized_measured_values",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> {
                }
                } },
            { "tbl_denormalized_measured_values_32", new Facet() {

                FacetId = 5,
                FacetCode = "tbl_denormalized_measured_values_32",
                DisplayTitle = "LOI",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "metainformation.tbl_denormalized_measured_values.value_32_0",
                CategoryNameExpr = "metainformation.tbl_denormalized_measured_values.value_32_0",
                IconIdExpr = "metainformation.tbl_denormalized_measured_values.value_32",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "metainformation.tbl_denormalized_measured_values.value_32",
                FacetType = new FacetType {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 5,
                        FacetId = 5,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "metainformation.tbl_denormalized_measured_values",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> {
                }
                } },
            { "tbl_denormalized_measured_values_37", new Facet() {

                FacetId = 6,
                FacetCode = "tbl_denormalized_measured_values_37",
                DisplayTitle = " P┬░",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "metainformation.tbl_denormalized_measured_values.value_37_0",
                CategoryNameExpr = "metainformation.tbl_denormalized_measured_values.value_37_0",
                IconIdExpr = "metainformation.tbl_denormalized_measured_values.value_37",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "metainformation.tbl_denormalized_measured_values.value_37",
                FacetType = new FacetType {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 6,
                        FacetId = 6,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "metainformation.tbl_denormalized_measured_values",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> {
                }
                } },
            { "measured_values_helper", new Facet() {

                FacetId = 7,
                FacetCode = "measured_values_helper",
                DisplayTitle = "values",
                FacetGroupId = 999,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_measured_values.measured_value",
                CategoryNameExpr = "tbl_measured_values.measured_value",
                IconIdExpr = "tbl_measured_values.measured_value",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_measured_values.measured_value",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 999,
                    DisplayTitle = "ROOT",
                    IsApplicable = false,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 7,
                        FacetId = 7,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_measured_values",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                } },
            { "taxon_result", new Facet() {
                FacetId = 8,
                FacetCode = "taxon_result",
                DisplayTitle = "taxon_id",
                FacetGroupId = 999,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_abundances.taxon_id",
                CategoryNameExpr = "tbl_abundances.taxon_id",
                IconIdExpr = "tbl_abundances.taxon_id",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_abundances.taxon_id",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 999,
                    DisplayTitle = "ROOT",
                    IsApplicable = false,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 8,
                        FacetId = 8,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_abundances",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> {}
                } },
            { "map_result", new Facet() {
                FacetId = 9,
                FacetCode = "map_result",
                DisplayTitle = "Site",
                FacetGroupId = 999,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sites.site_id",
                CategoryNameExpr = "tbl_sites.site_name",
                IconIdExpr = "tbl_sites.site_id",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_sites.site_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 999,
                    DisplayTitle = "ROOT",
                    IsApplicable = false,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 9,
                        FacetId = 9,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_sites",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> { }
                } },
            { "geochronology", new Facet() {
                FacetId = 10,
                FacetCode = "geochronology",
                DisplayTitle = "Geochronology",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "tbl_geochronology.age",
                CategoryNameExpr = "tbl_geochronology.age",
                IconIdExpr = "tbl_geochronology.age",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_geochronology.age",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 10,
                        FacetId = 10,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_geochronology",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause> { }
                } },
            { "relative_age_name", new Facet() {

                FacetId = 11,
                FacetCode = "relative_age_name",
                DisplayTitle = "Time periods",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_relative_ages.relative_age_id",
                CategoryNameExpr = "tbl_relative_ages.relative_age_name",
                IconIdExpr = "tbl_relative_ages.relative_age_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_relative_ages.relative_age_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 11,
                        FacetId = 11,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_relative_ages",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                } },
            { "record_types", new Facet() {
                FacetId = 12,
                FacetCode = "record_types",
                DisplayTitle = "Proxy types",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_record_types.record_type_id",
                CategoryNameExpr = "tbl_record_types.record_type_name",
                IconIdExpr = "tbl_record_types.record_type_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_record_types.record_type_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 1,
                    DisplayTitle = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 12,
                        FacetId = 12,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_record_types",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                } },
            { "sample_groups", new Facet() {

                FacetId = 13,
                FacetCode = "sample_groups",
                DisplayTitle = "Sample group",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sample_groups.sample_group_id",
                CategoryNameExpr = "tbl_sample_groups.sample_group_name",
                IconIdExpr = "tbl_sample_groups.sample_group_id",
                IsApplicable = true,
                IsDefault = true,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_sample_groups.sample_group_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 13,
                        FacetId = 13,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_sample_groups",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                } },
            { "places", new Facet() {
                FacetId = 14,
                FacetCode = "places",
                DisplayTitle = "Places",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_locations.location_id",
                CategoryNameExpr = "tbl_locations.location_name",
                IconIdExpr = "tbl_locations.location_id",
                IsApplicable = false,
                IsDefault = true,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_locations.location_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 14,
                        FacetId = 14,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_locations",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 39,
                        FacetId = 14,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_site_locations",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "places_all2", new Facet() {

                FacetId = 15,
                FacetCode = "places_all2",
                DisplayTitle = "view_places_relations",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_locations.location_id",
                CategoryNameExpr = "tbl_locations.location_name",
                IconIdExpr = "view_places_relations.rel_id",
                IsApplicable = false,
                IsDefault = true,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_locations.location_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 15,
                        FacetId = 15,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "view_places_relations",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 40,
                        FacetId = 15,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_locations",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 54,
                        FacetId = 15,
                        SequenceId = 3,
                        SchemaName = "",
                        ObjectName = "tbl_site_locations",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "sample_groups_helper", new Facet() {

                FacetId = 16,
                FacetCode = "sample_groups_helper",
                DisplayTitle = "Sample group",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sample_groups.sample_group_id",
                CategoryNameExpr = "tbl_sample_groups.sample_group_name",
                IconIdExpr = "tbl_sample_groups.sample_group_id",
                IsApplicable = false,
                IsDefault = true,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_sample_groups.sample_group_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 16,
                        FacetId = 16,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_sample_groups",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "physical_samples", new Facet() {

                FacetId = 17,
                FacetCode = "physical_samples",
                DisplayTitle = "physical samples",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_physical_samples.physical_sample_id",
                CategoryNameExpr = "tbl_physical_samples.sample_name",
                IconIdExpr = "tbl_physical_samples.physical_sample_id",
                IsApplicable = false,
                IsDefault = true,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_physical_samples.sample_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 17,
                        FacetId = 17,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_physical_samples",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "sites", new Facet() {

                FacetId = 18,
                FacetCode = "sites",
                DisplayTitle = "Site",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sites.site_id",
                CategoryNameExpr = "tbl_sites.site_name",
                IconIdExpr = "tbl_sites.site_id",
                IsApplicable = true,
                IsDefault = true,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_sites.site_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 18,
                        FacetId = 18,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_sites",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "sites_helper", new Facet() {

                FacetId = 19,
                FacetCode = "sites_helper",
                DisplayTitle = "Site",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sites.site_id",
                CategoryNameExpr = "tbl_sites.site_name",
                IconIdExpr = "tbl_sites.site_id",
                IsApplicable = false,
                IsDefault = true,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_sites.site_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 19,
                        FacetId = 19,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_sites",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "tbl_relative_dates_helper", new Facet() {

                FacetId = 20,
                FacetCode = "tbl_relative_dates_helper",
                DisplayTitle = "tbl_relative_dates",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_relative_dates.relative_age_id",
                CategoryNameExpr = "tbl_relative_dates.relative_age_name ",
                IconIdExpr = "tbl_relative_dates.relative_age_name",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_relative_dates.relative_age_name ",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 20,
                        FacetId = 20,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_relative_dates",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "country", new Facet() {

                FacetId = 21,
                FacetCode = "country",
                DisplayTitle = "Country",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "countries.location_id",
                CategoryNameExpr = "countries.location_name ",
                IconIdExpr = "countries.location_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "countries.location_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 21,
                        FacetId = 21,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_locations",
                        ObjectArgs = null,
                        Alias = "countries"
                    },
                    new FacetTable
                    {
                        FacetTableId = 41,
                        FacetId = 21,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_site_locations",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetSourceTableId = 1,
                        FacetId = 21,
                        Clause = "countries.location_type_id=1"
                    }
                }
                }},
            { "ecocode", new Facet() {

                FacetId = 22,
                FacetCode = "ecocode",
                DisplayTitle = "Eco code",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_ecocode_definitions.ecocode_definition_id",
                CategoryNameExpr = "tbl_ecocode_definitions.label",
                IconIdExpr = "tbl_ecocode_definitions.ecocode_definition_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_ecocode_definitions.label",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 4,
                    DisplayTitle = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 22,
                        FacetId = 22,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_ecocode_definitions",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 42,
                        FacetId = 22,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_ecocode_definitions",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "family", new Facet() {

                FacetId = 23,
                FacetCode = "family",
                DisplayTitle = "Family",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_families.family_id",
                CategoryNameExpr = "tbl_taxa_tree_families.family_name ",
                IconIdExpr = "tbl_taxa_tree_families.family_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_taxa_tree_families.family_name ",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 6,
                    DisplayTitle = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 23,
                        FacetId = 23,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_families",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 43,
                        FacetId = 23,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_families",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
            { "genus", new Facet() {

                FacetId = 24,
                FacetCode = "genus",
                DisplayTitle = "Genus",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_genera.genus_id",
                CategoryNameExpr = "tbl_taxa_tree_genera.genus_name",
                IconIdExpr = "tbl_taxa_tree_genera.genus_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_taxa_tree_genera.genus_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 6,
                    DisplayTitle = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 24,
                        FacetId = 24,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_genera",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 44,
                        FacetId = 24,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_genera",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "species", new Facet() {

                FacetId = 25,
                FacetCode = "species",
                DisplayTitle = "Taxa",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_master.taxon_id",
                CategoryNameExpr = "concat_ws(\' \', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)",
                IconIdExpr = "tbl_taxa_tree_master.taxon_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "sum",
                AggregateTitle = "sum of Abundance",
                AggregateFacetId = 32,
                SortExpr = "tbl_taxa_tree_genera.genus_name||\' \'||tbl_taxa_tree_master.species",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 6,
                    DisplayTitle = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 25,
                        FacetId = 25,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_master",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 45,
                        FacetId = 25,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_genera",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 57,
                        FacetId = 25,
                        SequenceId = 4,
                        SchemaName = "",
                        ObjectName = "tbl_sites",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 55,
                        FacetId = 25,
                        SequenceId = 3,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_authors",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetSourceTableId = 2,
                        FacetId = 25,
                        Clause = "tbl_sites.site_id is not null"
                    }
                }
                }},
			{ "species_helper", new Facet() {

                FacetId = 26,
                FacetCode = "species_helper",
                DisplayTitle = "Species",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_master.taxon_id",
                CategoryNameExpr = "tbl_taxa_tree_master.taxon_id",
                IconIdExpr = "tbl_taxa_tree_master.taxon_id",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_taxa_tree_master.species",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 6,
                    DisplayTitle = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 26,
                        FacetId = 26,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_master",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 46,
                        FacetId = 26,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_genera",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 56,
                        FacetId = 26,
                        SequenceId = 3,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_authors",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "abundance_helper", new Facet() {

                FacetId = 27,
                FacetCode = "abundance_helper",
                DisplayTitle = "abundance_id",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_abundances.abundance_id",
                CategoryNameExpr = "tbl_abundances.abundance_id",
                IconIdExpr = "tbl_abundances.abundance_id",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_abundances.abundance_id",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 6,
                    DisplayTitle = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 27,
                        FacetId = 27,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_abundances",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "species_author", new Facet() {

                FacetId = 28,
                FacetCode = "species_author",
                DisplayTitle = "Author",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_authors.author_id ",
                CategoryNameExpr = "tbl_taxa_tree_authors.author_name ",
                IconIdExpr = "tbl_taxa_tree_authors.author_id ",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_taxa_tree_authors.author_name ",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 6,
                    DisplayTitle = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 28,
                        FacetId = 28,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_authors",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 47,
                        FacetId = 28,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_taxa_tree_authors",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "feature_type", new Facet() {

                FacetId = 29,
                FacetCode = "feature_type",
                DisplayTitle = "Feature type",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_feature_types.feature_type_id ",
                CategoryNameExpr = "tbl_feature_types.feature_type_name",
                IconIdExpr = "tbl_feature_types.feature_id ",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_feature_types.feature_type_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 1,
                    DisplayTitle = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 29,
                        FacetId = 29,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_feature_types",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 48,
                        FacetId = 29,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_physical_sample_features",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "ecocode_system", new Facet() {

                FacetId = 30,
                FacetCode = "ecocode_system",
                DisplayTitle = "Eco code system",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_ecocode_systems.ecocode_system_id ",
                CategoryNameExpr = "tbl_ecocode_systems.name",
                IconIdExpr = "tbl_ecocode_systems.ecocode_system_id ",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_ecocode_systems.definition",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 4,
                    DisplayTitle = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 30,
                        FacetId = 30,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_ecocode_systems",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 49,
                        FacetId = 30,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_ecocode_systems",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "abundance_classification", new Facet() {

                FacetId = 31,
                FacetCode = "abundance_classification",
                DisplayTitle = "abundance classification",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "metainformation.view_abundance.elements_part_mod ",
                CategoryNameExpr = "metainformation.view_abundance.elements_part_mod ",
                IconIdExpr = "metainformation.view_abundance.elements_part_mod ",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "metainformation.view_abundance.elements_part_mod ",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 4,
                    DisplayTitle = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 31,
                        FacetId = 31,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "metainformation.view_abundance",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "abundances_all_helper", new Facet() {

                FacetId = 32,
                FacetCode = "abundances_all_helper",
                DisplayTitle = "Abundances",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "metainformation.view_abundance.abundance ",
                CategoryNameExpr = "metainformation.view_abundance.abundance ",
                IconIdExpr = "metainformation.view_abundance.abundance ",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "metainformation.view_abundance.abundance",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 4,
                    DisplayTitle = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 32,
                        FacetId = 32,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "metainformation.view_abundance",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetSourceTableId = 3,
                        FacetId = 32,
                        Clause = "metainformation.view_abundance.abundance is not null"
                    }
                }
                }},
			{ "abundances_all", new Facet() {

                FacetId = 33,
                FacetCode = "abundances_all",
                DisplayTitle = "Abundances",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "metainformation.view_abundance.abundance ",
                CategoryNameExpr = "metainformation.view_abundance.abundance ",
                IconIdExpr = "metainformation.view_abundance.abundance ",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "metainformation.view_abundance.abundance",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 4,
                    DisplayTitle = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 33,
                        FacetId = 33,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "metainformation.view_abundance",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetSourceTableId = 4,
                        FacetId = 33,
                        Clause = "metainformation.view_abundance.abundance is not null"
                    }
                }
                }},
			{ "activeseason", new Facet() {

                FacetId = 34,
                FacetCode = "activeseason",
                DisplayTitle = "Seasons",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_seasons.season_id",
                CategoryNameExpr = "tbl_seasons.season_name ",
                IconIdExpr = "tbl_seasons.season_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_seasons.season_type ",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 2,
                    DisplayTitle = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 34,
                        FacetId = 34,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_seasons",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "tbl_biblio_modern", new Facet() {

                FacetId = 35,
                FacetCode = "tbl_biblio_modern",
                DisplayTitle = "Bibligraphy modern",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "metainformation.view_taxa_biblio.biblio_id",
                CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.author ",
                IconIdExpr = "tbl_biblio.biblio_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "count of species",
                AggregateFacetId = 19,
                SortExpr = "tbl_biblio.author",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 1,
                    DisplayTitle = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 35,
                        FacetId = 35,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "metainformation.view_taxa_biblio",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 50,
                        FacetId = 35,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_biblio",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
                }},
			{ "tbl_biblio_sample_groups", new Facet() {

                FacetId = 36,
                FacetCode = "tbl_biblio_sample_groups",
                DisplayTitle = "Bibligraphy sites/Samplegroups",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_biblio.biblio_id",
                CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.author",
                IconIdExpr = "tbl_biblio.biblio_id",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_biblio.author",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 1,
                    DisplayTitle = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 36,
                        FacetId = 36,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_biblio",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 51,
                        FacetId = 36,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "metainformation.view_sample_group_references",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetSourceTableId = 5,
                        FacetId = 36,
                        Clause = "metainformation.view_sample_group_references.biblio_id is not null"
                    }
                }
                }},
			{ "tbl_biblio_sites", new Facet() {

                FacetId = 37,
                FacetCode = "tbl_biblio_sites",
                DisplayTitle = "Bibligraphy sites",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_biblio.biblio_id",
                CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.author",
                IconIdExpr = "tbl_biblio.biblio_id",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_biblio.author",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 1,
                    DisplayTitle = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 37,
                        FacetId = 37,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_biblio",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 52,
                        FacetId = 37,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "metainformation.view_site_references",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetSourceTableId = 6,
                        FacetId = 37,
                        Clause = "metainformation.view_site_references.biblio_id is not null"
                    }
                }
            }}
        };

        public static Dictionary<string, Facet> DummyStore = new Dictionary<string, Facet>() {

            { "dummy_discrete", new Facet() {

                FacetId = 1,
                FacetCode = "dummy",
                DisplayTitle = "Dummy discrete",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_dummy.category_id",
                CategoryNameExpr = "tbl_dummy.category_name",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of dummies",
                AggregateFacetId = 1,
                SortExpr = "dummy.sort_expr",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 1,
                    DisplayTitle = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 1,
                        FacetId = 1,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "tbl_dummy",
                        ObjectArgs = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 2,
                        FacetId = 1,
                        SequenceId = 2,
                        SchemaName = "",
                        ObjectName = "tbl_dummy_details",
                        ObjectArgs = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetSourceTableId = 1,
                        FacetId = 1,
                        Clause = "tbl_dummy_details.status = 9"
                    }
                }
            }}

        };
    }
}
