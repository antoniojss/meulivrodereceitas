using myRecipeBook.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.Recipe
{
   public interface IRecipeReadOnlyRepository
    {
        Task<Entities.Recipe?> GetById(Guid recipeId , Guid userId);
        Task<IList<Entities.Recipe>> GetRecentRecipes( Guid userId);
        Task<IList<Entities.Recipe>> FilterRecipes(Guid userId, RecipeFilterDto filter);
    }
}
