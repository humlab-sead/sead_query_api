using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SeadQueryCore;
using SeadQueryCore.Model;

namespace SeadQueryAPI.Serializers
{
    public class ResultConfigReconstituteService : ReconstituteService, IResultConfigReconstituteService
    {
        public static Dictionary<string, string> DefaultViewTypeResultFacet = new Dictionary<string, string>
        {
            { "map", "map_result" },
            { "tabular", "result_facet" }
        };

        public static Dictionary<string, List<string>> FixedViewTypeAggregateKeys = new Dictionary<string, List<string>>
        {
            { "map", new List<string>() { "map_result" } },
            { "tabular", new List<string>() { "site_level" } }
        };

        public ResultConfigReconstituteService(IRepositoryRegistry registry) : base(registry)
        {

        }

        public ResultConfig Reconstitute(ResultConfig resultConfig)
        {
            if (resultConfig.ViewTypeId.IsEmpty() || !DefaultViewTypeResultFacet.ContainsKey(resultConfig.ViewTypeId))
                throw new ArgumentNullException("Unknown or empty viewType cannot be reconstituted!");

            if (resultConfig.FacetCode.IsEmpty()) {
                resultConfig.FacetCode = DefaultViewTypeResultFacet[resultConfig.ViewTypeId];
            }

            if (FixedViewTypeAggregateKeys.ContainsKey(resultConfig.ViewTypeId)) {
                resultConfig.AggregateKeys = FixedViewTypeAggregateKeys[resultConfig.ViewTypeId];
            }

            resultConfig.Facet = GetFacetByCode(resultConfig.FacetCode);

            resultConfig.ResultComposites = Registry.Results.GetByKeys(resultConfig.AggregateKeys);

            return resultConfig;
        }

        public ResultConfig Reconstitute(JObject resultConfigJson)
        {
            var resultConfig = Reconstitute(resultConfigJson.ToObject<ResultConfig>());
            return resultConfig;
        }
    }
}
