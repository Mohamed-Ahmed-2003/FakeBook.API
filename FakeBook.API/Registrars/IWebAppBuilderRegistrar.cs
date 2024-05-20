namespace FakeBook.API.Registrars
{
    public interface IWebAppBuilderRegistrar : IRegistrar
    {
        void RegisterServices(WebApplicationBuilder builder);
    }
}
