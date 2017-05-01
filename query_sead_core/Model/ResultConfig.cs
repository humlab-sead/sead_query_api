using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryFacetDomain {

    public enum EResultViewType {
        Unknown = 0,
        Tabular = 1,
        Map = 2
    }

    public class ResultConfig
    {
        public string RequestId { get; set; }                    // ID of current (AJAX) request chain
        public string SessionId { get; set; }                    // Current session ID
        public EResultViewType ViewType { get; set; }            // Kind of result i.e. map, table, etc.
        public string AggregationCode { get; set; }
        public List<string> Items { get; set; } = new List<string>();

        //public string ClientRender { get; set; }
        //public string RequestType { get => ViewType + ClientRender;  }

        public ResultConfig()
        {
        }

        public string generateCacheId(FacetsConfig2 facetsConfig)
        {
            return ViewType.ToString("F") + "_" + facetsConfig.GetPicksCacheId() + String.Join("", Items) + facetsConfig.Language + AggregationCode;
        }

    }
}
