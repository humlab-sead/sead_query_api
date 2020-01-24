using System;
using System.Collections.Generic;
using System.Text;
using SeadQueryCore;

namespace SeadQueryTest.Fixtures
{
    public static class FacetFixtures
    {
        public static Dictionary<string, Facet> Store = new Dictionary<String, Facet>

        #region __the following content is generated with "GenerateFacets" test method

        {
            { "result_facet", new Facet
            {
                FacetId = 1,
                FacetCode = "result_facet",
                DisplayTitle = "Analysis entities",
                Description = "Analysis entities",
                FacetGroupId = 99,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                CategoryNameExpr = "tbl_physical_samples.sample_name||\' \'||tbl_datasets.dataset_name",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 0,
                SortExpr = "tbl_datasets.dataset_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 99,
                    DisplayTitle = "ROOT",
                    Description = "ROOT",
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
                        TableId = 4,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 4,
                            PrimaryKeyName = "analysis_entity_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 2,
                        FacetId = 1,
                        SequenceId = 3,
                        TableId = 86,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 86,
                            PrimaryKeyName = "dataset_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 3,
                        FacetId = 1,
                        SequenceId = 2,
                        TableId = 102,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 102,
                            PrimaryKeyName = "physical_sample_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "tbl_denormalized_measured_values_33_0", new Facet
            {
                FacetId = 3,
                FacetCode = "tbl_denormalized_measured_values_33_0",
                DisplayTitle = "MS ",
                Description = "MS ",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "method_values_33.measured_value",
                CategoryNameExpr = "method_values_33.measured_value",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "method_values_33.measured_value",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    Description = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 8,
                        FacetId = 3,
                        SequenceId = 1,
                        TableId = 150,
                        UdfCallArguments = null,
                        Alias = "method_values_33",
                        Table = new Table
                        {
                            TableId = 150,
                            PrimaryKeyName = "analysis_entity_id",
                            IsUdf = true
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "tbl_denormalized_measured_values_33_82", new Facet
            {
                FacetId = 4,
                FacetCode = "tbl_denormalized_measured_values_33_82",
                DisplayTitle = "MS Heating 550",
                Description = "MS Heating 550",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "method_values_33_82.measured_value",
                CategoryNameExpr = "method_values_33_82.measured_value",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "method_values_33_82.measured_value",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    Description = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 9,
                        FacetId = 4,
                        SequenceId = 1,
                        TableId = 151,
                        UdfCallArguments = null,
                        Alias = "method_values_33_82",
                        Table = new Table
                        {
                            TableId = 151,
                            PrimaryKeyName = "analysis_entity_id",
                            IsUdf = true
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "tbl_denormalized_measured_values_32", new Facet
            {
                FacetId = 5,
                FacetCode = "tbl_denormalized_measured_values_32",
                DisplayTitle = "LOI",
                Description = "Loss of ignition",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "method_values_32.measured_value",
                CategoryNameExpr = "method_values_32.measured_value",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "method_values_32.measured_value",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    Description = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 10,
                        FacetId = 5,
                        SequenceId = 1,
                        TableId = 152,
                        UdfCallArguments = null,
                        Alias = "method_values_32",
                        Table = new Table
                        {
                            TableId = 152,
                            PrimaryKeyName = "analysis_entity_id",
                            IsUdf = true
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "tbl_denormalized_measured_values_37", new Facet
            {
                FacetId = 6,
                FacetCode = "tbl_denormalized_measured_values_37",
                DisplayTitle = "P° ",
                Description = "P°",
                FacetGroupId = 5,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "method_values_37.measured_value",
                CategoryNameExpr = "method_values_37.measured_value",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "method_values_37.measured_value",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Range,
                    FacetTypeName = "range",
                    ReloadAsTarget = true
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 5,
                    DisplayTitle = "Measured values",
                    Description = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 11,
                        FacetId = 6,
                        SequenceId = 1,
                        TableId = 153,
                        UdfCallArguments = null,
                        Alias = "method_values_37",
                        Table = new Table
                        {
                            TableId = 153,
                            PrimaryKeyName = "analysis_entity_id",
                            IsUdf = true
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "map_result", new Facet
            {
                FacetId = 9,
                FacetCode = "map_result",
                DisplayTitle = "Site",
                Description = "Site",
                FacetGroupId = 99,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sites.site_id",
                CategoryNameExpr = "tbl_sites.site_name",
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
                    FacetGroupId = 99,
                    DisplayTitle = "ROOT",
                    Description = "ROOT",
                    IsApplicable = false,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 4,
                        FacetId = 9,
                        SequenceId = 1,
                        TableId = 119,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 119,
                            PrimaryKeyName = "site_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "geochronology", new Facet
            {
                FacetId = 10,
                FacetCode = "geochronology",
                DisplayTitle = "Geochronology",
                Description = "Sample ages as retrieved through absolute methods such as radiocarbon dating or other radiometric methods (in method based years before present - e.g. 14C years)",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "tbl_geochronology.age",
                CategoryNameExpr = "tbl_geochronology.age",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 12,
                        FacetId = 10,
                        SequenceId = 1,
                        TableId = 60,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 60,
                            PrimaryKeyName = "geochron_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "relative_age_name", new Facet
            {
                FacetId = 11,
                FacetCode = "relative_age_name",
                DisplayTitle = "Time periods",
                Description = "Age of sample as defined by association with a (often regionally specific) cultural or geological period (in years before present)",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_relative_ages.relative_age_id",
                CategoryNameExpr = "tbl_relative_ages.relative_age_name",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 13,
                        FacetId = 11,
                        SequenceId = 1,
                        TableId = 71,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 71,
                            PrimaryKeyName = "relative_age_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "record_types", new Facet
            {
                FacetId = 12,
                FacetCode = "record_types",
                DisplayTitle = "Proxy types",
                Description = "Proxy types",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_record_types.record_type_id",
                CategoryNameExpr = "tbl_record_types.record_type_name",
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
                    Description = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 14,
                        FacetId = 12,
                        SequenceId = 1,
                        TableId = 110,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 110,
                            PrimaryKeyName = "record_type_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "sample_groups", new Facet
            {
                FacetId = 13,
                FacetCode = "sample_groups",
                DisplayTitle = "Sample group",
                Description = "A collection of samples, usually defined by the excavator or collector",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sample_groups.sample_group_id",
                CategoryNameExpr = "tbl_sample_groups.sample_group_name",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 15,
                        FacetId = 13,
                        SequenceId = 1,
                        TableId = 91,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 91,
                            PrimaryKeyName = "sample_group_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "sites", new Facet
            {
                FacetId = 18,
                FacetCode = "sites",
                DisplayTitle = "Site",
                Description = "General name for the excavation or sampling location",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sites.site_id",
                CategoryNameExpr = "tbl_sites.site_name",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 16,
                        FacetId = 18,
                        SequenceId = 1,
                        TableId = 119,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 119,
                            PrimaryKeyName = "site_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "sites_helper", new Facet
            {
                FacetId = 19,
                FacetCode = "sites_helper",
                DisplayTitle = "Site",
                Description = "Report helper",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_sites.site_id",
                CategoryNameExpr = "tbl_sites.site_name",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 6,
                        FacetId = 19,
                        SequenceId = 1,
                        TableId = 119,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 119,
                            PrimaryKeyName = "site_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "country", new Facet
            {
                FacetId = 21,
                FacetCode = "country",
                DisplayTitle = "Country",
                Description = "The name of the country, at the time of collection, in which the samples were collected",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "countries.location_id",
                CategoryNameExpr = "countries.location_name ",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 17,
                        FacetId = 21,
                        SequenceId = 1,
                        TableId = 35,
                        UdfCallArguments = null,
                        Alias = "countries",
                        Table = new Table
                        {
                            TableId = 35,
                            PrimaryKeyName = "location_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 18,
                        FacetId = 21,
                        SequenceId = 2,
                        TableId = 113,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 113,
                            PrimaryKeyName = "site_location_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 2,
                        FacetId = 21,
                        Clause = "countries.location_type_id=1"
                    }
                }
            } },
            { "ecocode", new Facet
            {
                FacetId = 22,
                FacetCode = "ecocode",
                DisplayTitle = "Eco code",
                Description = "Ecological category (trait) or cultural relevance of organisms based on a classification system",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_ecocode_definitions.ecocode_definition_id",
                CategoryNameExpr = "tbl_ecocode_definitions.name",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_ecocode_definitions.name",
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
                    Description = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 19,
                        FacetId = 22,
                        SequenceId = 2,
                        TableId = 62,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 62,
                            PrimaryKeyName = "ecocode_definition_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 20,
                        FacetId = 22,
                        SequenceId = 1,
                        TableId = 62,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 62,
                            PrimaryKeyName = "ecocode_definition_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "family", new Facet
            {
                FacetId = 23,
                FacetCode = "family",
                DisplayTitle = "Family",
                Description = "Taxonomic family",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_families.family_id",
                CategoryNameExpr = "tbl_taxa_tree_families.family_name ",
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
                    Description = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 21,
                        FacetId = 23,
                        SequenceId = 2,
                        TableId = 36,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 36,
                            PrimaryKeyName = "family_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 22,
                        FacetId = 23,
                        SequenceId = 1,
                        TableId = 36,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 36,
                            PrimaryKeyName = "family_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "genus", new Facet
            {
                FacetId = 24,
                FacetCode = "genus",
                DisplayTitle = "Genus",
                Description = "Taxonomic genus (under family)",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_genera.genus_id",
                CategoryNameExpr = "tbl_taxa_tree_genera.genus_name",
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
                    Description = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 23,
                        FacetId = 24,
                        SequenceId = 2,
                        TableId = 148,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 148,
                            PrimaryKeyName = "genus_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 24,
                        FacetId = 24,
                        SequenceId = 1,
                        TableId = 148,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 148,
                            PrimaryKeyName = "genus_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "species", new Facet
            {
                FacetId = 25,
                FacetCode = "species",
                DisplayTitle = "Taxa",
                Description = "Taxonomic species (under genus)",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_master.taxon_id",
                CategoryNameExpr = "concat_ws(\' \', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)",
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
                    Description = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 25,
                        FacetId = 25,
                        SequenceId = 4,
                        TableId = 119,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 119,
                            PrimaryKeyName = "site_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 26,
                        FacetId = 25,
                        SequenceId = 3,
                        TableId = 39,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 39,
                            PrimaryKeyName = "author_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 27,
                        FacetId = 25,
                        SequenceId = 2,
                        TableId = 148,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 148,
                            PrimaryKeyName = "genus_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 28,
                        FacetId = 25,
                        SequenceId = 1,
                        TableId = 109,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 109,
                            PrimaryKeyName = "taxon_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 3,
                        FacetId = 25,
                        Clause = "tbl_sites.site_id is not null"
                    }
                }
            } },
            { "species_author", new Facet
            {
                FacetId = 28,
                FacetCode = "species_author",
                DisplayTitle = "Author",
                Description = "Authority of the taxonomic name (not used for all species)",
                FacetGroupId = 6,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_authors.author_id ",
                CategoryNameExpr = "tbl_taxa_tree_authors.author_name ",
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
                    Description = "Taxonomy",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 29,
                        FacetId = 28,
                        SequenceId = 2,
                        TableId = 39,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 39,
                            PrimaryKeyName = "author_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 30,
                        FacetId = 28,
                        SequenceId = 1,
                        TableId = 39,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 39,
                            PrimaryKeyName = "author_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "feature_type", new Facet
            {
                FacetId = 29,
                FacetCode = "feature_type",
                DisplayTitle = "Feature type",
                Description = "Feature type",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_feature_types.feature_type_id ",
                CategoryNameExpr = "tbl_feature_types.feature_type_name",
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
                    Description = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 31,
                        FacetId = 29,
                        SequenceId = 1,
                        TableId = 6,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 6,
                            PrimaryKeyName = "feature_type_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 32,
                        FacetId = 29,
                        SequenceId = 2,
                        TableId = 132,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 132,
                            PrimaryKeyName = "physical_sample_feature_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "ecocode_system", new Facet
            {
                FacetId = 30,
                FacetCode = "ecocode_system",
                DisplayTitle = "Eco code system",
                Description = "Ecological or cultural organism classification system (which groups items in the ecological/cultural category filter)",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_ecocode_systems.ecocode_system_id ",
                CategoryNameExpr = "tbl_ecocode_systems.name",
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
                    Description = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 33,
                        FacetId = 30,
                        SequenceId = 2,
                        TableId = 128,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 128,
                            PrimaryKeyName = "ecocode_system_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 34,
                        FacetId = 30,
                        SequenceId = 1,
                        TableId = 128,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 128,
                            PrimaryKeyName = "ecocode_system_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "abundance_classification", new Facet
            {
                FacetId = 31,
                FacetCode = "abundance_classification",
                DisplayTitle = "abundance classification",
                Description = "abundance classification",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "facet.view_abundance.elements_part_mod ",
                CategoryNameExpr = "facet.view_abundance.elements_part_mod ",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "facet.view_abundance.elements_part_mod ",
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
                    Description = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 35,
                        FacetId = 31,
                        SequenceId = 1,
                        TableId = 57,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 57,
                            PrimaryKeyName = "xxxx",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "abundances_all_helper", new Facet
            {
                FacetId = 32,
                FacetCode = "abundances_all_helper",
                DisplayTitle = "Abundances",
                Description = "Abundances",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "facet.view_abundance.abundance",
                CategoryNameExpr = "facet.view_abundance.abundance",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "facet.view_abundance.abundance",
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
                    Description = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 7,
                        FacetId = 32,
                        SequenceId = 1,
                        TableId = 57,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 57,
                            PrimaryKeyName = "xxxx",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 1,
                        FacetId = 32,
                        Clause = "facet.view_abundance.abundance is not null"
                    }
                }
            } },
            { "abundances_all", new Facet
            {
                FacetId = 33,
                FacetCode = "abundances_all",
                DisplayTitle = "Abundances",
                Description = "Abundances",
                FacetGroupId = 4,
                FacetTypeId = SeadQueryCore.EFacetType.Range,
                CategoryIdExpr = "facet.view_abundance.abundance",
                CategoryNameExpr = "facet.view_abundance.abundance",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "facet.view_abundance.abundance",
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
                    Description = "Ecology",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 36,
                        FacetId = 33,
                        SequenceId = 1,
                        TableId = 57,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 57,
                            PrimaryKeyName = "xxxx",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 4,
                        FacetId = 33,
                        Clause = "facet.view_abundance.abundance is not null"
                    }
                }
            } },
            { "activeseason", new Facet
            {
                FacetId = 34,
                FacetCode = "activeseason",
                DisplayTitle = "Seasons",
                Description = "Seasons",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_seasons.season_id",
                CategoryNameExpr = "tbl_seasons.season_name",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 37,
                        FacetId = 34,
                        SequenceId = 1,
                        TableId = 82,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 82,
                            PrimaryKeyName = "season_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "tbl_biblio_modern", new Facet
            {
                FacetId = 35,
                FacetCode = "tbl_biblio_modern",
                DisplayTitle = "Bibligraphy modern",
                Description = "Bibligraphy modern",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "facet.view_taxa_biblio.biblio_id",
                CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.authors ",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "count of species",
                AggregateFacetId = 19,
                SortExpr = "tbl_biblio.authors",
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
                    Description = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 38,
                        FacetId = 35,
                        SequenceId = 2,
                        TableId = 84,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 84,
                            PrimaryKeyName = "biblio_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 39,
                        FacetId = 35,
                        SequenceId = 1,
                        TableId = 99,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 99,
                            PrimaryKeyName = "xxxx",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "tbl_biblio_sample_groups", new Facet
            {
                FacetId = 36,
                FacetCode = "tbl_biblio_sample_groups",
                DisplayTitle = "Bibligraphy sites/Samplegroups",
                Description = "Bibligraphy sites/Samplegroups",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_biblio.biblio_id",
                CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.authors",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_biblio.authors",
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
                    Description = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 40,
                        FacetId = 36,
                        SequenceId = 1,
                        TableId = 84,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 84,
                            PrimaryKeyName = "biblio_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 41,
                        FacetId = 36,
                        SequenceId = 2,
                        TableId = 114,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 114,
                            PrimaryKeyName = "xxxx",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 5,
                        FacetId = 36,
                        Clause = "facet.view_sample_group_references.biblio_id is not null"
                    }
                }
            } },
            { "tbl_biblio_sites", new Facet
            {
                FacetId = 37,
                FacetCode = "tbl_biblio_sites",
                DisplayTitle = "Bibligraphy sites",
                Description = "Bibligraphy sites",
                FacetGroupId = 1,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_biblio.biblio_id",
                CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.authors",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_biblio.authors",
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
                    Description = "Others",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 42,
                        FacetId = 37,
                        SequenceId = 1,
                        TableId = 84,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 84,
                            PrimaryKeyName = "biblio_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 43,
                        FacetId = 37,
                        SequenceId = 2,
                        TableId = 26,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 26,
                            PrimaryKeyName = "xxxx",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 6,
                        FacetId = 37,
                        Clause = "facet.view_site_references.biblio_id is not null"
                    }
                }
            } },
            { "dataset_master", new Facet
            {
                FacetId = 38,
                FacetCode = "dataset_master",
                DisplayTitle = "Master datasets",
                Description = "Master datasets",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_dataset_masters.master_set_id ",
                CategoryNameExpr = "tbl_dataset_masters.master_name",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of samples",
                AggregateFacetId = 1,
                SortExpr = "tbl_dataset_masters.master_name",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 44,
                        FacetId = 38,
                        SequenceId = 1,
                        TableId = 76,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 76,
                            PrimaryKeyName = "master_set_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "dataset_methods", new Facet
            {
                FacetId = 39,
                FacetCode = "dataset_methods",
                DisplayTitle = "Dataset methods",
                Description = "Dataset methods",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_methods.method_id ",
                CategoryNameExpr = "tbl_methods.method_name",
                IsApplicable = true,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of datasets",
                AggregateFacetId = 40,
                SortExpr = "tbl_methods.method_name",
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 45,
                        FacetId = 39,
                        SequenceId = 1,
                        TableId = 76,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 76,
                            PrimaryKeyName = "master_set_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 46,
                        FacetId = 39,
                        SequenceId = 2,
                        TableId = 86,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 86,
                            PrimaryKeyName = "dataset_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "result_facet_dataset", new Facet
            {
                FacetId = 40,
                FacetCode = "result_facet_dataset",
                DisplayTitle = "Datasets",
                Description = "Datasets",
                FacetGroupId = 99,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_datasets.dataset_id",
                CategoryNameExpr = "tbl_datasets.dataset_name",
                IsApplicable = false,
                IsDefault = false,
                AggregateType = "count",
                AggregateTitle = "Number of datasets",
                AggregateFacetId = 0,
                SortExpr = "tbl_datasets.dataset_name",
                FacetType = new FacetType
                {
                    FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                    FacetTypeName = "discrete",
                    ReloadAsTarget = false
                },
                FacetGroup = new FacetGroup
                {
                    FacetGroupId = 99,
                    DisplayTitle = "ROOT",
                    Description = "ROOT",
                    IsApplicable = false,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 5,
                        FacetId = 40,
                        SequenceId = 1,
                        TableId = 86,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 86,
                            PrimaryKeyName = "dataset_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                }
            } },
            { "region", new Facet
            {
                FacetId = 41,
                FacetCode = "region",
                DisplayTitle = "Region",
                Description = "Region",
                FacetGroupId = 2,
                FacetTypeId = SeadQueryCore.EFacetType.Discrete,
                CategoryIdExpr = "tbl_locations.location_id ",
                CategoryNameExpr = "tbl_locations.location_name || \'  \' || tbl_sites.site_name",
                IsApplicable = true,
                IsDefault = false,
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
                    Description = "Space/Time",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable>
                {
                    new FacetTable
                    {
                        FacetTableId = 47,
                        FacetId = 41,
                        SequenceId = 1,
                        TableId = 35,
                        UdfCallArguments = null,
                        Alias = "null",
                        Table = new Table
                        {
                            TableId = 35,
                            PrimaryKeyName = "location_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 48,
                        FacetId = 41,
                        SequenceId = 2,
                        TableId = 113,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 113,
                            PrimaryKeyName = "site_location_id",
                            IsUdf = false
                        }
                    },
                    new FacetTable
                    {
                        FacetTableId = 49,
                        FacetId = 41,
                        SequenceId = 3,
                        TableId = 119,
                        UdfCallArguments = null,
                        Alias = null,
                        Table = new Table
                        {
                            TableId = 119,
                            PrimaryKeyName = "site_id",
                            IsUdf = false
                        }
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 7,
                        FacetId = 41,
                        Clause = "tbl_locations.location_type_id in (2, 7, 14, 16, 18)"
                    }
                }
            }}

        #endregion

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
                        TableId = 0,
                        Table = new Table { TableId = 0, TableOrUdfName = "tbl_dummy"},
                        UdfCallArguments = null,
                        Alias = ""
                    },
                    new FacetTable
                    {
                        FacetTableId = 2,
                        FacetId = 1,
                        SequenceId = 2,
                        TableId = 0,
                        Table = new Table { TableId = 0, TableOrUdfName = "tbl_dummy_details"},
                        UdfCallArguments = null,
                        Alias = ""
                    }
                },
                Clauses = new List<FacetClause>
                {
                    new FacetClause
                    {
                        FacetClauseId = 1,
                        FacetId = 1,
                        Clause = "tbl_dummy_details.status = 9"
                    }
                }
            }}

        };
    }
}
