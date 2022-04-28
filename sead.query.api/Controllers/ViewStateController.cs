﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeadQueryCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SeadQueryAPI.Controllers
{
    [Route("api/[controller]")]
    public class ViewStateController : Controller
    {
        /// <summary>
        /// Reference to current DB context
        /// </summary>
        public IRepositoryRegistry Context { get; private set; }
        /// <summary>
        /// Reference to facet contetnt load service
        /// </summary>
        public Services.IFacetContentReconstituteService LoadService { get; private set; }

        public ViewStateController(IRepositoryRegistry context)
        {
            Context = context;
        }

        [HttpGet("{id}")]
        [Produces("application/json", Type = typeof(IEnumerable<Facet>))]
        public ViewState Get(string id)
        {
            return Context.ViewStates.Get(id) ?? new ViewState() { Key = id, Data = null };
        }

        [HttpPost("")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public ViewState Store([FromBody] ViewState viewstate)
        {
            Context.ViewStates.Add(viewstate);
            Context.Commit();
            return viewstate;
        }
    }
}
