using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            #region Jwt with swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference =  new OpenApiReference()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            #endregion
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
