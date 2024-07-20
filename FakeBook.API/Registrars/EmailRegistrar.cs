
using Fakebook.Application.Options;
using Fakebook.Application.Services;

namespace FakeBook.API.Registrars
{
    public class EmailRegistrar : IWebAppBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
         

            #region appSetting mail values into mailsettings object
            var mailSettings = new EmailSettings();
            builder.Configuration.Bind(nameof(EmailSettings), mailSettings);
            var mailSection = builder.Configuration.GetSection(nameof(EmailSettings));
            builder.Services.Configure<EmailSettings>(mailSection);
            #endregion

            builder.Services.AddTransient<IEmailService, EmailService>();

        }
    }
    }
