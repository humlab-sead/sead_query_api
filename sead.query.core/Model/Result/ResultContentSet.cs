using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore.Model
{

    public class ResultContentSet // : IDisposable
    {
        public class ResultColumn
        {
            public string FieldKey { get; set; }
            public string DisplayText { get; set; }
            public string LinkLabel { get; set; }
            public string LinkUrl { get; set; }
            public string Type { get; set; }

            public static List<ResultColumn> Map(List<ResultAggregateField> resultFields, List<SourceColumnType> dataColumns)
            {
                return resultFields.Select((z, i) => new ResultColumn()
                {
                    FieldKey = z.ResultField.ResultFieldKey,
                    DisplayText = z.ResultField.DisplayText,
                    LinkLabel = z.ResultField.LinkLabel,
                    LinkUrl = z.ResultField.LinkUrl,
                    Type = dataColumns[i].NetType
                }).ToList();
            }
        }

        public class ResultMetaData
        {
            public List<ResultColumn> Columns { get; set; } = null;
        }

        public class ResultData
        {
            //public IEnumerable<object[]> DataCollection { get; set; } = null;
            public List<object[]> DataCollection { get; set; } = null;
        }

        [JsonIgnore] protected IDataReader Reader { get; set; } = null;

        public string RequestId { get; set; } = "0";
        public ResultMetaData Meta { get; set; }
        public ResultData Data { get; set; }
        public dynamic Payload { get; set; } = null;
        public string Query { get; set; } = "Hej Hopp";
    }
}
