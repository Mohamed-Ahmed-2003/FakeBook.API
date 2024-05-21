
using Fakebook.DAL;
using Microsoft.EntityFrameworkCore;

namespace FakeBook.API.Registrars
{
    public class DbRegistrar : IWebAppBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var conn = builder.Configuration.GetConnectionString("DBConn");
            builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(conn));

        }
    }
}
