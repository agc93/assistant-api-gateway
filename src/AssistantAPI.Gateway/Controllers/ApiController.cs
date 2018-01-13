using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssistantAPI.Gateway.Controllers
{
    public class ApiController : Controller
    {
        protected AssistantClient Client { get; }
        protected ApiController(AssistantClient client)
        {
            Client = client;
        }

        protected IActionResult HandleResponse(HttpResponseMessage response) {
            if (response.IsSuccessStatusCode) {
                return Ok();
            }
            return NotFound();
        }
    }
}