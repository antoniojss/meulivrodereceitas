using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using myRecipeBook.Application.UseCases.User.ChangePassword;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Tests.User.ChangePassword
{
    public class ChangePasswordUseCaseTests
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.CurrentPassword = password;

            var useCase = CreateUseCase(user, password);

            await useCase.Execute(request).ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenNewPasswordIsEmpty()
        {
            (var user, var password) = UserBuilder.Build();

            var request = new RequestChangePasswordJson
            {
                CurrentPassword = password,
                NewPassword = string.Empty

            };
            var useCase = CreateUseCase(user, password);

            var exeption = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exeption.ShouldSatisfyAllConditions(exeption =>
            {
                exeption.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
                exeption.GetErrorMessages().ShouldSatisfyAllConditions(messages =>
                {
                    messages.Count.ShouldBe(1);
                    messages.ShouldContain(ResourceMessagesException.VALIDATION_PASSWORD_REQUERID);
                });
            });
        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenCurrentPasswordDoesNotMatch()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
         
            var useCase = CreateUseCase(user, "invalidPassword");

            var exeption = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exeption.ShouldSatisfyAllConditions(exeption =>
            {
                exeption.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
                exeption.GetErrorMessages().ShouldSatisfyAllConditions(messages =>
                {
                    messages.Count.ShouldBe(1);
                    messages.ShouldContain(ResourceMessagesException.VALIDATION_CURRENT_PASSWORD);
                });
            });
        }

        private ChangePasswordUseCase CreateUseCase(myRecipeBook.Domain.Entities.User user, string password)
        {
            var userUpdateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();
            var loggedUser = ILoggedUserBuilder.Build(user);
            var passwordHasher = new IPasswordHasherBuilder().VerifyPassword(password).Build();
            return new ChangePasswordUseCase(loggedUser, passwordHasher, userUpdateOnlyRepository);
        }
    }
}
