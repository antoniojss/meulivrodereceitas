using Moq;
using myRecipeBook.Domain.Dtos;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
    public  class IRecipeReadOnlyRepositoryBuilder
    {
        private readonly Mock<IRecipeReadOnlyRepository> _mock;
        public IRecipeReadOnlyRepositoryBuilder()
        {
         _mock = new Mock<IRecipeReadOnlyRepository>();   
        }

        public IRecipeReadOnlyRepositoryBuilder GetById(Recipe recipe)
        {
            _mock.Setup(repository => repository.GetById(recipe.Id, recipe.UserId)).ReturnsAsync(recipe);
           
            return this;
        }

        public IRecipeReadOnlyRepositoryBuilder GetRecentRecipies(User user, IList<Recipe> recipes)
        {
            var recipesDto = recipes.Select(recipe => new RecipeSummaryDto(recipe.Id, recipe.Title)).ToList();

            _mock.Setup(repository => repository.GetRecentRecipes(user.Id)).ReturnsAsync(recipesDto);

            return this;
        }

        public IRecipeReadOnlyRepository Build() => _mock.Object;
    }
}
