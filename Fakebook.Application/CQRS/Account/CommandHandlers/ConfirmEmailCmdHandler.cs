using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class ConfirmEmailCmdHandler(UserManager<IdentityUser> userManager) : IRequestHandler<ConfirmEmailCmd, Response<object>>
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<Response<object>> Handle(ConfirmEmailCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
            {
                response.AddError(StatusCodes.NotFound, AccountErrorMessages.AccountNotFound);
                return response;
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddError(StatusCodes.ValidationError, error.Description);
                }
                return response;
            }

            return response;
        }
    }
}
