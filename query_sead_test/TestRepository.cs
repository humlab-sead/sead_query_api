using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using DataAccessPostgreSqlProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QueryFacetDomain;

namespace query_sead_test
{
    [TestClass]
    public class TestRepository {


        [TestMethod]
        public void CanGetFacetsWithAssociatedObjects()
        {
            using (var context = new DomainModelDbContext()) {
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
        }

        [TestMethod]
        public void CanGetAliasFacets()
        {
            using (var context = new DomainModelDbContext()) {
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
        }
    }
}
