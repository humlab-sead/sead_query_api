using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface ITableRelationRepository : IRepository<TableRelation, int>
    {
        TableRelation FindByName(string sourceName, string targetName);
    }
}