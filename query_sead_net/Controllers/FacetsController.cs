using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuerySeadDomain;

namespace QuerySeadAPI.Controllers
{
    [Route("api/[controller]")]
    public class FacetsController : Controller
    {
        public IUnitOfWork Context { get; private set; }
        public Services.ILoadFacetService LoadService { get; private set; }

        public FacetsController(IUnitOfWork context, Services.ILoadFacetService loadService)
        {
            Context = context;
            LoadService = loadService;
        }

        [HttpGet]
        public IEnumerable<FacetDefinition> Get()
        {
            return Context.Facets.GetAll().ToList();
        }

        [HttpGet("{id}")]
        public FacetDefinition Get(int id)
        {
            return Context.Facets.Get(id);
        }

        [HttpGet("load")]
        public FacetContent Load([FromBody]FacetsConfig2 facetsConfig)
        {
            return LoadService.Load(facetsConfig);
        }
    }
}
