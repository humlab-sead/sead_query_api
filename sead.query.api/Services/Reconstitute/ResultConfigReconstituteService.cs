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
        //public static Dictionary<string, List<string>> FixedViewTypeSpecificationKeys = new Dictionary<string, List<string>>
        //{
        //    { "map", new List<string>() { "map_result" } },
        //    { "tabular", new List<string>() { "site_level" } }
        //};

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

            if (resultConfig.ViewTypeId.IsEmpty())
                throw new ArgumentNullException("Empty viewType cannot be reconstituted!");

            resultConfig.ViewType = Registry.Results.GetViewType(resultConfig.ViewTypeId);

            if (resultConfig.FacetCode.IsEmpty()) {
                resultConfig.FacetCode = resultConfig.ViewType.ResultFacetCode;
            }

            //if (resultConfig.SpecificationKeys.Count == 0) {
                resultConfig.SpecificationKeys = resultConfig.ViewType.SpecificationKey.WrapToList();
            //}

            resultConfig.Facet = GetFacetByCode(resultConfig.FacetCode);
            resultConfig.Specifications = Registry.Results.GetByKeys(resultConfig.SpecificationKeys);

            return resultConfig;
        }

        public ResultConfig Reconstitute(JObject resultConfigJson)
        {
            var resultConfig = Reconstitute(resultConfigJson.ToObject<ResultConfigDTO>());
            return resultConfig;
        }
    }
}
