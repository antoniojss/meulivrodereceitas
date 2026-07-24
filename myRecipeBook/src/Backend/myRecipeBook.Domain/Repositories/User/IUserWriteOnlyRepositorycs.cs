using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.User
{
  public  interface IUserWriteOnlyRepository
    {
        Task Add(Entities.User users);
    }
}
