
namespace FakeBook.API.Registrars
{
    public class ApiWebAppRegistrar : IWebAppRegistrar
    {
        public void RegisterPipelineComponents(WebApplication app)
        {

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
