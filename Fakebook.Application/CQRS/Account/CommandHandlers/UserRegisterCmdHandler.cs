using AutoMapper;
using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.CQRS.Account.Dtos;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class UserRegisterCmdHandler(DataContext context, UserManager<IdentityUser> userManager, JwtService jwtService , IMapper mapper, IEmailService emailService,IConfiguration configuration) : IRequestHandler<UserRegisterCmd, Response<IdentityUserProfileDto>>
    {
        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly JwtService _jwtService = jwtService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Response<IdentityUserProfileDto>> Handle(UserRegisterCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<IdentityUserProfileDto>();

            try
            {
                // create Identity User

                var usedBefore = await _userManager.FindByEmailAsync(request.Username) != null;

                if (usedBefore)
                {
                    result.AddError(StatusCode.IdentityUserAlreadyExists, AccountErrorMessages.UserNameTaken);
                    return result;
                }

                var user = new IdentityUser
                {
                    Email = request.Username,
                    UserName = request.Username,
                    PhoneNumber = request.Phone,
                };
                using var transaction = await _context.Database.BeginTransactionAsync();

                var res = await _userManager.CreateAsync(user, request.Password);
                // create User Profile 
                if (!res.Succeeded)
                {
                    await transaction.RollbackAsync();
                    foreach (var err in res.Errors)
                    {
                        result.AddError(StatusCode.IdentityCreationFailed, err.Description);
                    }
                    return result;
                }
                var userInfo = GeneralInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username
                    , request.Phone, request.DateOfBirth, request.City);

                var profile = UserProfile.CreateUserProfile(user.Id, userInfo);
                try
                {
                    await _context.Set<UserProfile>().AddAsync(profile);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                var mapped = _mapper.Map<IdentityUserProfileDto>(profile);
                mapped.UserName = user.UserName;

                mapped.Token = _jwtService.GenerateJwtToken(user, profile.UserProfileId);

                // Generate the token

                result.Payload = mapped;

              
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var baseUrl = _configuration["BaseUrl"];
                    var confirmationLink = $"{baseUrl}/account/confirm-email?UserId={user.Id}&Token={token}";
              
                    await _emailService.SendConfirmationEmail(user.Email,confirmationLink);


            }
            catch (ProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(er => result.AddError(StatusCode.ValidationError, er));

            }
            catch (Exception ex)
            {
                result.AddError(StatusCode.UnknownError, ex.Message);
            }
            return result;

        }
    }
}
