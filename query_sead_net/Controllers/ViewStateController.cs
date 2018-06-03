using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuerySeadDomain;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace QuerySeadAPI.Controllers
{
    [Route("api/[controller]")]
    public class ViewStateController : Controller
    {
        /// <summary>
        /// Reference to current DB context
        /// </summary>
        public IUnitOfWork Context { get; private set; }
        /// <summary>
        /// Reference to facet contetnt load service
        /// </summary>
        public Services.ILoadFacetService LoadService { get; private set; }

        public ViewStateController(IUnitOfWork context)
        {
            Context = context;
        }

        [HttpGet("{id}")]
        [Produces("application/json", Type = typeof(IEnumerable<FacetDefinition>))]
        public ViewState Get(string id)
        {
            var value = Context.ViewStates.Get(id);
            if (value == null)
                return new ViewState() { Key = id, Data = null };
            return value;
        }

        [HttpPost("")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public ViewState Store([FromBody]ViewState viewstate)
        {
            Context.ViewStates.Add(viewstate);
            Context.Commit();
            return viewstate;
        }

    }
}
