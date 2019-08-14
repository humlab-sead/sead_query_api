using Autofac;
using DataAccessPostgreSqlProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;

namespace SeadQueryTest
{

    [TestClass]
    public class FacetDefinitionTests
    {
        DomainModelDbContext context;
        IContainer container;
        IQueryBuilderSetting settings;

        public FacetDefinitionTests()
        {
            container = new TestDependencyService().Register();
            settings = container.Resolve<IQueryBuilderSetting>();
        }

        [TestInitialize()]
        public void Initialize()
        {
            Debug.WriteLine("Called: TestInitialize");
            context = new DomainModelDbContext(settings);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Debug.WriteLine("Called: TestCleanup");
            context.Dispose();
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
            Assert.IsNotNull(facet.FacetGroup);
            Assert.IsNotNull(facet.TargetTable);
            Assert.IsNotNull(facet.FacetType);
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

        [TestMethod]
        public void CanGetFacetsFromStore()
        {
            var facets = context.FacetDefinitions
                .Include(x => x.FacetGroup)
                .Include(x => x.FacetType)
                .Include(x => x.Tables)
                .Include(x => x.Clauses)
                .ToList();
            Assert.IsTrue(facets.Count > 0);
        }


    }
}
