using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class ForgotPasswordCmdHandler (UserManager<IdentityUser> userManager,IEmailService emailService, IConfiguration  configuration) : IRequestHandler<ForgotPasswordCmd, Response<object>>
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IEmailService _emailService = emailService;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Response<object>> Handle(ForgotPasswordCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                response.AddError(StatusCode.NotFound, string.Format(AccountErrorMessages.AccountNotFound, request.Email));
                return response;
            }

            try
            {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // Send token via email (implementation not shown)
                var baseUrl = _configuration["BaseUrl"];;

                var passwordResetLink = $"{baseUrl}/account/reset-password?Email={Uri.EscapeDataString(request.Email)}&Token={Uri.EscapeDataString(token)}";

                await _emailService.SendEmailAsync(request.Email, "Reset Your Password", $"Reset your password by <a href='{passwordResetLink}'>clicking here</a>.", true);
            }
            catch (Exception ex)
            {
                throw;
            }
            

            return response;
        }
    }
}
