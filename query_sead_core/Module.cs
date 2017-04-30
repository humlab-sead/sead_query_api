using Autofac;
using DataAccessPostgreSqlProvider;
using QueryFacetDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryFacetDomain
{
    public class AutofacModule : Module {

        public bool ObeySpeedLimit { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterType<DomainModelDbContext>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.Register(c => new FacetGraphFactory().Build(c.Resolve<IUnitOfWork>()));
            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();

        }

        //var builder = new ContainerBuilder();
        //builder.RegisterType<DomainModelDbContext>().SingleInstance();
        //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
        //var container = builder.Build();
        //using (var scope = container.BeginLifetimeScope()) {
        //    var service = scope.Resolve<IMyController>();
        //    service.CallMyDependent();
        //}
        //builder.RegisterType<FacetGraphFactory>().As<FacetGraphFactory>();
        //builder.Register(c => new ConfigReader("mysection")).As<IConfigReader>();
        //builder.RegisterType<FacetGraphFactory>().As<IFacetsGraph>()
        //    .OnActivating(e => { e.Instance.Dependent = e.Context.Resolve<IDependent>(); });
        //var output = new StringWriter();
        //builder.RegisterInstance(output).As<TextWriter>().ExternallyOwned();
        //builder.RegisterType<ConsoleLogger>();
        //builder.Register(c => new A(c.Resolve<B>()));
        //builder.Register(c => new A() { MyB = c.ResolveOptional<B>() });
    }
}
