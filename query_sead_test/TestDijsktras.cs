using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using DataAccessPostgreSqlProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QueryFacetTest {
    [TestClass]
    public class TestDijsktras
    {

        [TestMethod]
        public void TestDijkstras()
        {
            var g = new QueryFacetDomain.DijkstrasGraph<char>();
            g.add_vertex('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } });
            g.add_vertex('B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } });
            g.add_vertex('C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } });
            g.add_vertex('D', new Dictionary<char, int>() { { 'F', 8 } });
            g.add_vertex('E', new Dictionary<char, int>() { { 'H', 1 } });
            g.add_vertex('F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } });
            g.add_vertex('G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 9 } });
            g.add_vertex('H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } });
            List<char> route = g.shortest_path('A', 'H');
            Assert.IsTrue(route.Count > 0);
            route.ForEach(x => Debug.Write(x));
        }

        [TestMethod]
        public void CanGetFacetsFromStore()
        {
            using (var context = new DomainModelDbContext()) {
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
}
