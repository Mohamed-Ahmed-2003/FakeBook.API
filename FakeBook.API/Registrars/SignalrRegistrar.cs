
using Fakebook.Application.Generics.Interfaces;
using FakeBook.API.RealTime;

namespace FakeBook.API.Registrars
{
    public class SignalrRegistrar : IWebAppBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IChatNotifier , ChatNotifier>();
            builder.Services.AddSingleton<OnlineTracker>();

        }
    }
}
