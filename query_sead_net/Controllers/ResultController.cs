using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuerySeadDomain;
using QuerySeadDomain.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace QuerySeadAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        public IUnitOfWork Context { get; private set; }
        private Services.ILoadResultService ResultService { get; set; }

        public ResultController(IUnitOfWork context, Services.ILoadResultService resultService)
        {
            Context = context;
            ResultService = resultService;
        }

        // GET api/values
        [HttpGet("definition")]
        public IEnumerable<ResultAggregate> Get()
        {
            return Context.Results.GetAll().ToList();
        }

        // GET api/values/5
        [HttpGet("definition/{id}")]
        public ResultAggregate Get(int id)
        {
            return Context.Results.Get(id);
        }

        [HttpPost("load")]
        [Produces("application/json", Type = typeof(ResultContentSet))]
        [Consumes("application/json")]
        public ResultContentSet Load([FromBody]JObject data) //[FromBody]FacetsConfig2 facetsConfig, [FromBody]ResultConfig resultConfig)
        {
            FacetsConfig2 facetsConfig = data["facetsConfig"].ToObject<FacetsConfig2>();
            ResultConfig resultConfig = data["resultConfig"].ToObject<ResultConfig>();
            facetsConfig.SetContext(Context);
            var result = ResultService.Load(facetsConfig, resultConfig);
            //var json = JsonConvert.SerializeObject(result);
            return result;
        }

    }
}
