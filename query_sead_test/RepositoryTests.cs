using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataAccessPostgreSqlProvider;
using System.Linq;
using QuerySeadDomain;
using System.Diagnostics;
using Autofac;

namespace QuerySeadTests.Repository {

    [TestClass]
    public class RepositoryTests {

        DomainModelDbContext context;
        readonly IContainer container;
        readonly IQueryBuilderSetting settings;

        public RepositoryTests()
        {
            container = new TestDependencyService().Register();
            settings = container.Resolve<IQueryBuilderSetting>();
        }

        [TestInitialize()]
        public void Initialize()
        {
            context = new DomainModelDbContext(settings);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void TestResolveUnitOfWork()
        {
            var builder = new ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterInstance<IQueryBuilderSetting>(Startup.Options).SingleInstance().ExternallyOwned();
            builder.RegisterType<DomainModelDbContext>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUnitOfWork>();
                Assert.IsTrue(service.Facets.GetAll().Count() > 0);
            }
        }

        [TestMethod]
        public void CanGetFacetObject()
        {
            var repository = new FacetRepository(context);

            FacetDefinition facet = repository.Get(14);

            Assert.AreEqual(facet.FacetCode, "places");
            Assert.AreEqual(facet.DisplayTitle, "Places");
            Assert.AreEqual(facet.CategoryIdExpr, "tbl_locations.location_id");
            Assert.AreEqual(facet.CategoryNameExpr, "tbl_locations.location_name");
            Assert.AreEqual(facet.AggregateType, "count");
        }

        [TestMethod]
        public void CanGetFacetsWithAssociatedObjects()
        {
            var repository = new FacetRepository(context);

            List<FacetDefinition> facets = repository.GetAll().ToList();

            Assert.AreEqual(facets.Count, 37);

            FacetDefinition facet = repository.Get(14);

            Assert.AreEqual(facet.FacetCode, "places");
            Assert.AreEqual(facet.DisplayTitle, "Places");
            Assert.IsNotNull(facet.FacetGroup, "FacetGroup");
            Assert.IsNotNull(facet.TargetTable, "TargetTable");
            Assert.IsNotNull(facet.FacetType, "FacetType");
            Assert.IsNotNull(facet.Tables);
            Assert.IsTrue(facet.Tables.Count > 0);
            Assert.AreEqual(facet.CategoryIdExpr, "tbl_locations.location_id");
            Assert.AreEqual(facet.CategoryNameExpr, "tbl_locations.location_name");
            Assert.AreEqual(facet.AggregateType, "count");
        }

        [TestMethod]
        public void CanGetAliasFacets()
        {
            var repository = new FacetRepository(context);
            List<FacetDefinition> facets = repository.GetAll().ToList();
            FacetDefinition facet = repository.Get(21);
            Assert.AreEqual("country", facet.FacetCode);
            Assert.AreEqual("Country", facet.DisplayTitle);
            Assert.IsNotNull(facet.FacetGroup);
            Assert.IsNotNull(facet.TargetTable);
            Assert.IsNotNull(facet.FacetType);
            Assert.IsNotNull(facet.Tables);
            Assert.IsTrue(facet.Tables.Count > 0);

            List<FacetDefinition> aliasFacets = repository.FindThoseWithAlias().ToList();
            Assert.AreEqual(1, aliasFacets.Count);
            Assert.AreSame(facet, aliasFacets[0]);
        }

        //[TestMethod]
        //public void CanGetAliasFacets()
        //{
        //    var repository = new FacetRepository(context);
        //    List<FacetDefinition> facets = repository.GetAll().ToList();
        //    FacetDefinition facet = repository.Get(21);
        //    Assert.AreEqual("country", facet.FacetCode);
        //    Assert.AreEqual("Country", facet.DisplayTitle);
        //    Assert.IsNotNull(facet.FacetGroup);
        //    Assert.IsNotNull(facet.TargetTable);
        //    Assert.IsNotNull(facet.FacetType);
        //    Assert.IsNotNull(facet.Tables);
        //    Assert.IsTrue(facet.Tables.Count > 0);

        //    List<FacetDefinition> aliasFacets = repository.FindThoseWithAlias().ToList();
        //    Assert.AreEqual(1, aliasFacets.Count);
        //    Assert.AreSame(facet, aliasFacets[0]);
        //}


    }
}
