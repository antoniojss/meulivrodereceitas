using IBM.Data.Db2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.DataAccess.Repositories
{
    internal sealed class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public RecipeRepository(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Recipe recipe)
        {
            await _dbContext.Recipes.AddAsync(recipe);
        }

        public async Task<bool> DeleteById(Guid recipeId, Guid userId)
        {
            var recipe = await _dbContext
                .Recipes
               .Where(recipe => recipe.Active &&
                                recipe.Id == recipeId &&
                                recipe.UserId == userId)
               .ExecuteDeleteAsync();

            return recipe > 0;
        }
        async Task<Recipe?> IRecipeReadOnlyRepository.GetById(Guid recipeId, Guid userId)
        {
            return await GetFullRecipes()
                .AsNoTracking()
                .FirstOrDefaultAsync(recipe => recipe.Active &&
                                               recipe.Id == recipeId &&
                                               recipe.UserId == userId);
        }

        async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(Guid recipeId, Guid userId)
        {
            return await GetFullRecipes()
                .FirstOrDefaultAsync(recipe => recipe.Active &&
                                               recipe.Id == recipeId &&
                                               recipe.UserId == userId);
        }

        public IIncludableQueryable<Recipe, IOrderedEnumerable<RecipeInstruction>> GetFullRecipes()
        {
            return _dbContext
                .Recipes
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.DishTypes)
                .Include(recipe => recipe.Instructions.OrderBy(instruction => instruction.Order));
        }

        public async Task<IList<Recipe>> GetRecentRecipes(Guid userId)
        {
            return await _dbContext
                .Recipes
                .AsNoTracking()
                .Where(recipe => recipe.Active && recipe.UserId == userId)
                .OrderByDescending(recipe => recipe.Id)
                .Take(6)
                .ToListAsync(); 
        }
    }
}
