using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SeadQueryAPI.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            System.Version version = typeof(VersionController).Assembly.GetName().Version;
            return version == null ? System.Array.Empty<string>() : version.ToString().Split(".");
        }
    }
}
