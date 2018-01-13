using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AssistantAPI.Gateway.Configuration;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AssistantAPI.Gateway
{
    public class AssistantClient
    {
        private readonly ILogger<AssistantClient> _logger;

        public AssistantClient(IOptions<HomeAssistantOptions> options, ILogger<AssistantClient> logger)
        {
            BaseUrl = (options.Value.Address.StartsWith("http") ? options.Value.Address : $"http://{options.Value.Address}").AppendPathSegment("api");
            // DefaultRequestHeaders.Accept
            //     .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            FlurlHttp.Configure(settings => {
                var jsonSettings = new JsonSerializerSettings {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    Formatting = Formatting.None,
                    StringEscapeHandling = StringEscapeHandling.Default
                };
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            });
            _logger = logger;
            _logger.LogInformation($"Initialized AssistantClient for '{BaseUrl}'");
        }

        private string BaseUrl { get; }

        public async Task<HttpResponseMessage> SendEvent(string eventName, Dictionary<string, string> eventData = null)
        {
            eventData = eventData ?? new Dictionary<string, string>();
            var url = BaseUrl
                .AppendPathSegment("events")
                .AppendPathSegment(eventName);
            _logger.LogDebug($"Making client call to '{url}' using event data: {eventData.ToValuesString()}");
            var result = await url.PostJsonAsync(eventData);
            _logger.LogDebug($"Received response from API: {result.StatusCode}/{(string.IsNullOrWhiteSpace(result.ReasonPhrase) ? "no reason" : result.ReasonPhrase)}");
            return result;
        }

        public async Task<HttpResponseMessage> CallService(string domain, string service, Dictionary<string, string> serviceData = null) {
            serviceData = serviceData ?? new Dictionary<string, string>();
            var url = BaseUrl
                .AppendPathSegment("services")
                .AppendPathSegment(domain)
                .AppendPathSegment(service);
            _logger.LogDebug($"Making service call to '{url}' using service data: {serviceData.ToValuesString()}");
            HttpResponseMessage resp;
            if (serviceData != null && serviceData.Any()) {
                resp = await url.PostJsonAsync(serviceData);
            } else {
                resp = await url.PostAsync(null);
            }
            _logger.LogDebug($"Received response from API: {resp.StatusCode}/{(string.IsNullOrWhiteSpace(resp.ReasonPhrase) ? "no reason" : resp.ReasonPhrase)}");
            return resp;
        }
    }
}