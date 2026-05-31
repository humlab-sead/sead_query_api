using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class FacetTypeRepository(RepositoryRegistry registry) : Repository<FacetType, int>(registry), IFacetTypeRepository { }

    public class FacetGroupRepository(RepositoryRegistry registry) : Repository<FacetGroup, int>(registry), IFacetGroupRepository { }

    public class FacetTableRepository(RepositoryRegistry registry) : Repository<FacetTable, int>(registry), IFacetTableRepository
    {
        private List<FacetTable> _aliasTables = null;

        public List<FacetTable> FindThoseWithAlias()
        {
            return _aliasTables ??= GetAll().Where(p => p.HasAlias).ToList();
        }

        protected override IQueryable<FacetTable> GetInclude(IQueryable<FacetTable> set)
        {
            return set.Include(x => x.Table);
        }

        public FacetTable GetByAlias(string aliasName) => FindThoseWithAlias().Where(x => x.Alias == aliasName).FirstOrDefault();
    }

    public class FacetAnchorRepository(RepositoryRegistry registry) : Repository<FacetAnchor, int>(registry), IFacetAnchorRepository { }

    public class FacetRepository(RepositoryRegistry registry) : Repository<Facet, int>(registry), IFacetRepository
    {
        public static int DOMAIN_FACET_GROUP_ID = 999;

        private Dictionary<string, Facet> hash = null;

        protected override IQueryable<Facet> GetInclude(IQueryable<Facet> set)
        {
            return set.ConfigureFacetQuery();
        }

        public Dictionary<string, Facet> ToDictionary()
        {
            return hash ??= GetAll().ToDictionary(x => x.FacetCode);
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
            return GetAll().Where(p => p.FacetGroupId == DOMAIN_FACET_GROUP_ID);
        }

        public IEnumerable<Facet> Children(string facetCode)
        {
            if (facetCode.IsEmpty() || facetCode.ToLower().Equals("general"))
            {
                return GetAllUserFacets();
            }
            var children = GetSet()
                .Include(f => f.Children)
                .ThenInclude(fc => fc.Child)
                .ThenInclude(c => c.FacetGroup)
                .Where(f => f.FacetCode == facetCode)
                .SelectMany(z => z.Children)
                .OrderBy(z => z.Position)
                .Select(z => z.Child);
            return children.ToList();
        }

        public IEnumerable<Facet> GetOfType(EFacetType type) => Find(z => z.FacetTypeId == type);

        public IEnumerable<Facet> GetAllUserFacets() =>
            GetAll().Where(z => z.FacetGroupId != 0 && z.FacetGroupId != DOMAIN_FACET_GROUP_ID && z.IsApplicable).ToList();
    }

    public static class FacetRepositoryEagerBuilder
    {
        public static IQueryable<Facet> ConfigureFacetQuery(this IQueryable<Facet> query)
        {
            return query
                .Include(x => x.FacetGroup)
                .Include(x => x.FacetType)
                .Include(x => x.Tables)
                .ThenInclude(ft => ft.Table)
                .Include(x => x.Clauses)
                .Include(x => x.FacetAnchors)
                .ThenInclude(fa => fa.Anchor)
                .Include(x => x.FacetAnchors)
                .ThenInclude(fa => fa.Route)
                .ThenInclude(r => r.Steps)
                .ThenInclude(step => step.Table);
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
