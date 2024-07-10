
using Asp.Versioning.ApiExplorer;

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
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
