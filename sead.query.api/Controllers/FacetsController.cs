using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;

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
        public Services.IFacetReconstituteService LoadService { get; private set; }
        public IFacetConfigReconstituteService ReconstituteConfigService { get; }

        public FacetsController(
            IRepositoryRegistry context,
            IFacetConfigReconstituteService reconstituteConfigService,
            Services.IFacetReconstituteService loadService
        )
        {
            Context = context;
            LoadService = loadService;
            ReconstituteConfigService = reconstituteConfigService;
        }

        /// <summary>
        /// Returns a list of all avaliable facet definitions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json", Type = typeof(IEnumerable<Facet>))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK , Type = typeof(IEnumerable<Facet>))]
        public IEnumerable<Facet> Get()
        {
            var facets = Context.Facets.GetAllUserFacets();
            return facets;
        }

        /// <summary>
        /// Returns a specific facet definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Facet))]
        [HttpGet("{id}")]
        [Produces("application/json", Type = typeof(IEnumerable<Facet>))]
        public Facet Get(int id)
        {
            return Context.Facets.Get(id);
        }

        /// <summary>
        /// Returns domain facets
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Facet))]
        [HttpGet("domain")]
        [Produces("application/json", Type = typeof(IEnumerable<Facet>))]
        public IEnumerable<Facet> GetDomainFacets()
        {
            return Context.Facets.Parents().ToList();
        }

        /// <summary>
        /// Returns a specific facet definition
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Facet))]
        [HttpGet("domain/{id}")]
        [Produces("application/json", Type = typeof(IEnumerable<Facet>))]
        public IEnumerable<Facet> GetDomainFacetChildren(string id)
        {
            return Context.Facets.Children(id);
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
            var reconstitutedFacetConfig = ReconstituteConfigService.Reconstitute(facetsConfig);
            var facetContent = LoadService.Load(reconstitutedFacetConfig);
            return facetContent;
        }

        [HttpPost("load2")]
        [Produces("application/json", Type = typeof(FacetContent))]
        [Consumes("application/json")]
        public FacetContent Load2([FromBody]JObject json)
        {
            var facetsConfig = ReconstituteConfigService.Reconstitute(json);
            var facetContent = LoadService.Load(facetsConfig);
            return facetContent;
        }

        [HttpPost("load3")]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        public string Load3([FromBody]JObject json)
        {
            var facetsConfig = ReconstituteConfigService.Reconstitute(json);
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
