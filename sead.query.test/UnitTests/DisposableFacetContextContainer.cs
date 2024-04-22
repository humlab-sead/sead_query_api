using Autofac.Features.Indexed;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Plugin;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

namespace SQT
{
    public class DisposableFacetContextContainer : IDisposable
    {
        private readonly JsonFacetContextFixture __fixture;

        private readonly Lazy<DbConnection> __DbConnection;
        private readonly Lazy<DbContextOptions> __DbContextOptions;
        private readonly Lazy<FacetContext> __FacetContext;
        private readonly Lazy<RepositoryRegistry> __RepositoryRegistry;
        private readonly Lazy<ISetting> __Settings;

        public virtual JsonFacetContextFixture Fixture => __fixture;
        public virtual ISetting Settings => __Settings.Value;
        public virtual DbConnection DbConnection => __DbConnection.Value;
        public virtual DbContextOptions DbContextOptions => __DbContextOptions.Value;
        public virtual FacetContext FacetContext => __FacetContext.Value;
        public virtual RepositoryRegistry Registry => __RepositoryRegistry.Value;

        public virtual ISetting CreateSettings()
            => (ISetting)new SettingFactory().Create().Value;

        public virtual DbConnection CreateDbConnection()
            => SqliteConnectionFactory.CreateAndOpen();

        public virtual DbContextOptions CreateDbContextOptions()
            => SqliteContextOptionsFactory.Create(DbConnection);

        public FacetContext CreateFacetContext()
            => JsonSeededFacetContextFactory.Create(DbContextOptions, Fixture);

        public virtual RepositoryRegistry CreateRepositoryRegistry()
            => new(FacetContext);

        public DisposableFacetContextContainer(JsonFacetContextFixture fixture)
        {
            __fixture = fixture;
            __DbConnection = new Lazy<DbConnection>(CreateDbConnection);
            __DbContextOptions = new Lazy<DbContextOptions>(CreateDbContextOptions);
            __FacetContext = new Lazy<FacetContext>(CreateFacetContext);
            __RepositoryRegistry = new Lazy<RepositoryRegistry>(CreateRepositoryRegistry);
            __Settings = new Lazy<ISetting>(CreateSettings);
        }

        public void Dispose()
        {
            if (__DbConnection.IsValueCreated)
                DbConnection.Close();

            if (__FacetContext.IsValueCreated)
                FacetContext.Dispose();

            if (__RepositoryRegistry.IsValueCreated)
                Registry.Dispose();

            if (__DbConnection.IsValueCreated)
                DbConnection.Dispose();
        }

        // Common mock helpers

        public virtual IRepositoryRegistry FakeRegistry() => Registry;
        public virtual IFacetRepository Facets => FakeRegistry().Facets;
        public virtual IResultSpecificationRepository Results => FakeRegistry().Results;

        public virtual FacetSetting FakeFacetSetting() => new SettingFactory().Create().Value.Facet;

        public virtual Mock<IRepositoryRegistry> MockRegistryWithFacetRepository()
        {
            // Default: a JSON-seeded FacetContext with Sqlite backend
            var mockRegistry = new Mock<IRepositoryRegistry>();
            mockRegistry.Setup(r => r.Facets).Returns(Registry.Facets);
            return mockRegistry;
        }

        public virtual Facet FakeFacet(
            string facetCode,
            FacetType t,
            FacetGroup g,
            bool is_applicable = true
        ) =>
            new()
            {
                FacetCode = facetCode,
                CategoryIdExpr = Guid.NewGuid().ToString(),
                CategoryNameExpr = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayTitle = Guid.NewGuid().ToString(),
                AggregateType = "",
                AggregateTitle = "",
                AggregateFacetId = 0,
                SortExpr = "",
                FacetGroup = g,
                FacetType = t,
                IsApplicable = is_applicable
            };

        public FacetsConfig2 FakeFacetsConfig(string uri)
            => new MockFacetsConfigFactory(Registry.Facets).Create(uri);

        public virtual Mock<IPickFilterCompiler> MockPickFilterCompiler(string returnCriteria)
        {
            var mock = new Mock<IPickFilterCompiler>();
            mock.Setup(x => x.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>()))
                .Returns(returnCriteria);
            return mock;
        }

        protected Mock<IPicksFilterCompiler> MockPicksFilterCompiler(IEnumerable<string> returnPicks)
        {
            var mock = new Mock<IPicksFilterCompiler>();
            mock.Setup(x => x.Compile(It.IsAny<Facet>(), It.IsAny<List<FacetConfig2>>()))
                .Returns(returnPicks);
            return mock;
        }

