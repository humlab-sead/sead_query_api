using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable RCS1163, IDE0060

namespace query_sead_net.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "hello", "world", "!" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Hello {id}!";
        }

        [HttpPost]
        public void Post([FromBody] string _)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
