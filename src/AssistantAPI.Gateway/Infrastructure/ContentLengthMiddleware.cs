using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AssistantAPI.Gateway.Infrastructure
{
    public class ContentLengthMiddleware
    {
        private readonly RequestDelegate _next;

        public ContentLengthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var contentLength = context.Request.ContentLength;
            if (contentLength == null) {
                contentLength = 0;
            }
            
            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }
}