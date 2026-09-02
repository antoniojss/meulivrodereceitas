using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using myRecipeBook.Application.Mappings;
using myRecipeBook.Application.UseCases.Recipe.DeleteById;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UseCases.Tests.Recipe.DeleteById
{
    public class DeleteRecipeByIdUseCaseTests
    {
        [Fact]
        public async Task Successs()
        {
            var (user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(recipe, user);

            await useCase.Execute(recipe.Id).ShouldNotThrowAsync();

        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenRecipeNotFound()
        {
            var (user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(recipe, user);

            var exception = await useCase.Execute(Guid.CreateVersion7()).ShouldThrowAsync<NotFoundException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_INVALID_RECIPE_NOT_FOUND);
            });
        }

        private static DeleteRecipeByIdUseCase CreateUseCase(myRecipeBook.Domain.Entities.Recipe recipe, myRecipeBook.Domain.Entities.User user)
        {

            var loggedUser = ILoggedUserBuilder.Build(user);
            var repository = new IRecipeWriteOnlyRepositoryBuilder().DeleteById(recipe).Build();

            return new DeleteRecipeByIdUseCase(repository, loggedUser);

        }

    }
}