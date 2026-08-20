using myRecipeBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Identity
{
  public interface ILoggedUser
    {
        Task<User> Get();
        Guid GetUserId();
  
    }
}
