using Autofac;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SQT;
using SQT.Infrastructure;
using SQT.Mocks;
using Xunit;

[Collection("UsePostgresFixture")]
public class IntegrationTestBase
{
    // FIXME Create base class
    public DependencyService DependencyService { get; }
    public IContainer Container { get; private set; }
    public IResultSqlCompilerLocator SqlCompilerLocator { get; private set; }
    public IQuerySetupBuilder QuerySetupBuilder { get; private set; }
    public IRepositoryRegistry Registry { get; private set; }

    public IntegrationTestBase(SqliteFacetContext facetConfig = null)
    {
        DependencyService = new DependencyService(facetConfig) { Options = SettingFactory.GetSettings() };
        var builder = new ContainerBuilder();
        builder.RegisterModule(DependencyService);
        Container = builder.Build();
        QuerySetupBuilder = Container.Resolve<IQuerySetupBuilder>();
        SqlCompilerLocator = Container.Resolve<IResultSqlCompilerLocator>();
        Registry = Container.Resolve<IRepositoryRegistry>();
    }
    public FacetsConfig2 FakeFacetsConfig(string uri)
        => new MockFacetsConfigFactory(Registry.Facets).Create(uri);

    public virtual ResultConfig FakeResultConfig(string facetCode, string specificationKey, string viewTypeId)
        => ResultConfigFactory.Create(Registry.Facets.GetByCode(facetCode), Registry.Results.GetByKey(specificationKey), viewTypeId);
}
