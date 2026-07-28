using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace myRecipeBook.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
       Task<bool> ExistActiveUserWithEmail(string email);
        Task<Entities.User?> GetByEmail(string email);
    }
}
