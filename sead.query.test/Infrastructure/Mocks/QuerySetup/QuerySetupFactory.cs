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

namespace SeadQueryTest.Fixtures
{
    public class QuerySetupFactory
    {

        public IRepositoryRegistry Registry { get; set; }

        public QuerySetupFactory(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        private Facet GetFacet(string facetCode) => !string.IsNullOrEmpty(facetCode) ? Registry.Facets.GetByCode(facetCode) : null;
        // FacetInstances.Store[facetCode]

        private IIndex<int, IPickFilterCompiler> ConcretePickCompilers()
        {
            return new MockIndex<int, IPickFilterCompiler>
            {
                { 1, new DiscreteFacetPickFilterCompiler() },
                { 2, new RangeFacetPickFilterCompiler() }
            };
        }

        public QuerySetup Scaffold(string uri)
        {
            var facetsConfigScaffolder = new MockFacetsConfigFactory(Registry);
            var facetsConfig = facetsConfigScaffolder.Create(uri);
            var edgeCompiler = new EdgeSqlCompiler();
            var pickCompilers = ConcretePickCompilers();
            var facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);

            QuerySetupCompiler compiler = new QuerySetupCompiler(facetsGraph, pickCompilers, edgeCompiler);

            var querySetup = compiler.Build(
                facetsConfig,
                facetsConfig.TargetFacet //,
                // new List<string>(),
                // new List<string>() { facetsConfig.TargetFacet.FacetCode }
            );
            return querySetup;
        }
    }
}
