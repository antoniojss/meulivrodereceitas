using myRecipeBook.Domain.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace myRecipeBook.Infrastructure.Security.Tokens.Refresh
{
    internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string GenerateToken()
        {
            var token = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(token);
        }   
    }
}
