using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore.Model
{

    public class TabularResultContentSet : ResultContentSet
    {
        public TabularResultContentSet(ResultConfig resultConfig, List<ResultAggregateField> resultFields, IDataReader reader) : base()
        {
            Reader = reader;
            Meta = new ResultMetaData() {
                Columns = ResultColumn.Map(resultFields, SourceColumnType.GetColumnTypes(reader))
            };
            Data = new ResultData() { DataCollection = Iterator.ToList() };
            RequestId = resultConfig.RequestId;
        }

        [JsonIgnore] public IEnumerable<object[]> Iterator
        {
            get {
                using (Reader)
                    while (Reader.Read())
                    {
                        var values = new object[Reader.FieldCount];
                        Reader.GetValues(values);
                        yield return values;
                    }
            }
        }
    }
}
