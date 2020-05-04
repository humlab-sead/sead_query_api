using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Collections.Generic;

namespace SQT.Fixtures
{
    public class MockQuerySetupFactory
    {
        public IRepositoryRegistry Registry { get; set; }

        public MockQuerySetupFactory(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        protected Mock<IPickFilterCompiler> MockPickFilterCompiler(string returnCriteria)
        {
            var mock = new Mock<IPickFilterCompiler>();
            mock.Setup(x => x.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>()))
                .Returns(returnCriteria);
            return mock;
        }

        protected Mock<IPicksFilterCompiler> MockPicksFilterCompiler(IEnumerable<string> returnCriteria)
        {
            var mock = new Mock<IPicksFilterCompiler>();
            mock.Setup(x => x.Compile(It.IsAny<Facet>(), It.IsAny<List<FacetConfig2>>()))
                .Returns(returnCriteria);
            return mock;
        }

        protected IPicksFilterCompiler MockPicksFilterCompiler(Mock<IPickFilterCompiler> discreteMock, Mock<IPickFilterCompiler> rangeMock)
        {
            var locator = new Mock<IPickFilterCompilerLocator>();
            locator.Setup(x => x.Locate(EFacetType.Discrete)).Returns(discreteMock.Object);
            locator.Setup(x => x.Locate(EFacetType.Range)).Returns(rangeMock.Object);
            var compiler = new PicksFilterCompiler(locator.Object);
            return compiler;
        }

        protected Mock<IJoinSqlCompiler> MockJoinCompiler(string fakeJoin)
        {
            var mock = new Mock<IJoinSqlCompiler>();
            mock.Setup(x => x.Compile(It.IsAny<TableRelation>(), It.IsAny<FacetTable>(), It.IsAny<bool>()))
                .Returns(fakeJoin);
            return mock;
        }

        public QuerySetup CountOrContentQuerySetup(FacetsConfig2 facetsConfig)
        {
            var fakeJoin = "INNER JOIN A as A.id = B.id";
            var joinCompiler = MockJoinCompiler(fakeJoin);
            var fakePickCriteria = new List<string> { "ID IN (1,2,3)" };
            var mockPicksCompiler = MockPicksFilterCompiler(fakePickCriteria);
            var facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);

            // FIXME: Should be mocked
            var compiler = new QuerySetupBuilder(facetsGraph, mockPicksCompiler.Object, joinCompiler.Object);

            var querySetup = compiler.Build(
                facetsConfig,
                facetsConfig.TargetFacet,
                new List<string>(),
                null
            );

            return querySetup;
        }

        public QuerySetup ResultQuerySetup(FacetsConfig2 facetsConfig, string resultFacetCode, string aggregateKey)
        {
            var resultFields = Registry.Results.GetFieldsByKey(aggregateKey);
            var fakeJoin = "INNER JOIN A as A.id = B.id";
            var joinCompiler = MockJoinCompiler(fakeJoin);
            var fakePickCriteria = new List<string> { "ID IN (1,2,3)" };
            var mockPicksCompiler = MockPicksFilterCompiler(fakePickCriteria);
            var facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);
            var resultFacet = Registry.Facets.GetByCode(resultFacetCode);

            // FIXME: Should be mocked
            var compiler = new QuerySetupBuilder(facetsGraph, mockPicksCompiler.Object, joinCompiler.Object);
            var querySetup = compiler.Build(facetsConfig, resultFacet, resultFields);

            return querySetup;
        }
    }
}
