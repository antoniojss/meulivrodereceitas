using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Security.PasswordHashing
{
    public  interface IPasswordHasher
    {
        string HashPassword(string password); 
        bool VerifyPassword(string password, string passwordHash);
    }
}
