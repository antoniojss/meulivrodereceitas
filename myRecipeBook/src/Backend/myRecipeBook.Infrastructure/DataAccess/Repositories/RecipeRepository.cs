using FluentMigrator.Runner;
using IBM.Data.Db2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic;
using myRecipeBook.Domain.Dtos;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Extensions;
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

        public async Task<IList<RecipeSummaryDto>> GetRecentRecipes(Guid userId)
        {
            return await _dbContext
                .Recipes
                .AsNoTracking()
                .Where(recipe => recipe.Active && recipe.UserId == userId)
                .OrderByDescending(recipe => recipe.Id)
                .Take(6)
                .Select(recipe => new RecipeSummaryDto(recipe.Id, recipe.Title))
                .ToListAsync();
        }

        public async Task<IList<RecipeSummaryDto>> FilterRecipes(Guid userId, RecipeFilterDto filter)
        {
            var query = _dbContext
                            .Recipes
                            .AsNoTracking()
                            .Where(recipe => recipe.Active && recipe.UserId == userId);

            if (filter.CookTime is not null)
                query = query.Where(recipe => recipe.CookTime == filter.CookTime);

            if (filter.SearchTerm.IsNotEmpty())
                query = query.Where(recipe => recipe.Title.Contains(filter.SearchTerm) || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filter.SearchTerm)));

            if (filter.DishTypes.Any())
            {
                //cria uma matrix com um elemento do primeiro item   
                var recipesWithDishTypes = query.Where(recipe => recipe.DishTypes.Any(dish => dish.Type == filter.DishTypes[0]));
                                //faz uma varredura para pegar um elemento da lista de DishTypes e adiciona na matrix
                // dai faz uma clausula UNION para unir as duas matrizes e retorna a lista final
                // pula o primeiro que ja montamos acima
                foreach (var dishType in filter.DishTypes.Skip(1))
                {
                    recipesWithDishTypes = recipesWithDishTypes.Union(query.Where(recipe => recipe.DishTypes.Any(dish => dish.Type == dishType)));
                    query = recipesWithDishTypes;
                }
            }

            return await query
            .Select(recipe => new RecipeSummaryDto(recipe.Id, recipe.Title))
            .ToListAsync();
        }
    }
}
