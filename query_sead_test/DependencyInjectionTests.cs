using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using DataAccessPostgreSqlProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Autofac;
using QuerySeadDomain;
using QuerySeadAPI;
using CacheManager.Core;
using QuerySeadDomain.QueryBuilder;
using QuerySeadAPI.Services;

namespace QuerySeadTests.IoC {

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

    [TestClass]
    public class DependencyInjectionTests
    {
        #region Basic tests
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null.")]
        public void DependenciesWithNullOptionCannotBeRegistered()
        {
            var container = new DependencyService().Register(null,null);
            Assert.IsNotNull(container);
        }
        #endregion

        [TestMethod]
        public void CanResolveRegisteredDependencies()
        {
            var container = new TestDependencyService().Register();
            using (var scope = container.BeginLifetimeScope())
            {
                Assert.IsNotNull(scope.Resolve<IQueryBuilderSetting>());
                Assert.IsNotNull(scope.Resolve<ICache>());
                Assert.IsNotNull(scope.Resolve<DomainModelDbContext>());
                Assert.IsNotNull(scope.Resolve<IUnitOfWork>());
                Assert.IsNotNull(scope.Resolve<IFacetGraphFactory>());
                Assert.IsNotNull(scope.Resolve<IFacetsGraph>());
                Assert.IsNotNull(scope.Resolve<IQuerySetupBuilder>());
                Assert.IsNotNull(scope.Resolve<IDeleteBogusPickService>());
                Assert.IsNotNull(scope.Resolve<ICategoryBoundsService>());
                Assert.IsNotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Discrete));
                Assert.IsNotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Range));
                Assert.IsNotNull(scope.ResolveKeyed<ICategoryCountService>(EFacetType.Discrete));
                Assert.IsNotNull(scope.ResolveKeyed<ICategoryCountService>(EFacetType.Range));
                Assert.IsNotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Discrete));
                Assert.IsNotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Range));
                Assert.IsNotNull(scope.ResolveKeyed<ICategoryBoundSqlQueryBuilder>(EFacetType.Range));
                Assert.IsNotNull(scope.ResolveKeyed<IResultSqlQueryCompiler>("tabular"));
                Assert.IsNotNull(scope.ResolveKeyed<IResultSqlQueryCompiler>("map"));
                Assert.IsNotNull(scope.ResolveKeyed<IResultService>("tabular"));
                Assert.IsNotNull(scope.ResolveKeyed<IResultService>("map"));

                Assert.IsNotNull(scope.Resolve<IResultQueryCompiler>());
                Assert.IsNotNull(scope.Resolve<ILoadFacetService>());
                Assert.IsNotNull(scope.Resolve<ILoadResultService>());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Autofac.Core.Registration.ComponentNotRegisteredException))]
        public void CannotResolveGeoDependency()
        {
            var container = new TestDependencyService().Register();
            using (var scope = container.BeginLifetimeScope()) {
                Assert.IsNotNull(scope.ResolveKeyed<IFacetContentService>(EFacetType.Geo));
            }
        }

    }
}
