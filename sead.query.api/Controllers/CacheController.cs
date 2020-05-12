using Microsoft.AspNetCore.Mvc;
using SeadQueryCore;

namespace query_sead_net.Controllers
{
    [Route("api/[controller]")]
    public class CacheController : Controller
    {
        private readonly ISeadQueryCache cache;

        public CacheController(ISeadQueryCache cache)
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
