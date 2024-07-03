
using Asp.Versioning;
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


            builder.Services.AddEndpointsApiExplorer();
        }
    }
}
