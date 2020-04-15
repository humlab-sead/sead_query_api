using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using Xunit;

namespace SeadQueryTest
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

        protected virtual ISetting CreateSettings()
            => (ISetting)new SettingFactory().Create().Value;

        protected virtual DbConnection CreateDbConnection()
            => SqliteConnectionFactory.CreateAndOpen();

        protected virtual DbContextOptions CreateDbContextOptions()
            => SqliteContextOptionsFactory.Create(DbConnection);

        private FacetContext CreateFacetContext()
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

        protected virtual IRepositoryRegistry FakeRegistry() => Registry;

        protected virtual FacetSetting FakeFacetSetting() => new SettingFactory().Create().Value.Facet;

        protected virtual Mock<IRepositoryRegistry> MockFacetRepository()
        {
            // Default: a JSON-seeded FacetContext with Sqlite backend
            var mockRegistry = new Mock<IRepositoryRegistry>();
            mockRegistry.Setup(r => r.Facets).Returns(Registry.Facets);
            return mockRegistry;
        }

        /// <summary>
        /// Mocks IQuerySetupBuilder.Setup. Returns passed argument.
        /// </summary>
        /// <param name="querySetup"></param>
        /// <returns></returns>
        protected virtual Mock<IQuerySetupBuilder> MockQuerySetupBuilder(QuerySetup querySetup)
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
        protected virtual Mock<ITypedQueryProxy> MockTypedQueryProxy(List<CategoryCountItem> fakeResult)
        {
            var mockQueryProxy = new Mock<ITypedQueryProxy>();
            mockQueryProxy.Setup(foo => foo.QueryRows<CategoryCountItem>(
                        It.IsAny<string>(),
                        It.IsAny<Func<IDataReader, CategoryCountItem>>()
                )).Returns(
                    fakeResult
                );
            return mockQueryProxy;
        }

        /// <summary>
        /// Returns a list of generated CategoryCountItems for range results
        /// </summary>
        /// <param name="returnSql"></param>
        /// <returns></returns>
        protected virtual List<CategoryCountItem> MockDiscreteCategoryCountItems(int start, int step, int count)
        {
            var fakeResult = new RangeCountDataReaderBuilder(start, step)
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
        protected virtual List<CategoryCountItem> MockRangeCategoryCountItems(int start, int step, int count)
        {
            var fakeResult = new RangeCountDataReaderBuilder(start, step)
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
        protected virtual Mock<IRangeCategoryCountSqlCompiler> MockRangeCategoryCountSqlQueryCompiler(string returnSql)
        {
            var mockCategoryCountSqlQueryCompiler = new Mock<IRangeCategoryCountSqlCompiler>();
            mockCategoryCountSqlQueryCompiler.Setup(c => c.Compile(
                It.IsAny<QuerySetup>(),
                It.IsAny<Facet>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(
                returnSql
            );
            return mockCategoryCountSqlQueryCompiler;
        }

        /// <summary>
        /// Mocks Compile method. Return passed SQL. No other calls avaliable.
        /// </summary>
        /// <param name="returnSql"></param>
        /// <returns></returns>
        protected virtual Mock<IDiscreteCategoryCountQueryCompiler> MockDiscreteCategoryCountSqlQueryCompiler(string returnSql)
        {
            var mockCategoryCountSqlQueryCompiler = new Mock<IDiscreteCategoryCountQueryCompiler>();
            mockCategoryCountSqlQueryCompiler.Setup(c => c.Compile(
                It.IsAny<QuerySetup>(),
                It.IsAny<Facet>(),
                It.IsAny<Facet>(),
                It.IsAny<string>()
            )).Returns(
                returnSql
            );
            return mockCategoryCountSqlQueryCompiler;
        }
        protected virtual Mock<IPickFilterCompilerLocator> MockPickCompilerLocator(string returnValue = "")
        {
            var mockPickCompiler = new Mock<IPickFilterCompiler>();

            mockPickCompiler
                .Setup(foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>()))
                .Returns(returnValue);

            var mockLocator = new Mock<IPickFilterCompilerLocator>();

            mockLocator
                .Setup(x => x.Locate(It.IsAny<EFacetType>()))
                .Returns(mockPickCompiler.Object);

            return mockLocator;
        }

        protected virtual Mock<IJoinSqlCompiler> MockJoinSqlCompiler(string returnValue)
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

        protected virtual Mock<IFacetsGraph> MockFacetsGraph(List<GraphRoute> returnRoutes)
        {
            var mockFacetsGraph = new Mock<IFacetsGraph>();

            mockFacetsGraph
                .Setup(x => x.Find(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>()))
                    .Returns(returnRoutes);

            return mockFacetsGraph;
        }

        protected GraphRoute MockGraphRoute(string[] nodePairs)
        {
            var graphRoute = new GraphRoute(nodePairs.Select(z => Registry.TableRelations.FindByName(z.Split("/")[0], z.Split("/")[1])));
            return graphRoute;
        }
        protected GraphRoute MockGraphRoute2(params string[] trail)
        {
            return MockGraphRoute(RouteHelper.ToPairs(trail));
        }

        protected List<GraphRoute> MockGraphRoutes(List<string[]> nodePairs)
        {
            var graphRoutes = nodePairs.Select(z => MockGraphRoute(z)).ToList();
            return graphRoutes;
        }

        protected static class RouteHelper
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

    }
}
