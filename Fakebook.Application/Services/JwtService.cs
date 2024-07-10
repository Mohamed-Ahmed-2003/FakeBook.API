using Fakebook.Application.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fakebook.Application.Services
{
    public class JwtService(IOptions<JwtSettings> jwtSettings)
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public string GenerateJwtToken(IdentityUser user, Guid profileId)
        {
            var claims = new[]
            {
              new Claim (JwtRegisteredClaimNames.Sub , user.UserName),
              new Claim (JwtRegisteredClaimNames.Email, user.Email),
              new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim ("ProfileId",profileId.ToString()),
              new Claim ("UserId",user.Id),
        };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audiences[0],
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
