using Autofac;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.Model;
using Xunit;
using SQT.Mocks;

namespace SQT.LiveServices
{
    public class ResultLoadServiceTests
    {
        public SeadQueryAPI.DependencyService DependencyService { get; private set; }
        public Autofac.IContainer Container { get; private set; }
        public IFacetContext FacetContext { get; private set; }
        public IRepositoryRegistry Registry { get; private set; }

        public ResultLoadServiceTests()
        {
            DependencyService = new SeadQueryAPI.DependencyService() { Options = SettingFactory.GetSettings() };
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterModule(DependencyService);
            Container = builder.Build();
            FacetContext = Container.Resolve<IFacetContext>();
            Registry = Container.Resolve<IRepositoryRegistry>();
        }

        public FacetsConfig2 FakeFacetsConfig(string uri)
            => new MockFacetsConfigFactory(Registry.Facets).Create(uri);

        public virtual ResultConfig FakeResultConfig(string facetCode, string specificationKey, string viewTypeId)
            => ResultConfigFactory.Create(Registry.Facets.GetByCode(facetCode), Registry.Results.GetByKey(specificationKey), viewTypeId);

        
        [Theory]
        // [InlineData("abundance_classification:abundance_classification", "result_facet", "site_level", "tabular")]
        // [InlineData("isotope://sites:sites", "result_facet", "site_level", "tabular")]
        [InlineData("isotope://sites:sites", "result_facet", "map_result", "map")]
        // [InlineData("genus:genus", "result_facet", "site_level", "tabular")]
        // [InlineData("sites:country@5/sites@4,5", "result_facet", "site_level", "map", 10)]
        // [InlineData("sites:data_types@5/rdb_codes@13,21/sites", "result_facet", "site_level", "map", 10)]
        // [InlineData("sites:data_types@5/rdb_codes@13,21/sites", "result_facet", "site_level", "tabular", 10)]
        public void Load_VariousConfigs_Success(string uri, string resultCode, string aggregateCode, string viewType)
        {
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeResultConfig = FakeResultConfig(resultCode, aggregateCode, viewType);
            var service = this.Container.Resolve<ILoadResultService>();
            var data = service.Load(fakeFacetsConfig, fakeResultConfig);
            Assert.NotNull(data);
        }

    }
}
