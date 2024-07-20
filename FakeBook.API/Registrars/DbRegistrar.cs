
using Fakebook.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FakeBook.API.Registrars
{
    public class DbRegistrar : IWebAppBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var conn = builder.Configuration.GetConnectionString("DBConn");
            builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(conn));

            builder.Services.AddIdentityCore<IdentityUser>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength= 5;

                opt.ClaimsIdentity.UserIdClaimType = "IdentityId";
            }
            ).AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        }
    }
}
