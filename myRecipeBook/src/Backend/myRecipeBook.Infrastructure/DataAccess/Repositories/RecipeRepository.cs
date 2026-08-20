using Microsoft.VisualBasic;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.DataAccess.Repositories
{
    internal sealed class RecipeRepository : IRecipeWriteOnlyRepository
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
    }
}
