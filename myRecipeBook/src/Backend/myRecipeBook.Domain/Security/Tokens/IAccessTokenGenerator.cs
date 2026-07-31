using myRecipeBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        string Generate(User user);
    }
}
