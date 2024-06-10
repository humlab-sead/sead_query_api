using System;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IFacetsGraph
    {
        public IEnumerable<Table> Tables { get; }
        public IEnumerable<TableRelation> Relations { get; }

        public TableLookup TableLookup { get; }
        public RelationLookup RelationLookup { get; }

        GraphRoute Find(string startTable, string destinationTable);
        List<GraphRoute> Find(string startTable, List<string> destinationTable, bool reduce = true);
        string ToCSV();

        IEnumerable<FacetTable> AliasedFacetTables { get; }
        FacetTable GetAliasedFacetTable(string aliasName);
    }
}
