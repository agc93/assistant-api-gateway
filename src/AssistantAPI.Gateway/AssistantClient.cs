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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AssistantAPI.Gateway
{
    public class AssistantClient
    {
        public AssistantClient(IOptions<HomeAssistantOptions> options)
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
        }

        private string BaseUrl { get; }

        public async Task<HttpResponseMessage> SendEvent(string eventName, Dictionary<string, string> eventData = null)
        {
            eventData = eventData ?? new Dictionary<string, string>();
            var result = await BaseUrl
                .AppendPathSegment("events")
                .AppendPathSegment(eventName)
                .PostJsonAsync(eventData);
            return result;
        }

        public async Task<HttpResponseMessage> CallService(string domain, string service, Dictionary<string, string> serviceData = null) {
            serviceData = serviceData ?? new Dictionary<string, string>();
            var url = BaseUrl
                .AppendPathSegment("services")
                .AppendPathSegment(domain)
                .AppendPathSegment(service);
            HttpResponseMessage resp;
            if (serviceData != null && serviceData.Any()) {
                resp = await url.PostJsonAsync(serviceData);
            } else {
                resp = await url.PostAsync(null);
            }
            return resp;
        }
    }
}