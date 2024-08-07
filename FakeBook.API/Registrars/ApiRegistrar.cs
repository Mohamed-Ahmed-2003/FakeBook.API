﻿using Asp.Versioning;
using Fakebook.Application.Services;
using FakeBook.API.Filters;

namespace FakeBook.API.Registrars
{
    public class ApiRegistrar : IWebAppBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {

            builder.Services.AddControllers(config =>
            {
                config.Filters.Add(typeof(ExceptionHandler));
            });
            builder.Services.AddCors();
            builder.Services.AddApiVersioning(conf =>
            {
                conf.DefaultApiVersion = new ApiVersion(1, 0);
                conf.AssumeDefaultVersionWhenUnspecified = true;
                conf.ReportApiVersions = true;
                conf.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(conf =>
            {
                conf.GroupNameFormat = "'v'VVV";
                conf.SubstituteApiVersionInUrl = true;
            });
            builder.Services.AddScoped<JwtService>();

            builder.Services.AddEndpointsApiExplorer();
        }
    }
}
