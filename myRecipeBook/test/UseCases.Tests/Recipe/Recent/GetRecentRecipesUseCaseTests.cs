using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using myRecipeBook.Application.Mappings;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Application.UseCases.Recipe.Recent;
using myRecipeBook.Application.UseCases.Recipe.Register;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UseCases.Tests.Recipe.Recent
{
    public class GetRecentRecipesUseCaseTests
    {
        [Fact]
        public async Task Successs()
        {
            var (user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user,[recipe]);

            var result = await useCase.Execute();

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

            var useCase = CreateUseCase(user, []);

            var result = await useCase.Execute();

            result.ShouldNotBeNull();
            result.Recipes.ShouldNotBeNull();
            result.Recipes.ShouldBeEmpty();
        }

        private static GetRecentRecipesUseCase CreateUseCase(myRecipeBook.Domain.Entities.User user, IList<myRecipeBook.Domain.Entities.Recipe> recipes)
        {

            var loggedUser = ILoggedUserBuilder.Build(user);
            var repository = new IRecipeReadOnlyRepositoryBuilder().GetRecentRecipies(user, recipes).Build();

            return new GetRecentRecipesUseCase(repository, loggedUser);

        }

    }
}
