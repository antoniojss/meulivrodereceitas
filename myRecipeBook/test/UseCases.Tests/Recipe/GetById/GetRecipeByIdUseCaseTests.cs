using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using Mapster;
using myRecipeBook.Application.Mappings;
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

namespace UseCases.Tests.Recipe.GetById
{
   public class GetRecipeByIdUseCaseTests
    {
        public GetRecipeByIdUseCaseTests()
        {
            MapsterConfiguration.Configure();
        }
                
        [Fact]
        public async Task Successs()
        {
            var(user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);
            
            var useCase = CreateUseCase(recipe, user);
            
            var result = await useCase.Execute(recipe.Id);
            
            result.ShouldNotBeNull();
            result.Id.ShouldBe(recipe.Id);
            result.Title.ShouldBe(recipe.Title);

            result.Instructions.Select(c=>c.Order).ShouldBeInOrder(SortDirection.Ascending);
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

        private static GetRecipeByIdUseCase CreateUseCase(myRecipeBook.Domain.Entities.Recipe recipe, myRecipeBook.Domain.Entities.User user)
        {

            var loggedUser = ILoggedUserBuilder.Build(user);
            var repository = new IRecipeReadOnlyRepositoryBuilder().GetById(recipe).Build();

            return new GetRecipeByIdUseCase(repository, loggedUser);

        }
    
    }
}
