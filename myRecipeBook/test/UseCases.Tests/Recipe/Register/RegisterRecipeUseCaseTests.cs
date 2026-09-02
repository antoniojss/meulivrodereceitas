using CommonTestUtilities.Requests;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Identity;
using myRecipeBook.Application.UseCases.Recipe.Register;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using myRecipeBook.Application.Mappings;
using CommonTestUtilities.Entities;
using System.Net;

namespace UseCases.Tests.Recipe.Register
{
    public class RegisterRecipeUseCaseTests
    {
        public RegisterRecipeUseCaseTests()
        {
            MapsterConfiguration.Configure();
        }


        [Fact]
        public async Task Success()
        {
            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);
          
            result.ShouldNotBeNull();
            result.Title.ShouldBe(result.Title);
        }


        [Fact]
        public async Task Validate_ShouldThrowException_WhenTitleIsEmpty()
        {
            var request = RequestRecipeJsonBuilder.Build();
           
            request.Title = string.Empty;

            var useCase = CreateUseCase();

            var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED);
            });
        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenIngredientsIsEmpty()
        {
            // Arrange
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients = new System.Collections.Generic.List<string>();

            var useCase = CreateUseCase();

            // Act & Assert
            var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_AT_LAST_ONE_INGREDIENT);
            });
        }

        private RegisterRecipeUseCase CreateUseCase()
        {
            var (user, _) = UserBuilder.Build();

            var loggedUser = ILoggedUserBuilder.Build(user);

            var recipeWriteOnlyRepository = new IRecipeWriteOnlyRepositoryBuilder().Build();

            var unitOfWork = UnitOfWorkBuilder.Build();
            
            return new RegisterRecipeUseCase(recipeWriteOnlyRepository, loggedUser, unitOfWork);
        }
    }
}
