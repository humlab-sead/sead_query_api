using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SeadQueryCore {
    public interface IFacetRepository : IRepository<FacetDefinition, int> {
    }

    public class FacetTypeRepository : Repository<FacetType, int>
    {
        public FacetTypeRepository(DomainModelDbContext context) : base(context)
        {
        }
    }

    public class FacetGroupRepository : Repository<FacetGroup, int>
    {
        public FacetGroupRepository(DomainModelDbContext context) : base(context)
        {
        }
    }

    public class FacetRepository : Repository<FacetDefinition, int>, IFacetRepository
    {
        private Dictionary<string, FacetDefinition> dictionary = null;

        public FacetRepository(DomainModelDbContext context) : base(context)
        {
        }

        protected override IQueryable<FacetDefinition> GetInclude(IQueryable<FacetDefinition> set)
        {
            return set.Include(x => x.FacetGroup)
                      .Include(x => x.FacetType)
                      .Include(x => x.Tables)
                      .Include(x => x.Clauses);
        }

        public Dictionary<string, FacetDefinition> ToDictionary()
        {
            return dictionary ?? (dictionary = GetAll().ToDictionary(x => x.FacetCode));
        }

        public FacetDefinition GetByCode(string facetCode)
        {
            return ToDictionary()?[facetCode];
        }

        public IEnumerable<FacetDefinition> FindThoseWithAlias()
        {
            return GetAll().Where(p => p.Tables.Any(c => ! c.Alias.Equals("")));
            //var query = GetAll()         // source
            //  .Join(context.Tables,         // target
            //     c => c.CategoryId,          // FK
            //     cm => cm.ChildCategoryId,   // PK
            //     (c, cm) => new { Category = c, CategoryMaps = cm }) // project result
            //  .Select(x => x.Category);  // select result
        }

        public IEnumerable<FacetDefinition> GetOfType(EFacetType type)
            => Find(z => z.FacetTypeId == type);

        public (decimal, decimal) GetUpperLowerBounds(FacetDefinition facet)
        {
            string sql = RangeLowerUpperSqlQueryBuilder.compile(null, facet);
            var item = QueryRow(sql, r => new {
                lower = r.IsDBNull(0) ? 0 : r.GetDecimal(0),
                upper = r.IsDBNull(1) ? 0 : r.GetDecimal(1)
            });
            return item == null ? (0, 0) : (item.lower, item.upper);
        }

        public string GenerateStateId()
        {
            var sql = $"select nextval('{Context.Settings.CacheSeq}') as cache_id;";
            using (var dr = Context.Database.ExecuteSqlQuery(sql).DbDataReader) {
                return "state_id_" + dr.GetInt32(0).ToString();
            }
        }
    }

    public static class FacetRepositoryEagerBuilder {
        public static IQueryable<FacetDefinition> BuildFacetDefinition(this IQueryable<FacetDefinition> query)
        {
            return query.Include(x => x.FacetGroup)
                        .Include(x => x.FacetType)
                        .Include(x => x.Tables)
                        .Include(x => x.Clauses);
        }
    }
}
