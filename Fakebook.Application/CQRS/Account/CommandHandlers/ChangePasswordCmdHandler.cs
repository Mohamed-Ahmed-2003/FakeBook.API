
using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class ChangePasswordHandler(UserManager<IdentityUser> userManager) : IRequestHandler<ChangePasswordCmd, Response<object>>
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<Response<object>> Handle(ChangePasswordCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();
            
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
            {
                response.AddError(StatusCode.NotFound, AccountErrorMessages.AccountNotFound);
                return response;
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddError(StatusCode.ValidationError, error.Description);
                }
                return response;
            }

            return response;
        }
    }

}
