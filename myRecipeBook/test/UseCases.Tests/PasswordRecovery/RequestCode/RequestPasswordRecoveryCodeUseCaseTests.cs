using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using myRecipeBook.Application.UseCases.PasswordRecovery.RequestCode;
using myRecipeBook.Application.UseCases.Recipe.Filters;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Tests.PasswordRecovery.RequestCode
{
    public class RequestPasswordRecoveryCodeUseCaseTests
    {
        [Fact]
        public async Task Successs()
        {
            var (user, _) = UserBuilder.Build();
             
            var request = new RequestPasswordRecoveryJson
            {
                Email = user.Email
            };      

            var useCase = CreateUseCase(user);

            await useCase.Execute(request).ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Validate_WhenUserNotFound()
        {
           var request = RequestPasswordRecoveryJsonBuilder.Build();

            var useCase = CreateUseCase();

            await useCase.Execute(request).ShouldNotThrowAsync();
        }

        private static RequestPasswordRecoveryCodeUseCase CreateUseCase(myRecipeBook.Domain.Entities.User? user =null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();  
            var verificationCodeWriteOnlyRepository = IVerificationCodeWriteOnlyRepositoryBuilder.Build();
            var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
            if (user is not null)
                userReadOnlyRepositoryBuilder.GetByEmail(user);

            return new RequestPasswordRecoveryCodeUseCase(userReadOnlyRepositoryBuilder.Build(),verificationCodeWriteOnlyRepository, unitOfWork);

        }

    }
}
