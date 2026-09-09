using myRecipeBook.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.Recipe
{
   public interface IRecipeReadOnlyRepository
    {
        Task<Entities.Recipe?> GetById(Guid recipeId , Guid userId);
        Task<IList<RecipeSummaryDto>> GetRecentRecipes( Guid userId);
        Task<IList<RecipeSummaryDto>> FilterRecipes(Guid userId, RecipeFilterDto filter);
    }
}
