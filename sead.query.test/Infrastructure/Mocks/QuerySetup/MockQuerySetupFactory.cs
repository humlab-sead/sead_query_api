using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;
using SeadQueryInfra;
using SeadQueryCore.QueryBuilder;
using Autofac.Features.Indexed;
using SeadQueryTest.Fixtures;
using SeadQueryTest;
using Xunit;
using System;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using Moq;

namespace SeadQueryTest.Fixtures
{
    public class MockQuerySetupFactory
    {
        public IRepositoryRegistry Registry { get; set; }

        public MockQuerySetupFactory(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        private Facet GetFacet(string facetCode) => !string.IsNullOrEmpty(facetCode) ? Registry.Facets.GetByCode(facetCode) : null;
        // FacetInstances.Store[facetCode]

        private Mock<PickFilterCompilerLocator> ConcretePickCompilers()
        {
            var mock = new Mock<PickFilterCompilerLocator>(It.IsAny<IIndex<int, IPickFilterCompiler>>());
            mock.Setup(x => x.Locate(It.Is<EFacetType>(x => x == EFacetType.Discrete)))
                .Returns(new DiscreteFacetPickFilterCompiler());
            mock.Setup(x => x.Locate(It.Is<EFacetType>(x => x == EFacetType.Range)))
                .Returns(new RangeFacetPickFilterCompiler());
            return mock;
        }

        public QuerySetup Scaffold(string uri)
        {
            var facetsConfigScaffolder = new MockFacetsConfigFactory(Registry.Facets);
            var facetsConfig = facetsConfigScaffolder.Create(uri);
            var joinCompiler = new JoinSqlCompiler();
            var pickCompilers = ConcretePickCompilers();
            var facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);
            var facetCodes = facetsConfig.GetFacetCodes().AddIfMissing(facetsConfig.TargetFacet.FacetCode);
            var extraTables = new List<string>();

            QuerySetupBuilder compiler = new QuerySetupBuilder(facetsGraph, pickCompilers.Object, joinCompiler);

            var querySetup = compiler.Build(
                facetsConfig,
                facetsConfig.TargetFacet,
                extraTables,
                facetCodes
            );
            return querySetup;
        }

        //public QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables = null, IEnumerable<FacetTable> aliases = null)
        //{
        //    List<string> facetCodes = facetsConfig.GetFacetCodes().AddIfMissing(facet.FacetCode);
        //    return Build(facetsConfig, facet, extraTables ?? new List<string>(), facetCodes, aliases);
        //}
    }
}
