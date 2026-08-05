using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace myRecipeBook.Infrastructure.Security.Tokens.Access
{
    internal sealed class JwtTokenHandler : IAccessTokenGenerator
    {
        private readonly uint _expirationTimeMinutes;
        private readonly string _signingKey;

        public JwtTokenHandler(uint expirationTimeMinutes, string signingKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _signingKey = signingKey;
        }

        public string Generate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
//                      new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())

            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
                SigningCredentials = new SigningCredentials(Credentials(),SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var handler = new JsonWebTokenHandler();

            return handler.CreateToken(tokenDescriptor);
        }

        private SymmetricSecurityKey Credentials()
        {
            var keyBytes = Encoding.UTF8.GetBytes(_signingKey);

            return new SymmetricSecurityKey(keyBytes);
        }

    }
}
