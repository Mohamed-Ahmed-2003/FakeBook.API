
using Asp.Versioning.ApiExplorer;
using FakeBook.API.RealTime;
using FakeBook.API.RealTime.FakeBook.API.RealTime;

namespace FakeBook.API.Registrars
{
    public class ApiWebAppRegistrar : IWebAppRegistrar
    {
        public void RegisterPipelineComponents(WebApplication app)
        {
            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{

            //}
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
                }
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseCors(c =>
            {
                c.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub"); 
            app.MapHub<OnlineHub>("/onlineHub"); 

        }
    }
}
