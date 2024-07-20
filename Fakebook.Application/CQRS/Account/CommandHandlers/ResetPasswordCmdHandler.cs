using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class ResetPasswordCmdHandler : IRequestHandler<ResetPasswordCmd, Response<object>>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ResetPasswordCmdHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<object>> Handle(ResetPasswordCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();

            // Replace spaces with '+' in the token
            request.Token = request.Token.Replace(" ", "+");

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.AddError(StatusCode.NotFound, string.Format(AccountErrorMessages.AccountNotFound, request.Email));
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.AddError(StatusCode.ValidationError, AccountErrorMessages.PasswordResetFailed);
                return response;
            }

            response.Payload = new { Message = "Password reset successfully" };
            return response;
        }
    }

}
