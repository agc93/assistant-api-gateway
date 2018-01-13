using System;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AssistantAPI.Gateway.Infrastructure
{
    public class ContentTypeAttribute : Attribute, IActionConstraint
    {
        public ContentTypeAttribute(string contentType)
        {
            ContentType = contentType;
        }
        public int Order => 0;

        public string ContentType { get; }

        public bool Accept(ActionConstraintContext context)
        {
            return context.RouteContext.HttpContext.Request.ContentType == ContentType;
        }
    }
}