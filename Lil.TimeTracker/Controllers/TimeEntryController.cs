using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lil.TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntryController : ControllerBase
    {
        // GET: api/<TimeEntryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TimeEntryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TimeEntryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TimeEntryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TimeEntryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
