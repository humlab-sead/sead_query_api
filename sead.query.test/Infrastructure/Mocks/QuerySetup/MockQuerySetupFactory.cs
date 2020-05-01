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

        private Mock<PickFilterCompilerLocator> ConcretePickCompilers()
        {
            var mock = new Mock<PickFilterCompilerLocator>(It.IsAny<IIndex<int, IPickFilterCompiler>>());
            mock.Setup(x => x.Locate(It.Is<EFacetType>(x => x == EFacetType.Discrete)))
                .Returns(new DiscreteFacetPickFilterCompiler());
            mock.Setup(x => x.Locate(It.Is<EFacetType>(x => x == EFacetType.Range)))
                .Returns(new RangeFacetPickFilterCompiler());
            return mock;
        }

        //private FacetsConfig2 FakeFacetsConfig(string uri)
        //{
        //    return new MockFacetsConfigFactory(Registry.Facets).Create(uri);
        //}

        //public QuerySetup Scaffold(string uri, string targetCode = null, List<string> extraTables = null)
        //{
        //    return Scaffold(FakeFacetsConfig(uri), targetCode, extraTables);
        //}

        public QuerySetup Scaffold(FacetsConfig2 facetsConfig, string targetCode = null, List<string> extraTables = null)
        {
            var joinCompiler = new JoinSqlCompiler();
            var pickCompilers = ConcretePickCompilers();
            var facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);
            var facetCodes = facetsConfig.GetFacetCodes().AddIfMissing(facetsConfig.TargetFacet.FacetCode);
            var targetFacet = Registry.Facets.GetByCode(targetCode ?? facetsConfig.TargetCode);

            QuerySetupBuilder compiler = new QuerySetupBuilder(facetsGraph, pickCompilers.Object, joinCompiler);

            var querySetup = compiler.Build(
                facetsConfig,
                targetFacet,
                extraTables ?? new List<string>(),
                facetCodes
            );
            return querySetup;
        }

    }
}
