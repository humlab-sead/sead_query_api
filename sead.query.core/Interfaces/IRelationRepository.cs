using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IEdgeRepository : IRepository<TableRelation, int>
    {
        TableRelation FindByName(string sourceName, string targetName);
        IEnumerable<TableRelation> GetEdges(bool bidirectional = true);
        GraphRoute ToRoute(IEnumerable<int> trail);
    }
}
