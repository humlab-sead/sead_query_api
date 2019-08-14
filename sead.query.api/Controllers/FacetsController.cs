using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeadQueryCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace SeadQueryAPI.Controllers
{
    /// <summary>
    /// Controller for methods related to facet definition and facet content.
    /// Uses FacetRepository to fetch facet definitions
    /// Uses LoadFacetService to load content of facet
    /// </summary>
    [Route("api/[controller]")]
    public class FacetsController : Controller
    {
        /// <summary>
        /// Reference to current DB context
        /// </summary>
        public IRepositoryRegistry Context { get; private set; }
        /// <summary>
        /// Reference to facet contetnt load service
        /// </summary>
        public Services.ILoadFacetService LoadService { get; private set; }

        public FacetsController(IRepositoryRegistry context, Services.ILoadFacetService loadService)
        {
            Context = context;
            LoadService = loadService;
        }

        /// <summary>
        /// Returns a list of all avaliable facet definitions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json", Type = typeof(IEnumerable<FacetDefinition>))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK , Type = typeof(IEnumerable<FacetDefinition>))]
        public IEnumerable<FacetDefinition> Get()
        {
            return Context.Facets.GetAll().Where(z => z.FacetGroupId != 0 && z.IsApplicable == true).ToList();
        }

        /// <summary>
        /// Returns a specific facet definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(FacetDefinition))]
        [HttpGet("{id}")]
        [Produces("application/json", Type = typeof(IEnumerable<FacetDefinition>))]
        public FacetDefinition Get(int id)
        {
            return Context.Facets.Get(id);
        }

        /// <summary>
        /// Returns facet content given supplied configuration
        /// </summary>
        /// <remarks>
        /// The call result is cached (using redis) for subsequent calls, and based on hash key generated from supplied configuration
        /// </remarks>
        /// <returns></returns>
         //[SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(FacetContent))]
        [HttpPost("load")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public FacetContent Load([FromBody]FacetsConfig2 facetsConfig)
        {
            facetsConfig.SetContext(Context);
            return LoadService.Load(facetsConfig);
        }

        [HttpPost("load2")]
        [Produces("application/json", Type = typeof(FacetContent))]
        [Consumes("application/json")]
        public FacetContent Load2([FromBody]JObject json)
        {
            var facetsConfig = JsonConvert.DeserializeObject<FacetsConfig2>(json.ToString());
            facetsConfig.SetContext(Context);
            var facetContent = LoadService.Load(facetsConfig);
            return facetContent;
        }

        [HttpPost("load3")]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        public string Load3([FromBody]JObject json)
        {
            var facetsConfig = JsonConvert.DeserializeObject<FacetsConfig2>(json.ToString());
            facetsConfig.SetContext(Context);
            FacetContent facetContent = LoadService.Load(facetsConfig);
            string data = JsonConvert.SerializeObject(facetContent);
            return data;
        }

        [HttpPost("mirror")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public JObject Mirror([FromBody]JObject json)
        {
            return json;
        }
    }
}
