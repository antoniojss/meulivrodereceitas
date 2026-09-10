using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using myRecipeBook.Application.UseCases.Recipe.Filters;
using myRecipeBook.Application.UseCases.Recipe.Recent;
using myRecipeBook.Communication.Requests;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Tests.Recipe.Filter
{
    public class FilterRecipesUseCaseTests
    {
        [Fact]
        public async Task Successs()
        {
            var (user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, [recipe]);

            var result = await useCase.Execute(request:null);

            result.ShouldNotBeNull();
            result.Recipes.ShouldNotBeNull();
            result.Recipes.Count.ShouldBe(1);
            result.Recipes.ShouldContain(recipeSummary =>
                                         recipeSummary.Id == recipe.Id &&
                                         recipeSummary.Title.Equals(recipe.Title));
        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenRecipeNotFound()
        {
            var (user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, [recipe]);

            var request = new RequestFilterRecipesJson();

            var result = await useCase.Execute(request);


            result.ShouldNotBeNull();
            result.Recipes.ShouldNotBeNull();
            result.Recipes.Count.ShouldBe(1);
            result.Recipes.ShouldContain(recipeSummary =>
                                         recipeSummary.Id == recipe.Id &&
                                         recipeSummary.Title.Equals(recipe.Title));
        }

        private static FilterRecipesUseCase CreateUseCase(myRecipeBook.Domain.Entities.User user, IList<myRecipeBook.Domain.Entities.Recipe> recipes)
        {

            var loggedUser = ILoggedUserBuilder.Build(user);
            var repository = new IRecipeReadOnlyRepositoryBuilder().FilterRecipies(user, recipes).Build();

            return new FilterRecipesUseCase(repository, loggedUser);

        }

    }
}
