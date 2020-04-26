using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore.Model
{
    public class SourceColumnType
    {
        public string Name { get; set; }
        public string NetType { get; set; }
        public string SqlType { get; set; }

        public static List<SourceColumnType> GetColumnTypes(IDataReader reader)
        {
            return Enumerable.Range(0, reader.FieldCount)
                .Select(i => new SourceColumnType()
                {
                    Name = reader.GetName(i),
                    NetType = reader.GetFieldType(i).Name,
                    SqlType = reader.GetDataTypeName(i)
                }).ToList();
        }
    }
}
