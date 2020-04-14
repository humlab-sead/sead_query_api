using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryInfra
{

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

        public TableRelation FindByName(string sourceName, string targetName)
        {
            return Context.Set<TableRelation>().BuildEntity()
                .Where(r => (r.SourceName == sourceName && r.TargetName == targetName) ||
                            (r.SourceName == targetName && r.TargetName == sourceName))
                    .FirstOrDefault();
        }

    }

    public static class EdgeRepositoryEagerBuilder {
        public static IQueryable<TableRelation> BuildEntity(this IQueryable<TableRelation> query)
        {
            return query.Include(x => x.SourceTable).Include(x => x.TargetTable);
        }
    }
}
