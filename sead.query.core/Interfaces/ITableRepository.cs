using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface INodeRepository : IRepository<Table, int>
    {
        public IEnumerable<Table> GetAliasTables();
        public IEnumerable<Table> GetNodes();
        public Table GetNode(int id);
        public Table GetNode(string id);
    }
}
