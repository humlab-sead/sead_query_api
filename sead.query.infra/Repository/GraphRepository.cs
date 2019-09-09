using DataAccessPostgreSqlProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SeadQueryCore;

namespace SeadQueryInfra {

    public class NodeRepository : Repository<GraphNode, int>, INodeRepository {
        public NodeRepository(IFacetContext context) : base(context)
        {
        }
    }

    public class EdgeRepository : Repository<GraphTableRelation, int>, IEdgeRepository
    {
        public EdgeRepository(IFacetContext context) : base(context)
        {
        }

        public override IEnumerable<GraphTableRelation> GetAll()
        {
            return Context.Set<GraphTableRelation>().BuildEntity().ToList();
        }
    }

    public static class EdgeRepositoryEagerBuilder {
        public static IQueryable<GraphTableRelation> BuildEntity(this IQueryable<GraphTableRelation> query)
        {
            return query.Include(x => x.SourceTable).Include(x => x.TargetTable);
        }
    }
}
