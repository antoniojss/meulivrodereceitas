using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using myRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UseCases.Tests.PasswordRecovery.ResetPassword
{
    public class ResetPasswordUseCaseTestscs
    {
        [Fact]
        public async Task Successs()
        {
            // Arrange
            var (user, _) = UserBuilder.Build();
            var verificationCode = VerificationCodeBuilder.Build(user);

            var request = RequestResetPasswordJsonBuilder.Build();
            request.Email = user.Email;
            request.Code = verificationCode.Code;

            var useCase = CreateUseCase(user, verificationCode);
            // Act & Assert
            await useCase.Execute(request).ShouldNotThrowAsync();
        }
 
        [Fact]
        public async Task Validate_WhenUserNotFound()
        {
            var (user, _) = UserBuilder.Build();
            var verificationCode = VerificationCodeBuilder.Build(user);

            var request = RequestResetPasswordJsonBuilder.Build();
            request.Code = verificationCode.Code;

            var useCase = CreateUseCase(user, verificationCode);

            var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_VERIFICATION_CODE_INVALID);
            });

        }

        [Fact]
        public async Task Validate_WhenVerificationCodeNotFound()
        {
            var (user, _) = UserBuilder.Build();
            var verificationCode = VerificationCodeBuilder.Build(user);

            var request = RequestResetPasswordJsonBuilder.Build();
            request.Email = user.Email;

            var useCase = CreateUseCase(user, verificationCode);

            var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_VERIFICATION_CODE_INVALID);
            });


        }

        [Fact]
        public async Task Validate_WhenVerificationCodeExpired()
        {
            var (user, _) = UserBuilder.Build();
            var verificationCode = VerificationCodeBuilder.Build(user);

            verificationCode.CreatedAt = DateTime.UtcNow.AddMinutes(-11);

            var request = RequestResetPasswordJsonBuilder.Build();
            request.Email = user.Email;
            request.Code = verificationCode.Code;

            var useCase = CreateUseCase(user, verificationCode);

            var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_VERIFICATION_CODE_INVALID);
            });
        }

        [Theory]
        [InlineData(1)]   
        [InlineData(2)]   
        [InlineData(3)]   
        [InlineData(4)]   
        [InlineData(5)]   
        public async Task Validate_WhenNewPasswordTooShort(int newPassword)
        {
            var (user, _) = UserBuilder.Build();
            var verificationCode = VerificationCodeBuilder.Build(user);
          
            var request = RequestResetPasswordJsonBuilder.Build(newPassword);
            request.Email = user.Email;
            request.Code = verificationCode.Code;

            var useCase = CreateUseCase(user, verificationCode);

            var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH);
            });

        }

        public static ResetPasswordUseCase CreateUseCase(myRecipeBook.Domain.Entities.User user, myRecipeBook.Domain.Entities.VerificationCode verificationCode  )
        {
            var userReadOnlyRepository = new IUserReadOnlyRepositoryBuilder().GetByEmail(user).Build();
            var verificationCodeReadOnlyRepository = new IVerificationCodeReadOnlyRepositoryBuilder().Get(verificationCode).Build();
            var verificationCodeWriteOnlyRepository = IVerificationCodeWriteOnlyRepositoryBuilder.Build();
            var userUpdateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();
            var passwordHasher = new IPasswordHasherBuilder().Build();
            return new ResetPasswordUseCase(
                userReadOnlyRepository,
                verificationCodeReadOnlyRepository,
                verificationCodeWriteOnlyRepository,
                userUpdateOnlyRepository,
                passwordHasher
            );  

        }
    }
}
