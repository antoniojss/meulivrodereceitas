using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using myRecipeBook.Application.Mappings;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Application.UseCases.Recipe.Register;
using myRecipeBook.Application.UseCases.Recipe.UpdateById;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UseCases.Tests.Recipe.UpdateById
{
    public class UpdateRecipeByIdUseCaseTests
    {
        static UpdateRecipeByIdUseCaseTests()
        {
            MapsterConfiguration.Configure();
        }

        [Fact]
        public async Task Successs()
        {
            var (user, _) = UserBuilder.Build();
     
            var recipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build(); 

            var useCase = CreateUseCase(recipe, user);

            await useCase.Execute(recipe.Id, request).ShouldNotThrowAsync();

        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenRecipeNotFound()
        {
            var (user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(recipe, user);


            var exception = await useCase.Execute(Guid.CreateVersion7(),request).ShouldThrowAsync<NotFoundException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_INVALID_RECIPE_NOT_FOUND);
            });
        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenTitleIsEmpty()
        {
            var (user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build();
            
            request.Title= string.Empty;    

            var useCase = CreateUseCase(recipe, user);


            var exception = await useCase.Execute(recipe.Id, request).ShouldThrowAsync<ErrorOnValidationException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED);
            });
        }

        private static UpdateRecipeByIdUseCase CreateUseCase(myRecipeBook.Domain.Entities.Recipe recipe, myRecipeBook.Domain.Entities.User user)
        {

            var loggedUser = ILoggedUserBuilder.Build(user);
            var repository = new IRecipeUpdateOnlyRepositoryBuilder().GetById(recipe).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            return new UpdateRecipeByIdUseCase(repository, loggedUser,  unitOfWork);

        }

    }
}
