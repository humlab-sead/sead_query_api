using System;
using System.Collections.Generic;
using System.Text;
using SeadQueryCore;

namespace SeadQueryTest.Infrastructure.Scaffolds
{
	public static class FacetInstances
	{
		public static Dictionary<string, Facet> Store = new Dictionary<string, Facet>() {
			{ "result_facet", new Facet
			{
				FacetId = 1,
				FacetCode = "result_facet",
				DisplayTitle = "Analysis entities",
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
					IsApplicable = false,
					IsDefault = false
				},
				Tables = new List<FacetTable>
				{
					new FacetTable
					{
						FacetTableId = 53,
						FacetId = 1,
						SequenceId = 3,
						TableId = 86,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 86,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 1,
						FacetId = 1,
						SequenceId = 1,
						TableId = 4,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 4,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 38,
						FacetId = 1,
						SequenceId = 2,
						TableId = 102,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 102,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "dataset_helper", new Facet
			{
				FacetId = 2,
				FacetCode = "dataset_helper",
				DisplayTitle = "dataset_helper",
				FacetGroupId = 99,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_datasets.dataset_id",
				CategoryNameExpr = "tbl_datasets.dataset_id",
				IsApplicable = false,
				IsDefault = false,
				AggregateType = "count",
				AggregateTitle = "Number of samples",
				AggregateFacetId = 1,
				SortExpr = "tbl_dataset.dataset_id",
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
						TableId = 86,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 86,
							PrimaryKeyName = "",
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
				FacetGroupId = 5,
				FacetTypeId = SeadQueryCore.EFacetType.Range,
				CategoryIdExpr = "method_values.measured_value",
				CategoryNameExpr = "method_values.measured_value",
				IsApplicable = true,
				IsDefault = false,
				AggregateType = "",
				AggregateTitle = "Number of samples",
				AggregateFacetId = 1,
				SortExpr = "method_values.measured_value",
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
						TableId = 149,
						UdfCallArguments = "(33, 0)",
						Alias = "method_values",
						Table = new Table
						{
							TableId = 149,
							PrimaryKeyName = "",
							IsUdf = false
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
				FacetGroupId = 5,
				FacetTypeId = SeadQueryCore.EFacetType.Range,
				CategoryIdExpr = "method_values.measured_value",
				CategoryNameExpr = "method_values.measured_value",
				IsApplicable = true,
				IsDefault = false,
				AggregateType = "",
				AggregateTitle = "Number of samples",
				AggregateFacetId = 1,
				SortExpr = "method_values.measured_value",
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
						TableId = 149,
						UdfCallArguments = "(33, 82)",
						Alias = "method_values",
						Table = new Table
						{
							TableId = 149,
							PrimaryKeyName = "",
							IsUdf = false
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
				FacetGroupId = 5,
				FacetTypeId = SeadQueryCore.EFacetType.Range,
				CategoryIdExpr = "method_values.measured_value",
				CategoryNameExpr = "method_values.measured_value",
				IsApplicable = true,
				IsDefault = false,
				AggregateType = "",
				AggregateTitle = "Number of samples",
				AggregateFacetId = 1,
				SortExpr = "method_values.measured_value",
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
						TableId = 149,
						UdfCallArguments = "(32, 0)",
						Alias = "method_values",
						Table = new Table
						{
							TableId = 149,
							PrimaryKeyName = "",
							IsUdf = false
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
				DisplayTitle = " P┬░",
				FacetGroupId = 5,
				FacetTypeId = SeadQueryCore.EFacetType.Range,
				CategoryIdExpr = "method_values.measured_value",
				CategoryNameExpr = "method_values.measured_value",
				IsApplicable = true,
				IsDefault = false,
				AggregateType = "",
				AggregateTitle = "Number of samples",
				AggregateFacetId = 1,
				SortExpr = "method_values.measured_value",
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
						TableId = 149,
						UdfCallArguments = "(37, 0)",
						Alias = "method_values",
						Table = new Table
						{
							TableId = 149,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "measured_values_helper", new Facet
			{
				FacetId = 7,
				FacetCode = "measured_values_helper",
				DisplayTitle = "values",
				FacetGroupId = 99,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_measured_values.measured_value",
				CategoryNameExpr = "tbl_measured_values.measured_value",
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
					FacetGroupId = 99,
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
						TableId = 121,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 121,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "taxon_result", new Facet
			{
				FacetId = 8,
				FacetCode = "taxon_result",
				DisplayTitle = "taxon_id",
				FacetGroupId = 99,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_abundances.taxon_id",
				CategoryNameExpr = "tbl_abundances.taxon_id",
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
					FacetGroupId = 99,
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
						TableId = 88,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 88,
							PrimaryKeyName = "",
							IsUdf = false
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
						TableId = 119,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 119,
							PrimaryKeyName = "",
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
						TableId = 60,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 60,
							PrimaryKeyName = "",
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
						TableId = 71,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 71,
							PrimaryKeyName = "",
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
						TableId = 110,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 110,
							PrimaryKeyName = "",
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
						TableId = 91,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 91,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "places", new Facet
			{
				FacetId = 14,
				FacetCode = "places",
				DisplayTitle = "Places",
				FacetGroupId = 2,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_locations.location_id",
				CategoryNameExpr = "tbl_locations.location_name",
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
						FacetTableId = 39,
						FacetId = 14,
						SequenceId = 2,
						TableId = 113,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 113,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 14,
						FacetId = 14,
						SequenceId = 1,
						TableId = 35,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 35,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "places_all2", new Facet
			{
				FacetId = 15,
				FacetCode = "places_all2",
				DisplayTitle = "view_places_relations",
				FacetGroupId = 2,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_locations.location_id",
				CategoryNameExpr = "tbl_locations.location_name",
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
						FacetTableId = 40,
						FacetId = 15,
						SequenceId = 2,
						TableId = 35,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 35,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 54,
						FacetId = 15,
						SequenceId = 3,
						TableId = 113,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 113,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 15,
						FacetId = 15,
						SequenceId = 1,
						TableId = 7,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 7,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "sample_groups_helper", new Facet
			{
				FacetId = 16,
				FacetCode = "sample_groups_helper",
				DisplayTitle = "Sample group",
				FacetGroupId = 2,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_sample_groups.sample_group_id",
				CategoryNameExpr = "tbl_sample_groups.sample_group_name",
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
						TableId = 91,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 91,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "physical_samples", new Facet
			{
				FacetId = 17,
				FacetCode = "physical_samples",
				DisplayTitle = "physical samples",
				FacetGroupId = 2,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_physical_samples.physical_sample_id",
				CategoryNameExpr = "tbl_physical_samples.sample_name",
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
						TableId = 102,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 102,
							PrimaryKeyName = "",
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
						TableId = 119,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 119,
							PrimaryKeyName = "",
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
						TableId = 119,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 119,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "tbl_relative_dates_helper", new Facet
			{
				FacetId = 20,
				FacetCode = "tbl_relative_dates_helper",
				DisplayTitle = "tbl_relative_dates",
				FacetGroupId = 2,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_relative_dates.relative_age_id",
				CategoryNameExpr = "tbl_relative_dates.relative_age_name ",
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
						TableId = 55,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 55,
							PrimaryKeyName = "",
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
						TableId = 35,
						UdfCallArguments = null,
						Alias = "countries",
						Table = new Table
						{
							TableId = 35,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 41,
						FacetId = 21,
						SequenceId = 2,
						TableId = 113,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 113,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
					new FacetClause
					{
						FacetClauseId = 1,
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
				FacetGroupId = 4,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_ecocode_definitions.ecocode_definition_id",
				CategoryNameExpr = "tbl_ecocode_definitions.label",
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
						FacetTableId = 42,
						FacetId = 22,
						SequenceId = 2,
						TableId = 62,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 62,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 22,
						FacetId = 22,
						SequenceId = 1,
						TableId = 62,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 62,
							PrimaryKeyName = "",
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
						TableId = 36,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 36,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 43,
						FacetId = 23,
						SequenceId = 2,
						TableId = 36,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 36,
							PrimaryKeyName = "",
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
					IsApplicable = true,
					IsDefault = false
				},
				Tables = new List<FacetTable>
				{
					new FacetTable
					{
						FacetTableId = 44,
						FacetId = 24,
						SequenceId = 2,
						TableId = 148,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 148,
							PrimaryKeyName = "",
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
						Alias = "",
						Table = new Table
						{
							TableId = 148,
							PrimaryKeyName = "",
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
					IsApplicable = true,
					IsDefault = false
				},
				Tables = new List<FacetTable>
				{
					new FacetTable
					{
						FacetTableId = 55,
						FacetId = 25,
						SequenceId = 3,
						TableId = 39,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 39,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 25,
						FacetId = 25,
						SequenceId = 1,
						TableId = 109,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 109,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 57,
						FacetId = 25,
						SequenceId = 4,
						TableId = 119,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 119,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 45,
						FacetId = 25,
						SequenceId = 2,
						TableId = 148,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 148,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
					new FacetClause
					{
						FacetClauseId = 2,
						FacetId = 25,
						Clause = "tbl_sites.site_id is not null"
					}
				}
			} },
			{ "species_helper", new Facet
			{
				FacetId = 26,
				FacetCode = "species_helper",
				DisplayTitle = "Species",
				FacetGroupId = 6,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_taxa_tree_master.taxon_id",
				CategoryNameExpr = "tbl_taxa_tree_master.taxon_id",
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
						FacetTableId = 46,
						FacetId = 26,
						SequenceId = 2,
						TableId = 148,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 148,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 56,
						FacetId = 26,
						SequenceId = 3,
						TableId = 39,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 39,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 26,
						FacetId = 26,
						SequenceId = 1,
						TableId = 109,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 109,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "abundance_helper", new Facet
			{
				FacetId = 27,
				FacetCode = "abundance_helper",
				DisplayTitle = "abundance_id",
				FacetGroupId = 6,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_abundances.abundance_id",
				CategoryNameExpr = "tbl_abundances.abundance_id",
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
						TableId = 88,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 88,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
				}
			} },
			{ "species_author", new Facet
			{
				FacetId = 28,
				FacetCode = "species_author",
				DisplayTitle = "Author",
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
					IsApplicable = true,
					IsDefault = false
				},
				Tables = new List<FacetTable>
				{
					new FacetTable
					{
						FacetTableId = 47,
						FacetId = 28,
						SequenceId = 2,
						TableId = 39,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 39,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 28,
						FacetId = 28,
						SequenceId = 1,
						TableId = 39,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 39,
							PrimaryKeyName = "",
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
						TableId = 6,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 6,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 48,
						FacetId = 29,
						SequenceId = 2,
						TableId = 132,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 132,
							PrimaryKeyName = "",
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
					IsApplicable = true,
					IsDefault = false
				},
				Tables = new List<FacetTable>
				{
					new FacetTable
					{
						FacetTableId = 49,
						FacetId = 30,
						SequenceId = 2,
						TableId = 128,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 128,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 30,
						FacetId = 30,
						SequenceId = 1,
						TableId = 128,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 128,
							PrimaryKeyName = "",
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
						TableId = 57,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 57,
							PrimaryKeyName = "",
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
				FacetGroupId = 4,
				FacetTypeId = SeadQueryCore.EFacetType.Range,
				CategoryIdExpr = "facet.view_abundance.abundance ",
				CategoryNameExpr = "facet.view_abundance.abundance ",
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
						TableId = 57,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 57,
							PrimaryKeyName = "",
							IsUdf = false
						}
					}
				},
				Clauses = new List<FacetClause>
				{
					new FacetClause
					{
						FacetClauseId = 3,
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
				FacetGroupId = 4,
				FacetTypeId = SeadQueryCore.EFacetType.Range,
				CategoryIdExpr = "facet.view_abundance.abundance ",
				CategoryNameExpr = "facet.view_abundance.abundance ",
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
						TableId = 57,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 57,
							PrimaryKeyName = "",
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
				FacetGroupId = 2,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_seasons.season_id",
				CategoryNameExpr = "tbl_seasons.season_name ",
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
						TableId = 82,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 82,
							PrimaryKeyName = "",
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
				FacetGroupId = 1,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "facet.view_taxa_biblio.biblio_id",
				CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.author ",
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
						TableId = 99,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 99,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 50,
						FacetId = 35,
						SequenceId = 2,
						TableId = 84,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 84,
							PrimaryKeyName = "",
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
				FacetGroupId = 1,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_biblio.biblio_id",
				CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.author",
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
						TableId = 84,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 84,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 51,
						FacetId = 36,
						SequenceId = 2,
						TableId = 114,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 114,
							PrimaryKeyName = "",
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
				FacetGroupId = 1,
				FacetTypeId = SeadQueryCore.EFacetType.Discrete,
				CategoryIdExpr = "tbl_biblio.biblio_id",
				CategoryNameExpr = "tbl_biblio.title||\'  \'||tbl_biblio.author",
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
						TableId = 84,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 84,
							PrimaryKeyName = "",
							IsUdf = false
						}
					},
					new FacetTable
					{
						FacetTableId = 52,
						FacetId = 37,
						SequenceId = 2,
						TableId = 26,
						UdfCallArguments = null,
						Alias = "",
						Table = new Table
						{
							TableId = 26,
							PrimaryKeyName = "",
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
			} }
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
