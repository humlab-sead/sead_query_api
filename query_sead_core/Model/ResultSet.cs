using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuerySeadDomain.Model
{
    public interface IResultData
    {
    }

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

    }

    public class TabularResultContentSet : ResultContentSet
    {
        public TabularResultContentSet(ResultConfig resultConfig, List<ResultAggregateField> resultFields, IDataReader reader) : base()
        {
            Reader = reader;
            Meta = new ResultMetaData() { Columns = ResultColumn.Map(resultFields, SourceColumnType.GetColumnTypes(reader)) };
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
                        int i = Reader.GetValues(values);
                        yield return values;
                    }
            }
        }
    }

    //public class MapResultContentSet : TabularResultContentSet
    //{
    //    public MapResultContentSet(ResultConfig resultConfig, List<ResultAggregateField> resultFields, IDataReader reader) : base(resultConfig, resultFields, reader)
    //    {
    //    }
    //}

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
