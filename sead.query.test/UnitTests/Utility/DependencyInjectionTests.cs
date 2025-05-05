using Autofac;
using Autofac.Core.Registration;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using SQT.Scaffolding;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace SQT.Infrastructure
{
    public interface IDependent
    {
        void DoSomeThingNice();
    }

    public class MyDependent : IDependent
    {
        public void DoSomeThingNice()
        {
            Debug.Write("Hello World!");
            Console.Write("HEJ");
        }
    }

    public interface IMyController
    {
        void CallMyDependent();
    }

    public class MyController : IMyController
    {
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
        private IContainer CreateDependencyContainer()
        {   var settingsMock = MockerWithFacetContext.MockSettings();
            var facetContext = new JsonSeededFacetContextFactory().Create("Json");
            var container = DependencyService.CreateContainer(facetContext, settingsMock.Object);
            return container;
        }

        [Fact]
        public void TestResolveService()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IDependent>(new MyDependent());
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
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
            using (var scope = container.BeginLifetimeScope())
            {
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
            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IMyController>();
                service.CallMyDependent();
            }
        }

        [Fact]
        public void CanResolveRegisteredDependencies()
        {
            //using (var context = JsonSeededFacetContextFactory.Create())
            using (var container = CreateDependencyContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                Assert.NotNull(scope.Resolve<ISetting>());
                Assert.NotNull(scope.Resolve<ISeadQueryCache>());
                Assert.NotNull(scope.Resolve<IFacetContext>());
                Assert.NotNull(scope.Resolve<IRepositoryRegistry>());
                Assert.NotNull(scope.Resolve<IPathFinder>());
                Assert.NotNull(scope.Resolve<IQuerySetupBuilder>());
                Assert.NotNull(scope.Resolve<IBogusPickService>());
                Assert.NotNull(scope.Resolve<IResultService>());
                Assert.NotNull(scope.Resolve<ICategoryCountService>());
                Assert.NotNull(scope.Resolve<IFacetContentService>());
                // Assert.NotNull(scope.ResolveKeyed<ICategoryBoundSqlCompiler>(EFacetType.Range));
                Assert.NotNull(scope.ResolveKeyed<IResultSqlCompiler>("tabular"));
                Assert.NotNull(scope.ResolveKeyed<IResultSqlCompiler>("map"));

                Assert.NotNull(scope.Resolve<IResultSqlCompilerLocator>());

                Assert.NotNull(scope.Resolve<ILoadFacetService>());
                Assert.NotNull(scope.Resolve<ILoadResultService>());
            }
        }
        [Fact]
        public void CanResolveResultSqlCompilerLocator()
        {
            //using (var context = JsonSeededFacetContextFactory.Create())
            using (var container = CreateDependencyContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                var locator = scope.Resolve<IResultSqlCompilerLocator>();

                Assert.NotNull(locator);

                Assert.NotNull(locator.Locate("tabular"));
                Assert.NotNull(locator.Locate("map"));

                Assert.Throws<ComponentNotRegisteredException>(() => locator.Locate("flaejl"));
            }
        }

        [Fact]
        public void CannotResolveGeoDependency()
        {
            using (var container = CreateDependencyContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                Assert.Throws<Autofac.Core.Registration.ComponentNotRegisteredException>(() => scope.ResolveKeyed<IFacetContentService>(EFacetType.GeoPolygon));
            }
        }
    }
}
