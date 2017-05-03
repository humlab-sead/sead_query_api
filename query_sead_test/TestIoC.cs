using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using DataAccessPostgreSqlProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Autofac;
using QuerySeadDomain;

namespace QueryFacetTest {

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

        //public MyController(IDependent dependent)
        //{
        //    this.Dependent = dependent;
        //}

        public void CallMyDependent()
        {
            Dependent.DoSomeThingNice();
        }

    }

    [TestClass]
    public class TestIoC
    {

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
        public void TestSResolveDependent()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IDependent>(new MyDependent()); //.As<IDependent>();
            builder.RegisterType<MyController>().As<IMyController>();
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

    }
}
