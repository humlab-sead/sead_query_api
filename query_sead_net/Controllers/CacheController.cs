using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuerySeadAPI;

namespace query_sead_net.Controllers
{
    [Route("api/[controller]")]
    public class CacheController : Controller
    {
        private ICache cache;

        public CacheController(ICache cache)
        {
            this.cache = cache;

        }
        [HttpGet("clear")]
        public string Clear()
        {
            cache.Clear();
            return "OK";
        }

    }
}
