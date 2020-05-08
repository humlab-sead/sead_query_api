using SeadQueryCore;
using SeadQueryCore.Model.Ext;
using SQT.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.QueryBuilder.ResultCompilers
{
    [Collection("JsonSeededFacetContext")]
	public class ResultCompositeFieldExtensionTests : DisposableFacetContextContainer
	{
		public ResultCompositeFieldExtensionTests(JsonSeededFacetContextFixture fixture) : base(fixture)
		{
		}

#if false
		[Fact]
		public void GenerateTestData()
		{
			var options = new DumpOptions()
			{
				DumpStyle = DumpStyle.CSharp,
				IndentSize = 1,
				IndentChar = '\t',
				LineBreakChar = Environment.NewLine,
				SetPropertiesOnly = false,
				MaxLevel = 5,
				ExcludeProperties = new HashSet<string>() {
					"Compiler",
					"FieldType",
					"ResultField",
					"IsGroupByField",
					"Composite"
				},
				PropertyOrderBy = null,
				IgnoreDefaultValues = false
			};


			ScaffoldUtility.Dump(
				new
				{
					//ResultFieldTypes = Registry.Context.Set<ResultFieldType>().ToList(),
					//ResultFields = Registry.Context.Set<ResultField>().ToList(),
					// ResultCompositeFields = Registry.Context.Set<ResultCompositeField>().ToList(),
					ResultComposites = Registry.Context.Set<ResultComposite>().Include(z => z.Fields).ToList()
				},
				@"C:\TEMP\ResultCompositeFields.cs",
				options
			);
		}
#endif

		[Fact]
		public void GetCompositeAliasedFields_Called_Success()
		{
			// Arrange
			var aggregate = FakeResultCompositeFixture();
			var fields = aggregate.GetSortedFields();

			// Act
			var result = fields.GetResultAliasedFields();

			// Assert
			var expected = aggregate.Fields.Count;
			Assert.Equal(expected, result.Count());
		}

		[Fact]
		public void GetCompositeGroupByFields_Called_Success()
		{
			// Arrange
			var aggregate = FakeResultCompositeFixture();
			var fields = aggregate.GetSortedFields();

			// Act
			var result = fields.GetResultGroupByFields();

			// Assert
			var expected = aggregate.Fields.Where(z => z.FieldType.IsGroupByField);
			Assert.Equal(expected.Count(), result.Count());
		}

		[Fact]
		public void GetCompositeCompiledDataFields_Called_Success()
		{
			// Arrange
			var aggregate = FakeResultCompositeFixture();
			var fields = aggregate.GetSortedFields();

			// Act
			var result = fields.GetResultCompiledValueFields();

			// Assert
			var expected = aggregate.Fields.Where(z => z.FieldType.IsResultValue);
			Assert.Equal(expected.Count(), result.Count());
		}

		[Fact]
		public void GetCompositeSortFields_Called_Success()
		{
			// Arrange
			var aggregate = FakeResultCompositeFixture();
			var fields = aggregate.GetSortedFields();

			// Act
			var result = fields.GetResultSortFields();

			// Assert
			var expected = aggregate.Fields.Where(z => z.FieldType.IsSortField);
			Assert.Equal(expected.Count(), result.Count());
		}

		[Fact]
		public void GetCompositeInnerGroupByFields_Called_Success()
		{
			// Arrange
			var aggregate = FakeResultCompositeFixture();
			var fields = aggregate.GetSortedFields();

			// Act
			var result = fields.GetResultInnerGroupByFields();

			// Assert
			var expected = aggregate.Fields;
			Assert.Equal(expected.Count, result.Count());
		}
		[Fact]
		public void GetCompositeColumnNameAliasPairs_Called_Success()
		{
			// Arrange
			var aggregate = FakeResultCompositeFixture();
			var fields = aggregate.GetSortedFields();

			// Act
			var result = fields.GetResultColumnNameAliasPairs();

			// Assert
			var expected = aggregate.Fields;
			Assert.Equal(expected.Count, result.Count());
		}

		[Fact]
		public void Any_Success()
		{
			var aggregate = FakeResultCompositeFixture();
			var fields = aggregate.GetSortedFields();

			// Act
			var result = fields.Any();

			//// Assert
			Assert.True(result);
		}

        #region FakeData

        ResultComposite FakeResultCompositeFixture() => new ResultComposite
		{
			CompositeId = 1,
			CompositeKey = "site_level",
			DisplayText = "Site level",
			IsActivated = true,
			Fields = new List<ResultCompositeField> {
				FakeResultCompositeField(4, "single_item", 1, 1),
				FakeResultCompositeField(5, "text_agg_item", 2, 2),
				FakeResultCompositeField(8, "count_item", 3, 3),
				FakeResultCompositeField(10, "link_item", 4, 4),
				FakeResultCompositeField(13, "link_item_filtered", 5, 5),
				FakeResultCompositeField(16, "sort_item", 1, 99)
			}
		};

		private static ResultCompositeField FakeResultCompositeField(int id, string fieldTypeId, int resultFieldId, int sequenceId)
			=> new ResultCompositeField
				{
					CompositeFieldId = id,
					CompositeId = 1,
					FieldTypeId = fieldTypeId,
					ResultFieldId = resultFieldId,
					SequenceId = sequenceId,
					FieldType = ResultFieldTypes[fieldTypeId],
					ResultField = ResultFields[resultFieldId]
				};

		public static Dictionary<int, ResultField> ResultFields = new List<ResultField>  {
			new ResultField
			{
				ResultFieldId = 1,
				TableName = "tbl_sites",
				ColumnName = "tbl_sites.site_name",
				ResultFieldKey = "sitename",
				DisplayText = "Site name",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 2,
				TableName = "tbl_record_types",
				ColumnName = "tbl_record_types.record_type_name",
				ResultFieldKey = "record_type",
				DisplayText = "Record type(s)",
				FieldTypeId = "text_agg_item",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 3,
				TableName = "tbl_analysis_entities",
				ColumnName = "tbl_analysis_entities.analysis_entity_id",
				ResultFieldKey = "analysis_entities",
				DisplayText = "Filtered records",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 4,
				TableName = "tbl_sites",
				ColumnName = "tbl_sites.site_id",
				ResultFieldKey = "site_link",
				DisplayText = "Full report",
				FieldTypeId = "link_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 5,
				TableName = "tbl_sites",
				ColumnName = "tbl_sites.site_id",
				ResultFieldKey = "site_link_filtered",
				DisplayText = "Filtered report",
				FieldTypeId = "link_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 6,
				TableName = "tbl_aggregate_samples",
				ColumnName = "\'Aggregated\'::text",
				ResultFieldKey = "aggregate_all_filtered",
				DisplayText = "Filtered report",
				FieldTypeId = "link_item_filtered",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 7,
				TableName = "tbl_sample_groups",
				ColumnName = "tbl_sample_groups.sample_group_id",
				ResultFieldKey = "sample_group_link",
				DisplayText = "Full report",
				FieldTypeId = "link_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 8,
				TableName = "tbl_sample_groups",
				ColumnName = "tbl_sample_groups.sample_group_id",
				ResultFieldKey = "sample_group_link_filtered",
				DisplayText = "Filtered report",
				FieldTypeId = "link_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 9,
				TableName = "tbl_abundances",
				ColumnName = "tbl_abundances.abundance",
				ResultFieldKey = "abundance",
				DisplayText = "number of taxon_id",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 10,
				TableName = "tbl_abundances",
				ColumnName = "tbl_abundances.taxon_id",
				ResultFieldKey = "taxon_id",
				DisplayText = "Taxon id  (specie)",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 11,
				TableName = "tbl_datasets",
				ColumnName = "tbl_datasets.dataset_name",
				ResultFieldKey = "dataset",
				DisplayText = "Dataset",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 12,
				TableName = "tbl_datasets",
				ColumnName = "tbl_datasets.dataset_id",
				ResultFieldKey = "dataset_link",
				DisplayText = "Dataset details",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 13,
				TableName = "tbl_datasets",
				ColumnName = "tbl_datasets.dataset_id",
				ResultFieldKey = "dataset_link_filtered",
				DisplayText = "Filtered report",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 14,
				TableName = "tbl_sample_groups",
				ColumnName = "tbl_sample_groups.sample_group_name",
				ResultFieldKey = "sample_group",
				DisplayText = "Sample group",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 15,
				TableName = "tbl_methods",
				ColumnName = "tbl_methods.method_name",
				ResultFieldKey = "methods",
				DisplayText = "Method",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 18,
				TableName = null,
				ColumnName = "category_id",
				ResultFieldKey = "category_id",
				DisplayText = "Site ID",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "int"
			},
			new ResultField
			{
				ResultFieldId = 19,
				TableName = null,
				ColumnName = "category_name",
				ResultFieldKey = "category_name",
				DisplayText = "Site Name",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "text"
			},
			new ResultField
			{
				ResultFieldId = 20,
				TableName = null,
				ColumnName = "latitude_dd",
				ResultFieldKey = "latitude_dd",
				DisplayText = "Latitude (dd)",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "decimal"
			},
			new ResultField
			{
				ResultFieldId = 21,
				TableName = null,
				ColumnName = "longitude_dd",
				ResultFieldKey = "longitude_dd",
				DisplayText = "Longitude (dd)",
				FieldTypeId = "single_item",
				Activated = true,
				DataType = "decimal"
			}
		}.ToDictionary(z => z.ResultFieldId);


		public static Dictionary<string, ResultFieldType> ResultFieldTypes = new List<ResultFieldType> {
			new ResultFieldType
			{
				FieldTypeId = "link_item",
				IsResultValue = true,
				IsSortField = false,
				IsAggregateField = false,
				IsItemField = true,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "{0}"
			},
			new ResultFieldType
			{
				FieldTypeId = "sort_item",
				IsResultValue = false,
				IsSortField = true,
				IsAggregateField = false,
				IsItemField = false,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "{0}"
			},
			new ResultFieldType
			{
				FieldTypeId = "single_item",
				IsResultValue = true,
				IsSortField = false,
				IsAggregateField = false,
				IsItemField = true,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "{0}"
			},
			new ResultFieldType
			{
				FieldTypeId = "link_item_filtered",
				IsResultValue = true,
				IsSortField = false,
				IsAggregateField = false,
				IsItemField = true,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "{0}"
			},
			new ResultFieldType
			{
				FieldTypeId = "avg_item",
				IsResultValue = true,
				IsSortField = false,
				IsAggregateField = true,
				IsItemField = false,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "AVG({0}) AS avg_of_{0}"
			},
			new ResultFieldType
			{
				FieldTypeId = "sum_item",
				IsResultValue = true,
				IsSortField = false,
				IsAggregateField = true,
				IsItemField = false,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "SUM({0}::double precision) AS sum_of_{0}"
			},
			new ResultFieldType
			{
				FieldTypeId = "text_agg_item",
				IsResultValue = true,
				IsSortField = false,
				IsAggregateField = true,
				IsItemField = false,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "ARRAY_TO_STRING(ARRAY_AGG(DISTINCT {0}),\',\') AS text_agg_of_{0}"
			},
			new ResultFieldType
			{
				FieldTypeId = "count_item",
				IsResultValue = true,
				IsSortField = false,
				IsAggregateField = true,
				IsItemField = false,
				SqlFieldCompiler = "TemplateFieldCompiler",
				SqlTemplate = "COUNT({0}) AS count_of_{0}"
			}
		}.ToDictionary(z => z.FieldTypeId);

		#endregion

	}
}
