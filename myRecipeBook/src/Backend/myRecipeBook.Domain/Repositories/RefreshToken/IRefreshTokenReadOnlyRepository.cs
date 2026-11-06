using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.RefreshToken
{
    public  interface IRefreshTokenReadOnlyRepository
    {
        Task<Entities.RefreshToken?> Get(string refreshToken);   
    }
}
