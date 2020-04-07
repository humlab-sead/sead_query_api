using System;
using System.Collections.Generic;
using System.Text;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;

namespace SeadQueryTest.Fixtures
{

    public class StoreBuilder : DisposableFacetContextContainer
    {
        public StoreBuilder(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        /// <summary>
        /// Reads out facets entities in Context and caches them in a Dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Facet> Build() { 
            return Registry.Facets.ToDictionary();
        }
    }

    public static class FacetFixtures
    {

        public static Dictionary<string, Facet> __Store = null;

        private static readonly object lockObject = new object();

        public static Dictionary<string, Facet> Store
        {
            get {
                if (__Store == null) {
                    lock (lockObject) {
                        __Store = new StoreBuilder(new JsonSeededFacetContextFixture()).Build();
                    }
                }
                return Store;
            }
        }

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
