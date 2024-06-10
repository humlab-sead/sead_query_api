using System;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IRouteFinder
    {

        GraphRoute Find(string startTable, string destinationTable);
        List<GraphRoute> Find(string startTable, List<string> destinationTable, bool reduce = true);

    }
}
