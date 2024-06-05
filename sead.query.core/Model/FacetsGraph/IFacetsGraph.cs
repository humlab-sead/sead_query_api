using System;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IFacetsGraph
    {
        public NodesContainer<Table> NodeContainer { get; }
        public EdgesContainer<TableRelation> EdgeContaniner { get; }

        GraphRoute Find(string startTable, string destinationTable);
        List<GraphRoute> Find(string startTable, List<string> destinationTable, bool reduce = true);
        string ToCSV();

        IEnumerable<FacetTable> AliasedFacetTables { get; }
        FacetTable GetAliasedFacetTable(string aliasName);
    }
}