        public virtual IPicksFilterCompiler MockPicksFilterCompiler(Mock<IPickFilterCompiler> discreteMock, Mock<IPickFilterCompiler> rangeMock)
        {
            var locator = new Mock<IPickFilterCompilerLocator>();
            locator.Setup(x => x.Locate(EFacetType.Discrete)).Returns(discreteMock.Object);
            locator.Setup(x => x.Locate(EFacetType.Range)).Returns(rangeMock.Object);
            var compiler = new PicksFilterCompiler(locator.Object);
            return compiler;
        }

        protected virtual List<string> FakeJoinsClause(int nCount)
            => Enumerable.Range(0, nCount)
                .Select(i => (L: Convert.ToChar('A' + (char)i), R: Convert.ToChar('A' + (char)(i + 1))))
                .Select(x => $" INNER JOIN {x.L} ON {x.L}.{x.L.ToString().ToLower()}_id = {x.R}.{x.R.ToString().ToLower()}_id ")
                .ToList();

        public virtual Mock<IJoinSqlCompiler> MockJoinSqlCompiler(string returnValue)
        {
            var mockCompiler = new Mock<IJoinSqlCompiler>();
            mockCompiler
                .Setup(x => x.Compile(It.IsAny<TableRelation>(), It.IsAny<FacetTable>(), It.IsAny<bool>()))
                .Returns(returnValue);
            return mockCompiler;
        }

        protected virtual Mock<IJoinsClauseCompiler> MockJoinsClauseCompiler(List<string> fakeJoins)
        {
            var mock = new Mock<IJoinsClauseCompiler>();
            mock.Setup(x => x.Compile(
                It.IsAny<FacetsConfig2>(),
                It.IsAny<Facet>(),
                It.IsAny<List<string>>())
            ).Returns(fakeJoins);
            return mock;
        }

        public QuerySetup FakeCountOrContentQuerySetup(FacetsConfig2 facetsConfig, string pickCriteria = null)
        {
            List<string> fakeJoins = FakeJoinsClause(5);
            var joinsCompiler = MockJoinsClauseCompiler(fakeJoins);
            var fakePickCriteria = new List<string> {  pickCriteria ?? "ID IN (1,2,3)" };
            var mockPicksCompiler = MockPicksFilterCompiler(fakePickCriteria);
            var facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);

            // FIXME: Should be mocked
            var compiler = new QuerySetupBuilder(facetsGraph, mockPicksCompiler.Object, joinsCompiler.Object);

            var querySetup = compiler.Build(
                facetsConfig,
                facetsConfig.TargetFacet,
                new List<string>(),
                null
            );

