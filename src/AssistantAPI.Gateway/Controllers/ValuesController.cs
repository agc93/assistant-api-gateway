using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AssistantAPI.Gateway.Controllers
{
    [Route("[controller]")]
    public class EventsController : Controller
    {
        public AssistantClient Client { get; }

        public EventsController(AssistantClient client)
        {
            Client = client;
        }
        // GET api/values
        [HttpPost("{eventName}")]
        public async Task<IActionResult> SendEvent(string eventName, [FromBody] Dictionary<string, string> eventData)
        {
            var query = HttpContext.Request.Query;
            eventData = eventData ?? 
                (HttpContext.Request.Query.Any() 
                    ? HttpContext.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()) 
                    : new Dictionary<string, string>()
                );
            var result = await Client.SendEvent(eventName, eventData);
            if (result.IsSuccessStatusCode) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
