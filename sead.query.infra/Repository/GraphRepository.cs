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

    public class EdgeRepository : Repository<GraphEdge, int>, IEdgeRepository
    {
        public EdgeRepository(IFacetContext context) : base(context)
        {
        }

        public override IEnumerable<GraphEdge> GetAll()
        {
            return Context.Set<GraphEdge>().BuildEntity().ToList();
        }
    }

    public static class EdgeRepositoryEagerBuilder {
        public static IQueryable<GraphEdge> BuildEntity(this IQueryable<GraphEdge> query)
        {
            return query.Include(x => x.SourceTable).Include(x => x.TargetTable);
        }
    }
}
