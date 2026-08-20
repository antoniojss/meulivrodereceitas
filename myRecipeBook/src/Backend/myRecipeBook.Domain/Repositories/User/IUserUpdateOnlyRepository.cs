using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.User
{
    public  interface IUserUpdateOnlyRepository
    {
        void UpdateProfile(Entities.User user); 
        Task UpdatePassword(Guid userId, string passwordHash);
    }
}
