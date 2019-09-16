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

namespace SeadQueryTest2.Model
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
                typeof(GraphNode),
                13,
                new Dictionary<string, object>() {
                    { "TableId", 13 },
                    { "TableName", "tbl_taxa_synonyms" }
                }
            },
            new object[] {
                typeof(GraphEdge),
                5,
                new Dictionary<string, object>() {
                    { "EdgeId", 5 },
                    { "SourceNodeId", 44 },
                    { "TargetNodeId", 142 },
                    { "Weight", 20 },
                    { "SourceKeyName", "modification_type_id" },
                    { "TargetKeyName", "modification_type_id" }
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
