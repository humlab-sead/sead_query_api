using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryInfra
{


    public class FacetTypeRepository : Repository<FacetType, int>, IFacetTypeRepository
    {
        public FacetTypeRepository(IFacetContext context) : base(context)
        {
        }
    }

    public class FacetGroupRepository : Repository<FacetGroup, int>, IFacetGroupRepository
    {
        public FacetGroupRepository(IFacetContext context) : base(context)
        {
        }
    }
    public class FacetTableRepository : Repository<FacetTable, int>, IFacetTableRepository
    {
        public FacetTableRepository(IFacetContext context) : base(context)
        {
        }

        public IEnumerable<FacetTable> FindThoseWithAlias()
        {
            return GetAll().Where(p => p.HasAlias);
        }
        protected override IQueryable<FacetTable> GetInclude(IQueryable<FacetTable> set)
        {
            return set.Include(x => x.Table);
        }

    }

    public class FacetRepository : Repository<Facet, int>, IFacetRepository
    {
        public static int DOMAIN_FACET_GROUP_ID = 999;

        private Dictionary<string, Facet> hash = null;

        public FacetRepository(IFacetContext context) : base(context)
        {
        }

        protected override IQueryable<Facet> GetInclude(IQueryable<Facet> set)
        {
            return set.Include(x => x.FacetGroup)
                      .Include(x => x.FacetType)
                      .Include("Tables.Table")
                      .Include(x => x.Clauses);
        }

        public Dictionary<string, Facet> ToDictionary()
        {
            return hash ?? (hash = GetAll().ToDictionary(x => x.FacetCode));
        }

        public Facet GetByCode(string facetCode)
        {
            return ToDictionary()?[facetCode];
        }

        public IEnumerable<Facet> FindThoseWithAlias()
        {
            return GetAll().Where(p => p.Tables.Any(c => !c.Alias.IsEmpty()));
        }

        public IEnumerable<Facet> Parents()
        {
            // FIXME: Get all with children instead of magic group id
            return GetAll().Where(p => p.FacetGroupId == DOMAIN_FACET_GROUP_ID);
        }

        public IEnumerable<Facet> Children(string facetCode)
        {
            if (facetCode.IsEmpty() || facetCode.ToLower().Equals("general")) {
                return GetAllUserFacets();
            }
            var children = GetSet()
                .Include("Children.Child")
                .Include("Children.Child.FacetGroup")
                .Where(f => f.FacetCode == facetCode)
                .SelectMany(z => z.Children)
                .OrderBy(z => z.Position)
                .Select(z => z.Child);
            return children.ToList();
        }

        public IEnumerable<Facet> GetOfType(EFacetType type)
            => Find(z => z.FacetTypeId == type);

        public IEnumerable<Facet> GetAllUserFacets()
            => GetAll().Where(z => z.FacetGroupId != 0 && z.FacetGroupId != DOMAIN_FACET_GROUP_ID && z.IsApplicable == true).ToList();

    }

    public static class FacetRepositoryEagerBuilder {
        public static IQueryable<Facet> BuildFacetDefinition(this IQueryable<Facet> query)
        {
            return query.Include(x => x.FacetGroup)
                        .Include(x => x.FacetType)
                        .Include("Tables.Table")
                        .Include(x => x.Clauses);
        }
    }

    public static class FacetTableEagerBuilder
    {
        public static IQueryable<FacetTable> BuildEntity(this IQueryable<FacetTable> query)
        {
            return query.Include(x => x.Table);
        }
    }
}
