using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.Recipe
{
  public interface IRecipeWriteOnlyRepository
    {
        Task Add(Entities.Recipe recipe);
    }
}
