using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;
using SeadQueryInfra;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure.Scaffolds;
using SeadQueryTest.Infrastructure.Scaffolding;
using Autofac.Features.Indexed;
using SeadQueryTest.Fixtures;
using SeadQueryTest;
using Xunit;

namespace SeadQueryTest.Fixtures
{
    public class ScaffoldQuerySetup
    {

        public IRepositoryRegistry Registry { get; set; }

        public ScaffoldQuerySetup(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        private Facet GetFacet(string facetCode) => facetCode != "" ? Registry.Facets.GetByCode(facetCode) : null;
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
            var facetsConfigScaffolder = new ScaffoldFacetsConfig(Registry);
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
