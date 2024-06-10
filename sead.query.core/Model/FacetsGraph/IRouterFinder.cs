using System;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IRouteFinder
    {
        IRepositoryRegistry Registry { get; }
        GraphRoute Find(string startTable, string destinationTable);
        List<GraphRoute> Find(string startTable, List<string> destinationTable, bool reduce = true);

    }
}
