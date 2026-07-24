using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
namespace UseCases.Tests.User.Register
{
    public class RegisterUserAccountUseCaseTests
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = RequestRegisterUserAccountJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Name.ShouldBe(request.Name);
            result.Tokens.AccessToken.ShouldBeNullOrEmpty();
            result.Tokens.RefreshToken.ShouldBeNullOrEmpty();

        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            var exeption = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();
            exeption.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
            });

        }
        [Fact]
        public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();
            request.Email = "existing@example.com";

            var useCase = CreateUseCase(request.Email);

            var exeption = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();
            exeption.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS);
            });
        }

        private RegisterUserAccountUseCase CreateUseCase(string? emailThatAlreadyExists = null)
        {
            var unitofwork = UnitOfWorkBuilder.Build();

            var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();

            var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
            if (emailThatAlreadyExists.IsNotEmpty())
            {
                userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(emailThatAlreadyExists);
            }
            ;

            var passwordHasher = new IPasswordHasherBuilder().Build();

            return new RegisterUserAccountUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepositoryBuilder.Build(), unitofwork);
        }
    }
}
