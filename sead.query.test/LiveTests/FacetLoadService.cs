using Autofac;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.Model;
using Xunit;
using SQT.Mocks;

namespace SQT.LiveServices
{
    public class FacetLoadServiceTests
    {
        public SeadQueryAPI.DependencyService DependencyService { get; private set; }
        public Autofac.IContainer Container { get; private set; }
        public IFacetContext FacetContext { get; private set; }
        public IRepositoryRegistry Registry { get; private set; }

        public FacetLoadServiceTests()
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
        // [InlineData("genus:genus")]
        // [InlineData("abundance_classification:abundance_classification")]
        [InlineData("sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738")]
        // [InlineData("sites_polygon:sites_polygon")]
        public void Load_VariousConfigs_Success(string uri)
        {
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var service = this.Container.Resolve<ILoadFacetService>();

            var data = service.Load(fakeFacetsConfig);

            Assert.NotNull(data);
        }
    }
}
