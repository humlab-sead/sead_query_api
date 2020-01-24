using SeadQueryInfra.DataAccessProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SeadQueryCore;

namespace SeadQueryInfra {

    public class TableRepository : Repository<Table, int>, ITableRepository {
        public TableRepository(IFacetContext context) : base(context)
        {
        }
    }

    public class TableRelationRepository : Repository<TableRelation, int>, ITableRelationRepository
    {
        public TableRelationRepository(IFacetContext context) : base(context)
        {
        }

        public override IEnumerable<TableRelation> GetAll()
        {
            return Context.Set<TableRelation>().BuildEntity().ToList();
        }
    }

    public static class EdgeRepositoryEagerBuilder {
        public static IQueryable<TableRelation> BuildEntity(this IQueryable<TableRelation> query)
        {
            return query.Include(x => x.SourceTable).Include(x => x.TargetTable);
        }
    }
}
