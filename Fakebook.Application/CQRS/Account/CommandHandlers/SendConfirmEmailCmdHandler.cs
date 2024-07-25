
using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Services;
using Fakebook.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class SendConfirmEmailCmdHandler (IEmailService emailService, IConfiguration configuration , UserManager<IdentityUser> userManager) : IRequestHandler<SendConfirmEmailCmd,Response<object>>
    {
        private readonly IEmailService _emailService = emailService;
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<Response<object>> Handle(SendConfirmEmailCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
            {
                response.AddError(StatusCodes.NotFound, AccountErrorMessages.AccountNotFound );
                return response;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var baseUrl = _configuration["BaseUrl"];
            var confirmationLink = $"{baseUrl}/account/confirm-email?UserId={user.Id}&Token={token}";

            try
            {
                await _emailService.SendConfirmationEmail(user.Email, confirmationLink);
            }
            catch (Exception ex)
            {
                response.AddError(StatusCodes.ServerError, "An error occurred while sending the confirmation email. \n"+ex.Message);
                return response;
            }

            return response;
        }
    }
}
