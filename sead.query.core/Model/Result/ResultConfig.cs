using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore.Model
{

    public class ResultConfig
    {
        public string RequestId { get; set; }
        public string SessionId { get; set; }
        public string ViewTypeId { get; set; }
        public List<string> SpecificationKeys { get; set; } = new List<string>();

        public ResultConfig()
        {
        }

        public string GetCacheId(FacetsConfig2 facetsConfig)
        {
            return $"{ViewTypeId}_{facetsConfig.GetPicksCacheId()}_{String.Join("", SpecificationKeys)}_{facetsConfig.DomainCode}";
        }

        [JsonIgnore] public bool IsEmpty => (SpecificationKeys?.Count ?? 0) == 0;

        public string FacetCode { get; set; } = "result_facet";

        public Facet Facet { get; set; }

        public List<ResultSpecification> Specifications = new List<ResultSpecification>();

        public ResultViewType ViewType { get; set; }

        public List<ResultSpecificationField> GetSortedFields()
            => Specifications.SelectMany(x => x.GetSortedFields()).ToList();
    }
}
