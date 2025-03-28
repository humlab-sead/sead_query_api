using System.Collections.Generic;

namespace SeadQueryCore
{
    using Route = List<TableRelation>;

    public interface IEdgeRepository : IRepository<TableRelation, int>
    {
        // TableRelation FindByName(string sourceName, string targetName);
        Route GetEdges(bool bidirectional = true);
        // Route ToRoute(IEnumerable<int> trail);
    }
}
