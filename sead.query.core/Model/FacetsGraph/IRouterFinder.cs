using System;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IRouteFinder
    {
        IRepositoryRegistry Registry { get; }
        List<TableRelation> Find(string startTable, string destinationTable);
        List<List<TableRelation>> Find(string startTable, List<string> destinationTable, bool reduce = true);

        List<TableRelation> Edges {get; set; }
    }
}
