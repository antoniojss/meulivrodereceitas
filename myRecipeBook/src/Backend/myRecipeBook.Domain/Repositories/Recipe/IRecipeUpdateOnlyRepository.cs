using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.Recipe
{
   public interface IRecipeUpdateOnlyRepository
    {
      Task<Entities.Recipe?> GetById(Guid recipeId, Guid userId);
    }
}