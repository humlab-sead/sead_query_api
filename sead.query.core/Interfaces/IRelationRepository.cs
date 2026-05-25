using System.Collections.Generic;

namespace SeadQueryCore
{

    public interface IEdgeRepository : IRepository<TableRelation, int>
    {
        // TableRelation FindByName(string sourceName, string targetName);
        List<TableRelation> GetEdges(bool bidirectional = true);
    }
}
