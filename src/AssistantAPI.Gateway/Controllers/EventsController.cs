using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssistantAPI.Gateway.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AssistantAPI.Gateway.Controllers
{
    [Route("[controller]")]
    public class EventsController : ApiController
    {
        private readonly ILogger<EventsController> _logger;

        public EventsController(AssistantClient client, ILogger<EventsController> logger) : base(client)
        {
            _logger = logger;
        }

        [HttpPost("{eventName}")]
        public async Task<IActionResult> SendEvent(string eventName) {
            var query = HttpContext.Request.Query;
            var eventData = HttpContext.Request.Query.Any() 
                    ? HttpContext.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()) 
                    : new Dictionary<string, string>();
            _logger.LogInformation($"Firing event '{eventName}' with event data: {eventData.ToValuesString()}");
            var result = await Client.SendEvent(eventName, eventData);
            return HandleResponse(result);
        }
        
        [ContentType("application/json")]
        [HttpPost("{eventName}")]
        public async Task<IActionResult> SendEvent(string eventName, [FromBody] Dictionary<string, string> eventData)
        {
            var query = HttpContext.Request.Query;
            eventData = eventData ?? 
                (HttpContext.Request.Query.Any() 
                    ? HttpContext.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()) 
                    : new Dictionary<string, string>()
                );
            _logger.LogInformation($"Firing event '{eventName}' with event data: {eventData.ToValuesString()}");
            var result = await Client.SendEvent(eventName, eventData);
            return HandleResponse(result);
        }
    }
}
