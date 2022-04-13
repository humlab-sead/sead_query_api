using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeadQueryCore;
using SeadQueryCore.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.DTO;

namespace SeadQueryAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        public IRepositoryRegistry Context { get; private set; }
        private Services.ILoadResultService ResultService { get; set; }
        public IFacetConfigReconstituteService ReconstituteFacetsConfigService { get; }
        public IResultConfigReconstituteService ReconstituteResultConfigService { get; }

        public ResultController(
            IRepositoryRegistry context,
            IFacetConfigReconstituteService reconstituteFacetsConfigService,
            IResultConfigReconstituteService reconstituteResultConfigService,
            Services.ILoadResultService resultService
        )
        {
            Context = context;
            ResultService = resultService;
            ReconstituteFacetsConfigService = reconstituteFacetsConfigService;
            ReconstituteResultConfigService = reconstituteResultConfigService;
        }

        [HttpGet("definition")]
        public IEnumerable<ResultSpecification> Get()
        {
            return Context.Results.GetAll().ToList();
        }

        [HttpGet("definition/{id}")]
        public ResultSpecification Get(int id)
        {
            return Context.Results.Get(id);
        }

        [HttpPost("load")]
        [Produces("application/json", Type = typeof(ResultContentSet))]
        [Consumes("application/json")]
        public ResultContentSet Load([FromBody] JObject data)
        {
            var facetsConfig = ReconstituteFacetsConfigService.Reconstitute(GetFacetsConfig(data));
            var resultConfig = ReconstituteResultConfigService.Reconstitute(GetResultConfig(data));
            var result = ResultService.Load(facetsConfig, resultConfig).Nullify();
            return result;
        }

        private ResultConfigDTO GetResultConfig(JObject data)
            => data["resultConfig"].ToObject<ResultConfigDTO>();

        private FacetsConfig2 GetFacetsConfig(JObject data)
            => data["facetsConfig"].ToObject<FacetsConfig2>();
    }
}
