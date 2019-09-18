using System;
using System.Diagnostics;
using System.Linq;
using Autofac;
using DataAccessPostgreSqlProvider;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using SeadQueryTest;
using SeadQueryTest.Infrastructure;
using Xunit;

namespace SeadQueryTest2.IoC
{

    public interface IDependent {
        void DoSomeThingNice();
    }

    public class MyDependent : IDependent {
        public void DoSomeThingNice()
        {
            Debug.Write("Hello World!");
            Console.Write("HEJ");
        }
    }

    public interface IMyController {
        void CallMyDependent();
    }

    public class MyController : IMyController {

        public IDependent Dependent { get; set; }

        public void CallMyDependent()
        {
            Dependent.DoSomeThingNice();
        }

    }

    public class MyController2 : MyController
    {
        public MyController2(IDependent dependent)
        {
            this.Dependent = dependent;
        }
    }

    public class DependencyInjectionTests
    {
        #region Basic tests
        [Fact]
        public void TestResolveService()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IDependent>(new MyDependent());
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IDependent>();
                service.DoSomeThingNice();
            }
        }

        [Fact]
        public void TestResolveDependent()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IDependent>(new MyDependent()); //.As<IDependent>();
            builder.RegisterType<MyController2>().As<IMyController>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IMyController>();
                service.CallMyDependent();
            }
        }

        [Fact]
        public void TestMemberInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IDependent>(new MyDependent()); //.As<IDependent>();
            builder.RegisterType<MyController>().As<IMyController>()
                .OnActivating(e => { e.Instance.Dependent = e.Context.Resolve<IDependent>(); });

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IMyController>();
                service.CallMyDependent();
            }
        }

        //[Fact]
        //public void DependenciesWithNullOptionCannotBeRegistered()
        //{
        //    var container = new DependencyService().Register(null,null);
        //    using (var scope = container.BeginLifetimeScope()) {
        //        Assert.Throws<ArgumentNullException>(() => scope.ResolveKeyed<IFacetContentService>(EFacetType.Geo));
        //    }
        //}
        #endregion

        [Fact]
        public void CanResolveRegisteredDependencies()
        {
            var container = new TestDependencyService().Register();
            using (var scope = container.BeginLifetimeScope())
            {
                Assert.NotNull(scope.Resolve<IQueryBuilderSetting>());
                Assert.NotNull(scope.Resolve<ISeadQueryCache>());
                Assert.NotNull(scope.Resolve<IFacetContext>());
                Assert.NotNull(scope.Resolve<IRepositoryRegistry>());
                Assert.NotNull(scope.Resolve<IFacetGraphFactory>());
                Assert.NotNull(scope.Resolve<IFacetsGraph>());
                Assert.NotNull(scope.Resolve<IQuerySetupBuilder>());
                Assert.NotNull(scope.Resolve<IDiscreteBogusPickService>());
                Assert.NotNull(scope.Resolve<ICategoryBoundsService>());
                Assert.NotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Discrete));
                Assert.NotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Range));
                Assert.NotNull(scope.ResolveKeyed<ICategoryCountService>(EFacetType.Discrete));
                Assert.NotNull(scope.ResolveKeyed<ICategoryCountService>(EFacetType.Range));
                Assert.NotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Discrete));
                Assert.NotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Range));
                Assert.NotNull(scope.ResolveKeyed<ICategoryBoundSqlQueryCompiler>(EFacetType.Range));
                Assert.NotNull(scope.ResolveKeyed<IResultSqlQueryCompiler>("tabular"));
                Assert.NotNull(scope.ResolveKeyed<IResultSqlQueryCompiler>("map"));
                Assert.NotNull(scope.ResolveKeyed<IResultService>("tabular"));
                Assert.NotNull(scope.ResolveKeyed<IResultService>("map"));

                Assert.NotNull(scope.Resolve<IResultCompiler>());
                Assert.NotNull(scope.Resolve<ILoadFacetService>());
                Assert.NotNull(scope.Resolve<ILoadResultService>());
            }
        }

        [Fact]
        public void CannotResolveGeoDependency()
        {
            var container = new TestDependencyService().Register();
            using (var scope = container.BeginLifetimeScope()) {
                Assert.Throws<Autofac.Core.Registration.ComponentNotRegisteredException>(() => scope.ResolveKeyed<IFacetContentService>(EFacetType.Geo));
            }
        }

        [Fact]
        public void TestResolveUnitOfWork()
        {
            var builder = new ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html
            IQueryBuilderSetting options = new MockOptionBuilder().Build().Value;
            builder.RegisterInstance<IQueryBuilderSetting>(options).SingleInstance().ExternallyOwned();
            builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance();
            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IRepositoryRegistry>();
                Assert.True(service.Facets.GetAll().Count() > 0);
            }
        }
    }
}
