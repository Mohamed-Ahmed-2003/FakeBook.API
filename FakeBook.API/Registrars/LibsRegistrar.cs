
using Fakebook.Application.CQRS.Profile.Queries;

namespace FakeBook.API.Registrars
{
    public class LibsRegistrar : IWebAppBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfilesQuery));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(GetAllUserProfilesQuery));
            });

        }
    }
}
