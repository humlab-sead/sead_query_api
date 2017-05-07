using DataAccessPostgreSqlProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace QuerySeadDomain {

    public class NodeRepository : Repository<GraphNode> {

        public NodeRepository(DomainModelDbContext context) : base(context)
        {
        }
    }

    public class EdgeRepository : Repository<GraphEdge> {

        public EdgeRepository(DomainModelDbContext context) : base(context)
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
