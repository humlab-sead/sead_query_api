using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using DataAccessPostgreSqlProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Autofac;
using QueryFacetDomain;
using QueryFacetDomain.QueryBuilder;

namespace QueryFacetTest {
    [TestClass]
    public class TestFacetsGraph
    {

        //[TestMethod]
        //public void TestCreateFacetGraph()
        //{
        //    var builder = new ContainerBuilder();

        //    // http://docs.autofac.org/en/latest/register/registration.html

        //    builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

        //    builder.RegisterType<FacetGraphFactory>().As<FacetGraphFactory>();

        //    builder.Register(c => new ConfigReader("mysection")).As<IConfigReader>();

        //    builder.RegisterType<FacetGraphFactory>().As<IFacetsGraph>()
        //        .OnActivating(e => { e.Instance.Dependent = e.Context.Resolve<IDependent>(); });

        //    var output = new StringWriter();
        //    builder.RegisterInstance(output).As<TextWriter>().ExternallyOwned();
        //    builder.RegisterType<ConsoleLogger>();
        //    builder.Register(c => new A(c.Resolve<B>()));

        //    builder.Register(c => new A() { MyB = c.ResolveOptional<B>() });

        //    var container = builder.Build();
        //    using (var scope = container.BeginLifetimeScope()) {
        //        var service = scope.Resolve<IMyController>();
        //        service.CallMyDependent();
        //    }
        //}

        [TestMethod]
        public void TestResolveUnitOfWork()
        {
            var builder = new ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterType<DomainModelDbContext>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IUnitOfWork>();
                Assert.IsTrue(service.Facets.GetAll().Count() > 0);
            }
        }

        [TestMethod]
        public void TestResolveFacetsGraph()
        {
            var container = RegisterDependencies.Register();
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IFacetsGraph>();
                Assert.IsTrue(service.Nodes.Count > 0);
            }
        }

    }
}
