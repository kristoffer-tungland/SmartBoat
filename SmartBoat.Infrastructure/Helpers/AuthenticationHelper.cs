using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmartBoat.Infrastructure.Models.Identity;
using SmartBoat.Infrastructure.Settings;

namespace SmartBoat.Infrastructure.Helpers
{
    public static class AuthenticationHelper
    {
        public static string GenerateJwtToken(string email, ApplicationUser user, IMongoDbSettings settings)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(settings.JwtExpireDays));

            var token = new JwtSecurityToken(
                settings.JwtIssuer,
                settings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
