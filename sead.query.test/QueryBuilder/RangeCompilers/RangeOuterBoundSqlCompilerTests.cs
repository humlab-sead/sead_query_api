using DataAccessPostgreSqlProvider;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeOuterBoundSqlCompilerTests
    {
        private IFacetSetting mockSettings;
        private RepositoryRegistry mockRegistry;
        private FacetContext mockContext;

        public object ReconstituteFacetConfigService { get; private set; }

        public RangeOuterBoundSqlCompilerTests()
        {
            mockSettings = new MockOptionBuilder().Build().Value.Facet;
            mockContext = ScaffoldUtility.DefaultFacetContext();
            mockRegistry = new RepositoryRegistry(mockContext);
        }

        private RangeOuterBoundSqlCompiler CreateRangeOuterBoundSqlCompiler()
        {
            return new RangeOuterBoundSqlCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeOuterBoundSqlCompiler = this.CreateRangeOuterBoundSqlCompiler();
            var facetCode = "tbl_denormalized_measured_values_33_0";

            var facet = new Facet {
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
                    FacetGroupKey = "measured_values",
                    DisplayTitle = "Measured values",
                    IsApplicable = true,
                    IsDefault = false
                },
                Tables = new List<FacetTable> {
                    new FacetTable
                    {
                        FacetTableId = 3,
                        FacetId = 3,
                        SequenceId = 1,
                        SchemaName = "",
                        ObjectName = "facet.method_measured_values",
                        ObjectArgs = "(33, 0)",
                        Alias = "",
                    }
                },
                Clauses = new List<FacetClause> {
                }
            };

            var targetFacetConfig = new FacetConfig2 {
                FacetCode = facetCode,
                Position = 1,
                TextFilter = "",
                Picks = new List<FacetConfigPick> {
                },
                Facet = facet
            };

            var sqlJoins = new List<string>() { };
            var criterias = new Dictionary<string, string>() { };
            var routes = new List<GraphRoute>() { };
            var reducedRoutes = new List<GraphRoute>() { };

            QuerySetup query = new QuerySetup(
                targetFacetConfig,
                facet,
                sqlJoins,
                criterias,
                routes,
                reducedRoutes
            );

            // Act
            var result = rangeOuterBoundSqlCompiler.Compile(
                query,
                facet
            );

            // Assert
            Assert.True(false);
        }
    }
}
