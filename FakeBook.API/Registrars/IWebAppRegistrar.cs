namespace FakeBook.API.Registrars
{
    public interface IWebAppRegistrar : IRegistrar
    {
        void RegisterPipelineComponents (WebApplication app);
    }
}
