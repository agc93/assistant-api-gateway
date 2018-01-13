using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AssistantAPI.Gateway
{
    public static class StartupExtensions
    {
        public static IMvcCoreBuilder AddMvcServices(this IServiceCollection services)
        {
            return services.AddMvcCore(opts =>
            {
                opts.AddFormatterMappings();
            })
            .AddJsonOptions(j =>
            {
                j.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .AddJsonFormatters()
            .AddApiExplorer()
            .AddDataAnnotations();
        }

        public static MvcOptions AddFormatterMappings(this MvcOptions opts)
        {
            opts.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
            opts.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
            return opts;
        }

        public static IApplicationBuilder UseWildcardCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors(c =>
                c.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            return app;
        }
    }
}