using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Security.Tokens
{
    public  interface IRefreshTokenGenerator
    {
        string GenerateToken();
    }
}
