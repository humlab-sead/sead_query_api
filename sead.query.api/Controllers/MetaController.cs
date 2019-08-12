using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeadQueryCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SeadQueryAPI.Controllers
{
    /// <summary>
    /// Controller for methods related to facet definition and facet content.
    /// Uses FacetRepository to fetch facet definitions
    /// Uses LoadFacetService to load content of facet
    /// </summary>
    [Route("api/[controller]")]
    public class MetaController : Controller
    {
        /// <summary>
        /// Reference to current DB context
        /// </summary>
        public IUnitOfWork Context { get; private set; }
        /// <summary>
        /// Reference to facet contetnt load service
        /// </summary>
        public Services.ILoadFacetService LoadService { get; private set; }

        public MetaController(IUnitOfWork context, Services.ILoadFacetService loadService)
        {
            Context = context;
            LoadService = loadService;
        }

        /// <summary>
        /// Returns a list of all avaliable facet groups
        /// </summary>
        /// <returns></returns>
        [HttpGet("facet/group")]
        [Produces("application/json", Type = typeof(IEnumerable<FacetGroup>))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK , Type = typeof(IEnumerable<FacetGroup>))]
        public IEnumerable<FacetGroup> GetGroups()
        {
            return Context.FacetGroups.GetAll().ToList();
        }

        /// <summary>
        /// Returns a list of all avaliable facet types
        /// </summary>
        /// <returns></returns>
        [HttpGet("facet/type")]
        [Produces("application/json", Type = typeof(IEnumerable<FacetType>))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(IEnumerable<FacetType>))]
        public IEnumerable<FacetType> GetTypes()
        {
            return Context.FacetTypes.GetAll().ToList();
        }

        public class FacetMetaData
        {
            public IEnumerable<FacetType> FacetType { get; set; }
            public IEnumerable<FacetGroup> FacetGroup { get; set; }
        }
        /// <summary>
        /// Returns facet meta data aggregate
        /// </summary>
        /// <returns></returns>
        [HttpGet("facet")]
        [Produces("application/json", Type = typeof(FacetMetaData))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(FacetMetaData))]
        public FacetMetaData GetFacetMetaData()
        {
            var types = Context.FacetTypes.GetAll().ToList();
            var groups = Context.FacetGroups.GetAll().ToList();
            return new FacetMetaData() { FacetGroup = groups, FacetType = types };
        }

    }
}
