using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SeadQueryAPI.DTO;
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

        public static Dictionary<string, List<string>> FixedViewTypeSpecificationKeys = new Dictionary<string, List<string>>
        {
            { "map", new List<string>() { "map_result" } },
            { "tabular", new List<string>() { "site_level" } }
        };

        public ResultConfigReconstituteService(IRepositoryRegistry registry) : base(registry)
        {

        }

        public ResultConfig Reconstitute(ResultConfigDTO resultConfigDTO)
        {
            var resultConfig = new ResultConfig
            {
                SpecificationKeys = resultConfigDTO.AggregateKeys,
                RequestId = resultConfigDTO.RequestId,
                SessionId = resultConfigDTO.SessionId,
                ViewTypeId = resultConfigDTO.ViewTypeId,
                FacetCode = resultConfigDTO.FacetCode
            };

            if (resultConfig.ViewTypeId.IsEmpty() || !DefaultViewTypeResultFacet.ContainsKey(resultConfig.ViewTypeId))
                throw new ArgumentNullException("Unknown or empty viewType cannot be reconstituted!");

            if (resultConfig.FacetCode.IsEmpty()) {
                resultConfig.FacetCode = DefaultViewTypeResultFacet[resultConfig.ViewTypeId];
            }

            if (FixedViewTypeSpecificationKeys.ContainsKey(resultConfig.ViewTypeId)) {
                resultConfig.SpecificationKeys = FixedViewTypeSpecificationKeys[resultConfig.ViewTypeId];
            }

            resultConfig.Facet = GetFacetByCode(resultConfig.FacetCode);

            resultConfig.ResultSpecifications = Registry.Results.GetByKeys(resultConfig.SpecificationKeys);

            return resultConfig;
        }

        public ResultConfig Reconstitute(JObject resultConfigJson)
        {
            var resultConfig = Reconstitute(resultConfigJson.ToObject<ResultConfigDTO>());
            return resultConfig;
        }
    }
}
