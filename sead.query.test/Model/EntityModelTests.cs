using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

using SeadQueryTest.Infrastructure.Scaffolding;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace SeadQueryTest.Model
{

    public class EntityModelTests : FacetTestBase
    {


        [Fact]
        public void Context_Should_Have_Values_For_All_Entity_Types()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                foreach (Type type in ScaffoldUtility.GetModelTypes()) {
                    var g = GetGenericMethodForType<FacetContext>("Set", type);
                    var entities = (IEnumerable<object>)g.Invoke(context, new object[] { });
                    Assert.True(entities.ToList().Count > 0);
                }
            }
        }

        [Fact]
        public void Facet_Should_Be_Complete_When_Fetch_From_Context()
        {
            // Arrange
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                // Act
                var facet = context.Facets.Find(1);

                // Assert
                Assert.NotNull(facet);

                var expected = new Facet() {
                    FacetId = 1,
                    FacetCode = "result_facet",
                    DisplayTitle = "Analysis entities",
                    FacetGroupId = 999,
                    FacetTypeId = (EFacetType)1,
                    CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                    CategoryNameExpr = "tbl_physical_samples.sample_name||' '||tbl_datasets.dataset_name",
                    IconIdExpr = "tbl_analysis_entities.analysis_entity_id",
                    SortExpr = "tbl_datasets.dataset_name",
                    IsApplicable = false,
                    IsDefault = false,
                    AggregateType = "count",
                    AggregateTitle = "Number of samples",
                    AggregateFacetId = 0
                };

                Assert.Equal(expected.FacetId, facet.FacetId);
                Assert.Equal(expected.FacetCode, facet.FacetCode);
                Assert.Equal(expected.DisplayTitle, facet.DisplayTitle);
                Assert.Equal(expected.FacetGroupId, facet.FacetGroupId);
                Assert.Equal(expected.FacetTypeId, facet.FacetTypeId);
                Assert.Equal(expected.CategoryIdExpr, facet.CategoryIdExpr);
                Assert.Equal(expected.CategoryNameExpr, facet.CategoryNameExpr);
                Assert.Equal(expected.IconIdExpr, facet.IconIdExpr);
                Assert.Equal(expected.SortExpr, facet.SortExpr);
                Assert.Equal(expected.IsApplicable, facet.IsApplicable);
                Assert.Equal(expected.IsDefault, facet.IsDefault);
                Assert.Equal(expected.FacetCode, facet.FacetCode);
                Assert.Equal(expected.AggregateType, facet.AggregateType);
                Assert.Equal(expected.AggregateTitle, facet.AggregateTitle);
                Assert.Equal(expected.AggregateFacetId, facet.AggregateFacetId);

                Assert.NotNull(facet.Clauses);
                Assert.NotNull(facet.Tables);
                Assert.NotNull(facet.ExtraTables);
            }
        }

        public static List<object[]> EntityModelTestData = new List<object[]>() {
            new object[] {
                typeof(FacetGroup),
                999,
                new Dictionary<string, object>() {
                    { "FacetGroupId", 999 },
                    { "FacetGroupKey", "ROOT" },
                    { "DisplayTitle", "ROOT"},
                    { "IsApplicable", false},
                    { "IsDefault", false}
                }
            },
            new object[] {
                typeof(FacetClause),
                4,
                new Dictionary<string, object>() {
                    { "FacetSourceTableId", 4 },
                    { "FacetId", 33  },
                    { "Clause", "metainformation.view_abundance.abundance is not null" }
                }
            },
            new object[] {
                typeof(Facet),
                25,
                new Dictionary<string, object>() {
                    { "FacetId", 25 },
                    { "FacetCode", "species" },
                    { "DisplayTitle", "Taxa" },
                    { "FacetGroupId", 6 },
                    { "FacetTypeId", EFacetType.Discrete },
                    { "CategoryIdExpr", "tbl_taxa_tree_master.taxon_id" },
                    { "CategoryNameExpr", "concat_ws(' ', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)" },
                    { "IconIdExpr", "tbl_taxa_tree_master.taxon_id" },
                    { "SortExpr", "tbl_taxa_tree_genera.genus_name||' '||tbl_taxa_tree_master.species" },
                    { "IsApplicable", true },
                    { "IsDefault", false },
                    { "AggregateType", "sum" },
                    { "AggregateTitle", "sum of Abundance" },
                    { "AggregateFacetId", 32 }
                }
            },
            new object[] {
                typeof(FacetTable),
                3,
                new Dictionary<string, object>() {
                    { "FacetTableId", 3 },
                    { "FacetId", 3 },
                    { "SequenceId", 1 },
                    { "SchemaName", "" },
                    { "TableName", "metainformation.tbl_denormalized_measured_values" },
                    { "Alias", "" }
                }
            },
            new object[] {
                typeof(FacetType),
                EFacetType.Geo,
                new Dictionary<string, object>() {
                    { "FacetTypeId", EFacetType.Geo },
                    { "FacetTypeName", "geo" },
                    { "ReloadAsTarget", true }
                }
            },
            new object[] {
                typeof(GraphTable),
                13,
                new Dictionary<string, object>() {
                    { "TableId", 13 },
                    { "TableName", "tbl_taxa_synonyms" }
                }
            },
            new object[] {
                typeof(GraphTableRelation),
                5,
                new Dictionary<string, object>() {
                    { "RelationId", 5 },
                    { "SourceTableId", 44 },
                    { "TargetTableId", 142 },
                    { "Weight", 20 },
                    { "SourceColumnName", "modification_type_id" },
                    { "TargetColumnName", "modification_type_id" }
                }
            },
            new object[] {
                typeof(ResultAggregate),
                3,
                new Dictionary<string, object>() {
                    { "AggregateId", 3 },
                    { "AggregateKey", "sample_group_level" },
                    { "DisplayText", "Sample group level" },
                    { "IsApplicable", false },
                    { "IsActivated", true },
                    { "InputType", "checkboxes" },
                    { "HasSelector", true }
                }
            },
            new object[] {
                typeof(ResultAggregateField),
                13,
                new Dictionary<string, object>() {
                    { "AggregateFieldId", 13 },
                    { "AggregateId", 1 },
                    { "ResultFieldId", 1 },
                    { "FieldTypeId", "sort_item" },
                    { "SequenceId", 99 }
                }
            },
            new object[] {
                typeof(ResultField),
                5,
                new Dictionary<string, object>() {
                    { "ResultFieldId", 5 },
                    { "ResultFieldKey", "site_link_filtered" },
                    { "TableName", "tbl_sites" },
                    { "ColumnName", "tbl_sites.site_id" },
                    { "DisplayText", "Filtered report" },
                    { "FieldTypeId", "link_item" },
                    { "Activated", true },
                    { "LinkUrl", "api/report/show_site_details.php?site_id" },
                    { "LinkLabel", "Show filtered report" }
                }
            },
            new object[] {
                typeof(ResultFieldType),
                "link_item",
                new Dictionary<string, object>() {
                    { "FieldTypeId", "link_item" },
                    { "IsResultValue", true },
                    { "SqlFieldCompiler", "TemplateFieldCompiler" },
                    { "IsAggregateField", false },
                    { "IsSortField", false },
                    { "IsItemField", true },
                    { "SqlTemplate", "{0}" }
                }
            }
        };

        [Theory]
        [MemberData(nameof(EntityModelTestData))]
        public void FacetGroup_Should_Be_Complete_When_Fetch_From_Context(Type type, object id, Dictionary<string, object> expected)
        {
            PropertyValuesForEntityShouldBeEqualToExpected(type, id, expected);
        }

    }
}
