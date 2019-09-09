using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SeadQueryTest.MockRepository
{
    public class FakeFacetRepository : FakeRepository<Facet, int>, IFacetRepository, IEquatable<FakeFacetRepository>
    {
        public FakeFacetRepository(System.Linq.IQueryable<Facet> entities) : base(entities)
        {
        }

        public Facet GetByCode(string facetCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Facet> FindThoseWithAlias()
        {
            return GetAll().Where(p => p.Tables.Any(c => !c.Alias.Equals("")));
        }

        public IEnumerable<Facet> GetOfType(EFacetType type)
            => Find(z => z.FacetTypeId == type);

        public (decimal, decimal) GetUpperLowerBounds(Facet facet)
        {
            throw new NotImplementedException();
        }

        public bool Equals(FakeFacetRepository other)
        {
            return true;
        }

        public Dictionary<string, Facet> ToDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
