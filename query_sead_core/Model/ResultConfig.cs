using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryFacetDomain {

    public class ResultConfig
    {
        public string RequestId { get; set; }             // ID of current (AJAX) request chain
        public string SessionId { get; set; }             // Current session ID
        public string ViewType { get; set; }              // Kind of result i.e. map, table, etc.
        public string ClientRender { get; set; }
        public string AggregationCode { get; set; }
        public List<string> Items { get; set; } = new List<string>();
        public string RequestType { get => ViewType + ClientRender;  }

        public ResultConfig()
        {
        }

        //public ResultConfig($property_array) {
        //    foreach ($property_array as $key => $value) {
        //        if (property_exists($this, $key)) {
        //            $this->$key = $value;
        //        }
        //    }
        //}

        public string generateCacheId(FacetsConfig2 facetsConfig)
        {
            return ViewType + "_" + facetsConfig.GetPicksCacheId() + String.Join("", Items) + facetsConfig.Language + AggregationCode;
        }

    }
}
