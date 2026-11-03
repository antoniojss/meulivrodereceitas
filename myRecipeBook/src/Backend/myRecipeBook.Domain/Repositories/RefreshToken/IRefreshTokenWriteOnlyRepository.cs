using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.RefreshToken
{
   public  interface IRefreshTokenWriteOnlyRepository
    {
        Task Replace(Entities.RefreshToken refreshToken);
    }
}
