using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SQT.Fixtures;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace SQT
{
    [Collection("JsonSeededFacetContext")]
    public class DisposableFacetContextContainer : IDisposable
    {
        private JsonSeededFacetContextFixture __fixture;

        private Lazy<DbConnection> __DbConnection;
        private Lazy<DbContextOptions> __DbContextOptions;
        private Lazy<FacetContext> __FacetContext;
        private Lazy<RepositoryRegistry> __RepositoryRegistry;
        private Lazy<ISetting> __Settings;

        public virtual JsonSeededFacetContextFixture Fixture => __fixture;
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
            => new RepositoryRegistry(FacetContext);

        public DisposableFacetContextContainer(JsonSeededFacetContextFixture fixture)
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

        public virtual FacetSetting FakeFacetSetting() => new SettingFactory().Create().Value.Facet;

        public virtual Mock<IRepositoryRegistry> MockRegistryWithFacetRepository()
        {
            // Default: a JSON-seeded FacetContext with Sqlite backend
            var mockRegistry = new Mock<IRepositoryRegistry>();
            mockRegistry.Setup(r => r.Facets).Returns(Registry.Facets);
            return mockRegistry;
        }

        public FacetsConfig2 FakeFacetsConfig(string uri) => new MockFacetsConfigFactory(Registry.Facets).Create(uri);

        public QuerySetup FakeQuerySetup(string uri) => new MockQuerySetupFactory(Registry).Scaffold(uri);

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
                        It.IsAny<List<string>>()
                    )).Returns(querySetup ?? new QuerySetup { });
            return mockQuerySetupBuilder;
        }

        /// <summary>
        /// Mocks ITypedQueryProxy.QueryRows. Returns passed fake items.
        /// </summary>
        /// <param name="fakeResult"></param>
        /// <returns></returns>
        public virtual Mock<ITypedQueryProxy> MockTypedQueryProxy(List<CategoryCountItem> fakeCategoryCountItems)
        {
            var mockQueryProxy = new Mock<ITypedQueryProxy>();
            mockQueryProxy.Setup(foo => foo.QueryRows<CategoryCountItem>(
                        It.IsAny<string>(),
                        It.IsAny<Func<IDataReader, CategoryCountItem>>()
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
        public virtual List<CategoryCountItem> FakeDiscreteCategoryCountItems(int count)
        {
            var fakeResult = new DiscreteCountDataReaderBuilder()
                .CreateNewTable()
                .GenerateBogusRows(count)
                .ToItems<CategoryCountItem>().ToList();
            return fakeResult;
        }

        /// <summary>
        /// Returns a list of generated CategoryCountItems for range results
        /// </summary>
        /// <param name="returnSql"></param>
        /// <returns></returns>
        public virtual List<CategoryCountItem> FakeRangeCategoryCountItems(int start, int size, int count)
        {
            var fakeResult = new RangeCountDataReaderBuilder(start, size)
                .CreateNewTable()
                .GenerateBogusRows(count)
                .ToItems<CategoryCountItem>().ToList();
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
                It.IsAny<string>(),
                It.IsAny<string>()
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
        public virtual Mock<IDiscreteCategoryCountQueryCompiler> MockDiscreteCategoryCountSqlCompiler(string returnSql)
        {
            var mockCategoryCountSqlCompiler = new Mock<IDiscreteCategoryCountQueryCompiler>();
            mockCategoryCountSqlCompiler.Setup(c => c.Compile(
                It.IsAny<QuerySetup>(),
                It.IsAny<Facet>(),
                It.IsAny<Facet>(),
                It.IsAny<string>()
            )).Returns(
                returnSql
            );
            return mockCategoryCountSqlCompiler;
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

        public virtual Mock<IJoinSqlCompiler> MockJoinSqlCompiler(string returnValue)
        {
            var mockJoinCompiler = new Mock<IJoinSqlCompiler>();
            mockJoinCompiler
                .Setup(x => x.Compile(
                    It.IsAny<TableRelation>(),
                    It.IsAny<FacetTable>(),
                    It.IsAny<bool>()
                ))
                .Returns(returnValue);
            return mockJoinCompiler;
        }

        public virtual Mock<IResultConfigCompiler> MockResultConfigCompiler(string returnSql, string facetCode = "result_facet")
        {
            var mockResultQueryCompiler = new Mock<IResultConfigCompiler>();
            mockResultQueryCompiler.Setup(
                c => c.Compile(
                    It.IsAny<FacetsConfig2>(),
                    It.IsAny<ResultConfig>(),
                    facetCode
                )
            ).Returns(returnSql);
            return mockResultQueryCompiler;
        }

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
                SourceTable = new Table { TableId = sourceName.GetHashCode(), TableOrUdfName = sourceName, IsUdf = sourceName.Contains("("), PrimaryKeyName = "X" },
                TargetTable = new Table { TableId = targetName.GetHashCode(), TableOrUdfName = targetName, IsUdf = targetName.Contains("("), PrimaryKeyName = "X" },
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
                .Setup(z => z.Compile(It.IsAny<QuerySetup>(), It.IsAny<Facet>(), It.IsAny<IEnumerable<ResultAggregateField>>()))
                .Returns(returnSql);
            var mockResultSqlCompilerLocator = new Mock<IResultSqlCompilerLocator>();
            mockResultSqlCompilerLocator
                .Setup(z => z.Locate(It.IsAny<string>()))
                .Returns(mockResultSqlCompiler.Object);
            return mockResultSqlCompilerLocator;
        }

        public virtual ResultConfig FakeResultConfig(string aggregateKey, string viewTypeId)
            => new ResultConfig
            {
                AggregateKeys = new System.Collections.Generic.List<string> { aggregateKey },
                RequestId = "1",
                ViewTypeId = viewTypeId,
                SessionId = "1"
            };

        protected virtual IEnumerable<ResultAggregateField> FakeResultAggregateFields(string aggregateKey, string viewTypeId)
        {
            ResultConfig resultConfig = FakeResultConfig(aggregateKey, viewTypeId);
            var fields = Registry.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
            return fields;
        }

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
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(object1, object2);

            return result.AreEqual;
        }

        public static void DumpUriObject(string uri, object value)
        {
            dynamic expando = new ExpandoObject();
            expando.Uri = uri;
            expando.ValueType = value.GetType().Name;
            expando.Value = value;
            ScaffoldUtility.Dump(expando, $@"C:\TEMP\{value.GetType().Name}_{uri.Replace(":", "#").Replace("/", "+")}.cs"); ;
        }
    }
}