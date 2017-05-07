using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuerySeadDomain;

namespace query_sead_net.Controllers
{
    [Route("api/[controller]/")]
    public class ResultController : Controller
    {
        public IUnitOfWork Context { get; private set; }

        public ResultController(IUnitOfWork context)
        {
            Context = context;
        }

        // GET api/values
        [HttpGet("definition")]
        public IEnumerable<ResultDefinition> Get()
        {
            return Context.Results.GetAll().ToList();
        }

        // GET api/values/5
        [HttpGet("definition/{id}")]
        public ResultDefinition Get(int id)
        {
            return Context.Results.Get(id);
        }

        [HttpGet("load")]
        public FacetContent Load([FromBody]FacetsConfig2 facetsConfig)
        {
            return null; // LoadService.Load(facetsConfig);
        }

    }
}
