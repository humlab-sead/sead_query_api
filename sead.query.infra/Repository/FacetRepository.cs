using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SeadQueryInfra {


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

    public class FacetRepository : Repository<Facet, int>, IFacetRepository
    {
        private Dictionary<string, Facet> dictionary = null;

        public FacetRepository(IFacetContext context) : base(context)
        {
        }

        protected override IQueryable<Facet> GetInclude(IQueryable<Facet> set)
        {
            return set.Include(x => x.FacetGroup)
                      .Include(x => x.FacetType)
                      .Include(x => x.Tables)
                      .Include(x => x.Clauses);
        }

        public Dictionary<string, Facet> ToDictionary()
        {
            return dictionary ?? (dictionary = GetAll().ToDictionary(x => x.FacetCode));
        }

        public Facet GetByCode(string facetCode)
        {
            return ToDictionary()?[facetCode];
        }

        public IEnumerable<Facet> FindThoseWithAlias()
        {
            return GetAll().Where(p => p.Tables.Any(c => !c.Alias.Equals("")));
            //var query = GetAll()         // source
            //  .Join(context.Tables,         // target
            //     c => c.CategoryId,          // FK
            //     cm => cm.ChildCategoryId,   // PK
            //     (c, cm) => new { Category = c, CategoryMaps = cm }) // project result
            //  .Select(x => x.Category);  // select result
        }

        public IEnumerable<Facet> GetOfType(EFacetType type)
            => Find(z => z.FacetTypeId == type);

        public (decimal, decimal) GetUpperLowerBounds(Facet facet)
        {
            string sql = new RangeOuterBoundSqlCompiler().Compile(null, facet);
            var item = QueryRow(sql, r => new {
                lower = r.IsDBNull(0) ? 0 : r.GetDecimal(0),
                upper = r.IsDBNull(1) ? 0 : r.GetDecimal(1)
            });
            return item == null ? (0, 0) : (item.lower, item.upper);
        }

        // public string GenerateStateId()
        // {
        //     var sql = $"select nextval('{Context.Settings.CacheSeq}') as cache_id;";
        //     using (var dr = Context.Database.ExecuteSqlQuery(sql).DbDataReader) {
        //         return "state_id_" + dr.GetInt32(0).ToString();
        //     }
        // }
    }

    public static class FacetRepositoryEagerBuilder {
        public static IQueryable<Facet> BuildFacetDefinition(this IQueryable<Facet> query)
        {
            return query.Include(x => x.FacetGroup)
                        .Include(x => x.FacetType)
                        .Include(x => x.Tables)
                        .Include(x => x.Clauses);
        }
    }
}
