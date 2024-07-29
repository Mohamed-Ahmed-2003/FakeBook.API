using Fakebook.Application.Options;
using Fakebook.Application.Services;


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

            builder.Services.Configure<MediaSettings>(builder.Configuration.GetSection("MediaSettings"));

        }
    }
}
