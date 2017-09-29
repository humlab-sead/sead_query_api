using Newtonsoft.Json;
using QuerySeadDomain.QueryBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace QuerySeadDomain {

    public class ResultViewType
    {
        public string ViewTypeId { get; set; }
        public string ViewName { get; set; }
        public bool IsCachable { get; set; }
    }

    public class ResultConfig
    {
        /// <summary>
        ///  ID of current (AJAX) request chain
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// // Current session's identity
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// Kind of result i.e. map, table, etc.
        /// </summary>
        public string ViewTypeId { get; set; }
        /// <summary>
        /// Aggregate fields, as defined by the keys, to return in result set
        /// </summary>
        public List<string> AggregateKeys { get; set; } = new List<string>();

        //public string ClientRender { get; set; }
        //public string RequestType { get => ViewType + ClientRender;  }

        //[JsonIgnore] public IUnitOfWork Context { get; set; } = null;

        public ResultConfig()
        {
        }

        //public ResultConfig(IUnitOfWork context)
        //{
        //    Context = context;
        //    Debug.Assert(context != null);
        //}

        public string GetCacheId(FacetsConfig2 facetsConfig)
        {
            return  $"{ViewTypeId}_{facetsConfig.GetPicksCacheId()}_{String.Join("", AggregateKeys)}_{facetsConfig.Language}";
        }

        [JsonIgnore] public bool IsEmpty => (AggregateKeys?.Count ?? 0) == 0;

        //private List<ResultAggregate> GetAggregates()
        //{
        //    return Context.Results.GetByKeys(AggregateKeys);
        //}

        //public IEnumerable<ResultAggregateField> GetAggregateFields()
        //{
        //    return GetAggregates().SelectMany(z => z.Fields);
        //}


        //public ResultAggregate GetFirstOrDefaultAggregate()
        //{
        //    var aggregates = GetAggregates();
        //    return aggregates.Count > 0 ? aggregates[0] : null;
        //}
    }

}
