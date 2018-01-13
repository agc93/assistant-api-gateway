using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssistantAPI.Gateway.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AssistantAPI.Gateway.Controllers
{
    [Route("[controller]")]
    public class ServicesController : ApiController
    {
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(AssistantClient client, ILogger<ServicesController> logger) : base(client)
        {
            _logger = logger;
        }

        [HttpPost("{serviceName}")]
        public async Task<IActionResult> CallService(string serviceName) {
            var eventData = HttpContext.Request.Query.Any() 
                    ? HttpContext.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()) 
                    : null;
            var resp = await Client.CallService(serviceName.Split('.').First(), serviceName.Split('.').Last(), eventData);
            return HandleResponse(resp);
        }

        [HttpPost("{domain}/{serviceName}")]
        public async Task<IActionResult> CallDomainService(string domain, string serviceName) {
            var eventData = HttpContext.Request.Query.Any() 
                    ? HttpContext.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()) 
                    : null;
            var resp = await Client.CallService(domain, serviceName, eventData);
            return HandleResponse(resp);
        }

        [ContentType("application/json")]
        [HttpPost("{domain}/{serviceName}")]
        public async Task<IActionResult> CallDomainService(string domain, string serviceName, [FromBody]Dictionary<string, string> serviceData) {
            var resp = await Client.CallService(domain, serviceName, serviceData);
            return HandleResponse(resp);
        }
    }
}