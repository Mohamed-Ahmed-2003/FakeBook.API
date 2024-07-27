using Fakebook.Application.Options;
using Fakebook.Application.Services;
using FakeBook.Domain.Aggregates.Shared;

namespace FakeBook.API.Registrars
{
    public class ColudinaryRegistrar : IWebAppBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {

            var coludinarySettings = new ColudinarySettings();
            builder.Configuration.Bind(nameof(ColudinarySettings), coludinarySettings);

            var coludinarySection = builder.Configuration.GetSection(nameof(ColudinarySettings));
            builder.Services.Configure<ColudinarySettings>(coludinarySection);
            builder.Services.AddScoped<MediaService>();
        }
    }
}