            return querySetup;
        }

        public QuerySetup FakeResultQuerySetup(FacetsConfig2 facetsConfig, string resultFacetCode, string specificationKey)
        {
            var resultFields = Registry.Results.GetFieldsByKey(specificationKey);
            var fakeJoins = FakeJoinsClause(5);
            var joinCompiler = MockJoinsClauseCompiler(fakeJoins);
            var fakePickCriteria = new List<string> { "ID IN (1,2,3)" };
            var mockPicksCompiler = MockPicksFilterCompiler(fakePickCriteria);
            var facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);
            var resultFacet = Registry.Facets.GetByCode(resultFacetCode);

            // FIXME: Should be mocked
            var compiler = new QuerySetupBuilder(facetsGraph, mockPicksCompiler.Object, joinCompiler.Object);
            var querySetup = compiler.Build(facetsConfig, resultFacet, resultFields);

            return querySetup;
        }

        /// <summary>
        /// Mocks IQuerySetupBuilder.Setup. Returns passed argument.
        /// </summary>
        /// <param name="querySetup"></param>
        /// <returns></returns>
        public virtual Mock<IQuerySetupBuilder> MockQuerySetupBuilder(QuerySetup querySetup)
        {
            var mockQuerySetupBuilder = new Mock<IQuerySetupBuilder>();
            mockQuerySetupBuilder.Setup(x => x.Build(
                        It.IsAny<FacetsConfig2>(),
                        It.IsAny<Facet>(),
                        It.IsAny<List<string>>(),
                        It.IsAny<List<string>>()
                    )).Returns(querySetup ?? new QuerySetup());
            return mockQuerySetupBuilder;
        }

        /// <summary>
        /// Mocks ITypedQueryProxy.QueryRows. Returns passed fake items.
        /// </summary>
        /// <param name="fakeResult"></param>
        /// <returns></returns>
        public virtual Mock<ITypedQueryProxy> MockTypedQueryProxy(List<CategoryItem> fakeCategoryCountItems)
        {
            var mockQueryProxy = new Mock<ITypedQueryProxy>();
            mockQueryProxy.Setup(foo => foo.QueryRows<CategoryItem>(
                        It.IsAny<string>(),
                        It.IsAny<Func<IDataReader, CategoryItem>>()
                )).Returns(
                    fakeCategoryCountItems
                );
            return mockQueryProxy;
        }

        /// <summary>
        /// Returns a list of generated CategoryCountItems for range results
        /// </summary>
        /// <param name="returnSql"></param>
        /// <returns></returns>
        public virtual List<CategoryItem> FakeDiscreteCategoryCountItems(int count)
        {
            var fakeResult = new DiscreteCountDataReaderBuilder()
                .CreateNewTable()
                .GenerateBogusRows(count)
                .ToItems<CategoryItem>().ToList();
            return fakeResult;
        }

        /// <summary>
        /// Returns a list of generated CategoryCountItems for range results
        /// </summary>
        /// <param name="returnSql"></param>
        /// <returns></returns>
        public virtual List<CategoryItem> FakeRangeCategoryCountItems(int start, int size, int count)
        {
            var fakeResult = new RangeCountDataReaderBuilder(start, size)
                .CreateNewTable()
                .GenerateBogusRows(count)
                .ToItems<CategoryItem>().ToList();
            return fakeResult;
        }

        /// <summary>
        /// Mocks Compile method. Return passed SQL. No other calls avaliable.
        /// </summary>
        /// <param name="returnSql"></param>
        /// <returns></returns>
        public virtual Mock<IRangeCategoryCountSqlCompiler> MockRangeCategoryCountSqlCompiler(string returnSql)
        {
            var mockCategoryCountSqlCompiler = new Mock<IRangeCategoryCountSqlCompiler>();
            mockCategoryCountSqlCompiler.Setup(c => c.Compile(
                It.IsAny<QuerySetup>(),
                It.IsAny<Facet>(),
                It.IsAny<CompilePayload>()
            )).Returns(
                returnSql
            );
            return mockCategoryCountSqlCompiler;
        }

        /// <summary>
        /// Mocks Compile method. Return passed SQL. No other calls avaliable.
        /// </summary>
        /// <param name="returnSql"></param>
        /// <returns></returns>
        public virtual Mock<IDiscreteCategoryCountSqlCompiler> MockDiscreteCategoryCountSqlCompiler(string returnSql)
        {
            var mockCategoryCountSqlCompiler = new Mock<IDiscreteCategoryCountSqlCompiler>();
            mockCategoryCountSqlCompiler.Setup(c => c.Compile(
                It.IsAny<QuerySetup>(),
                It.IsAny<Facet>(),
                It.IsAny<CompilePayload>()
            )).Returns(
                returnSql
            );
            return mockCategoryCountSqlCompiler;
        }

        public virtual Mock<IIndex<EFacetType, ICategoryCountHelper>> MockCategoryCountHelpers()
        {
            var config = new SettingFactory().Create().Value.Facet;
            var mockHelpers = new Mock<IIndex<EFacetType, ICategoryCountHelper>>();
            mockHelpers.Setup(x => x[EFacetType.Range])
                .Returns(new RangeCategoryCountHelper(config));
            mockHelpers.Setup(x => x[EFacetType.Discrete])
                .Returns(new DiscreteCategoryCountHelper());
            return mockHelpers;
        }

        public virtual Mock<IIndex<EFacetType, ICategoryInfoService>> MockCategoryInfoServices()
        {
            var mockServices = new Mock<IIndex<EFacetType, ICategoryInfoService>>();
            var mockRangeCategoryInfoService = new Mock<ICategoryInfoService>();
            mockRangeCategoryInfoService.Setup(c => c.GetCategoryInfo(
                It.IsAny<FacetsConfig2>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).Returns(
                new FacetContent.CategoryInfo
                {
                    Count = 99,
                    Query = "dumy-sql",
                    Extent = [1, 100]
                }
            );
            mockRangeCategoryInfoService.Setup(c => c.SqlCompiler).Returns(
                new Mock<ICategoryInfoSqlCompiler>().Object
            );

            var mockDiscreteCategoryInfoService = new Mock<ICategoryInfoService>();
            mockDiscreteCategoryInfoService.Setup(c => c.GetCategoryInfo(
                It.IsAny<FacetsConfig2>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).Returns(
                new FacetContent.CategoryInfo
                {
                    Count = 88,
                    Query = "dumy-sql",
                    Extent = [1]
                }
            );
            mockDiscreteCategoryInfoService.Setup(c => c.SqlCompiler).Returns(
                new Mock<ICategoryInfoSqlCompiler>().Object
            );
            mockServices.Setup(x => x[EFacetType.Range])
                .Returns(mockRangeCategoryInfoService.Object);
            mockServices.Setup(x => x[EFacetType.Discrete])
                .Returns(mockDiscreteCategoryInfoService.Object);
            return mockServices;
        }

        public virtual Mock<IIndex<EFacetType, ICategoryCountSqlCompiler>> MockCategoryCountSqlCompilers(string returnSql)
        {
            var mockRangeCountSqlCompiler = MockRangeCategoryCountSqlCompiler(returnSql: returnSql);
            var mockDiscreteCountSqlCompiler = MockDiscreteCategoryCountSqlCompiler(returnSql: returnSql);
            var mockSqlCompilers = new Mock<IIndex<EFacetType, ICategoryCountSqlCompiler>>();
            mockSqlCompilers.Setup(x => x[EFacetType.Range]).Returns(mockRangeCountSqlCompiler.Object);
            mockSqlCompilers.Setup(x => x[EFacetType.Discrete]).Returns(mockDiscreteCountSqlCompiler.Object);

            return mockSqlCompilers;
        }

        public virtual Mock<IPickFilterCompilerLocator> MockPickCompilerLocator(string returnValue = "")
        {
            var mockPickCompiler = new Mock<IPickFilterCompiler>();
            var HasNoPicks = Match.Create<FacetConfig2>(x => !x.HasPicks());

            mockPickCompiler
                .Setup(foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>()))
                .Returns(returnValue);

            var mockLocator = new Mock<IPickFilterCompilerLocator>();

            mockLocator
                .Setup(x => x.Locate(It.IsAny<EFacetType>()))
                .Returns(mockPickCompiler.Object);

            return mockLocator;
        }

        public virtual Mock<IPickFilterCompilerLocator> MockConcretePickCompilerLocator()
        {
            var mockLocator = new Mock<IPickFilterCompilerLocator>();
            mockLocator
                .Setup(x => x.Locate(EFacetType.Discrete))
                .Returns(new DiscreteFacetPickFilterCompiler());

            mockLocator
                .Setup(x => x.Locate(EFacetType.Range))
                .Returns(new RangeFacetPickFilterCompiler());

            return mockLocator;
        }

        //public virtual Mock<IResultConfigCompiler> MockResultConfigCompiler(string returnSql, string facetCode = "result_facet")
        //{
        //    var mockResultQueryCompiler = new Mock<IResultConfigCompiler>();
        //    mockResultQueryCompiler.Setup(
        //        c => c.Compile(
        //            It.IsAny<FacetsConfig2>(),
        //            It.IsAny<ResultConfig>(),
        //            facetCode
        //        )
        //    ).Returns(returnSql);
        //    return mockResultQueryCompiler;
        //}

        public virtual Mock<IFacetsGraph> MockFacetsGraph(List<GraphRoute> returnRoutes)
        {
            var mockFacetsGraph = new Mock<IFacetsGraph>();

            mockFacetsGraph
                .Setup(x => x.Find(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>()))
                    .Returns(returnRoutes);

            return mockFacetsGraph;
        }

        public TableRelation FakeTableRelation(string sourceName, string targetName)
        {
            //try {
            //    var repository = Registry.TableRelations;
            //    return repository.FindByName(sourceName, targetName) ??
            //        (TableRelation)repository.FindByName(targetName, sourceName).Reverse();
            //} catch (System.NullReferenceException ex) {
            //    throw;
            //}

            return new TableRelation
            {
                SourceTable = new Table { TableId = sourceName.GetHashCode(), TableOrUdfName = sourceName, IsUdf = sourceName.Contains('('), PrimaryKeyName = "X" },
                TargetTable = new Table { TableId = targetName.GetHashCode(), TableOrUdfName = targetName, IsUdf = targetName.Contains('('), PrimaryKeyName = "X" },
                SourceColumName = sourceName + "_id",
                TargetColumnName = targetName + "_id",
                Weight = 20
            };
        }

        public GraphRoute FakeRoute(string[] nodePairs)
        {
            var graphRoute = new GraphRoute(
                nodePairs.Select(x => x.Split("/"))
                    .Select(z => new { Source = z[0], Target = z[1] })
                    .Select(z => FakeTableRelation(z.Source, z.Target))
            );
            return graphRoute;
        }

        public GraphRoute FakeRoute2(params string[] trail)
        {
            return FakeRoute(RouteHelper.ToPairs(trail));
        }

        public List<GraphRoute> FakeRoutes(List<string[]> nodePairs)
        {
            var graphRoutes = nodePairs.Select(z => FakeRoute(z)).ToList();
            return graphRoutes;
        }

        public virtual Mock<IResultSqlCompilerLocator> MockResultSqlCompilerLocator(string returnSql)
        {
            var mockResultSqlCompiler = new Mock<IResultSqlCompiler>();
            mockResultSqlCompiler
                .Setup(z => z.Compile(It.IsAny<QuerySetup>(), It.IsAny<Facet>(), It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(returnSql);
            var mockResultSqlCompilerLocator = new Mock<IResultSqlCompilerLocator>();
            mockResultSqlCompilerLocator
                .Setup(z => z.Locate(It.IsAny<string>()))
                .Returns(mockResultSqlCompiler.Object);
            return mockResultSqlCompilerLocator;
        }

        public virtual ResultConfig FakeResultConfig(string facetCode, string specificationKey, string viewTypeId)
            => ResultConfigFactory.Create(Facets.GetByCode(facetCode), Results.GetByKey(specificationKey), viewTypeId);

        protected virtual Mock<DiscreteCategoryInfoSqlCompiler> MockDiscreteContentSqlCompiler(string returnSql)
        {
            var mock = new Mock<DiscreteCategoryInfoSqlCompiler>();
            mock.Setup(
                x => x.Compile(It.IsAny<QuerySetup>(), It.IsAny<Facet>(), It.IsAny<string>())
            ).Returns(returnSql);
            return mock;
        }

        protected Mock<ICategoryCountService> MockCategoryCountService(List<CategoryItem> fakeCategoryCountItems)
        {
            var mockCategoryCountService = new Mock<ICategoryCountService>();
            mockCategoryCountService.Setup(
                x => x.Load(It.IsAny<string>(), It.IsAny<FacetsConfig2>(), EFacetType.Unknown)
            ).Returns(
                new CategoryCountService.CategoryCountData
                {
                    CategoryInfo = new FacetContent.CategoryInfo
                    {
                        Count = fakeCategoryCountItems.Count,
                        Query = "all-categories-sql"
                    },
                    CategoryCounts = fakeCategoryCountItems.ToDictionary(z => z.Category),
                    OuterCategoryCounts = fakeCategoryCountItems,
                    SqlQuery = "SELECT * FROM bla.bla"
                }
            );
            return mockCategoryCountService;
        }

        // protected Mock<ICategoryCountServiceLocator> MockCategoryCountServiceLocator(List<CategoryCountItem> fakeCategoryCountItems)
        // {
        //     var mockCategoryCountService = MockCategoryCountService(fakeCategoryCountItems);

        //     var mockCategoryCountServiceLocator = new Mock<ICategoryCountServiceLocator>();
        //     mockCategoryCountServiceLocator
        //         .Setup(z => z.Locate(It.IsAny<EFacetType>()))
        //         .Returns(
        //             mockCategoryCountService.Object
        //         );
        //     return mockCategoryCountServiceLocator;
        // }

        public static class RouteHelper
        {
            public static string[] ToPairs(params string[] trail)
            {
                return trail.Take(trail.Length - 1).Select((e, i) => e + "/" + trail[i + 1]).ToArray();
            }

            public static string[] ToPairs(List<string> trail)
            {
                return ToPairs(trail.ToArray());
            }
        }

        public static bool AreEqualByProperty(object object1, object object2)
        {
            CompareLogic compareLogic = new();
            ComparisonResult result = compareLogic.Compare(object1, object2);

            return result.AreEqual;
        }

        public static void DumpUriObject(string uri, object value)
        {
            dynamic expando = new ExpandoObject();
            expando.Uri = uri;
            expando.ValueType = value.GetType().Name;
            expando.Value = value;
            ScaffoldUtility.Dump(expando, $@"C:\TEMP\{value.GetType().Name}_{uri.Replace(":", "#").Replace("/", "+")}.cs");
        }
    }
}
