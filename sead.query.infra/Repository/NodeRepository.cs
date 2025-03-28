using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class NodeRepository(IRepositoryRegistry registry) : Repository<Table, int>(registry), INodeRepository
    {
        public class NodesLookup(IEnumerable<Table> nodes)
        {
            public IEnumerable<Table> Nodes { get; private set; } = nodes;
            public Dictionary<string, Table> Name2Node { get; private set; } = nodes.ToDictionary(x => x.Name);
            public Dictionary<int, Table> Id2Node { get; private set; } = nodes.ToDictionary(x => x.Id);

            public Table this[int index] { get { return Id2Node[index]; } }
            public Table this[string name] { get { return Name2Node[name]; } }
        }

        private NodesLookup __Lookup = null;

        private NodesLookup GetLookup()
        {
            if (__Lookup == null)
            {
                __Lookup = new NodesLookup(GetAll().Concat(Registry.Tables.GetAliasTables()).Distinct());
            }
            return __Lookup;
        }
        public IEnumerable<Table> GetAliasTables()
        {
            // ...project all FacetTable items that has an alias to a new Table item
            var aliasNodes = Registry.FacetTables.FindThoseWithAlias()
                .Select((x, id) => new Table
                {
                    TableId = 10000 + id,
                    TableOrUdfName = x.Alias
                });
            return aliasNodes;
        }

        public IEnumerable<Table> GetNodes()
        {
            return GetLookup().Nodes;
        }

        public Table this[int index] { get { return GetLookup()[index]; } }
        public Table this[string name] { get { return GetLookup()[name]; } }

        public Table GetNode(int id)
        {
            return GetLookup()[id];
        }
        public Table GetNode(string id)
        {
            return GetLookup()[id];
        }
    }

}
