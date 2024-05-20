using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FakeBook.API.Options
{
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider  Provider) : IConfigureOptions<SwaggerGenOptions>
    {
        private  readonly IApiVersionDescriptionProvider _provider = Provider;
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, CreateVersionInfo(desc));
            }
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
        {

            var info = new OpenApiInfo
            {
                Title = "FakeBook",
                Version = desc.ApiVersion.ToString(),
            };
            if (desc.IsDeprecated)
            {
                info.Description = "This Api version has been deprecated";
            }
            return info;
        }
    }
}
