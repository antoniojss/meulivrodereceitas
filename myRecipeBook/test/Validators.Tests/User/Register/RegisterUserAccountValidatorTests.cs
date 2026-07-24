using CommonTestUtilities.Requests;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Exception;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Tests.User.Register
{
    public class RegisterUserAccountValidatorTests
    {

        [Fact]
        public void Success()
        {
            ///AAA

            //arrange
            var request = RequestRegisterUserAccountJsonBuilder.Build();

            var validator = new RegisterUserAccountValidator();

            //act
            var result = validator.Validate(request);

            //assert
            //Assert.True(result.IsValid); usando a bibliotecas Shouldly

            result.IsValid.ShouldBeTrue();
        }

        //[Fact]
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("             ")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "use test nullable")]
        public void Validate_ShouldHaveError_WhenNameIsEmpty(string name)
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();
            request.Name = name;

            var validator = new RegisterUserAccountValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED.ToString()));
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("             ")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "use test nullable")]
        public void Validate_ShouldHaveError_WhenEmailIsEmpty(string email)
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();
            request.Email = email;

            var validator = new RegisterUserAccountValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUERID.ToString()));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenPasswordIsEmpty()
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();
            request.Password = string.Empty;

            var validator = new RegisterUserAccountValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUERID.ToString()));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenEmailIsInvalid()
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();
            request.Email = "invalid-email";

            var validator = new RegisterUserAccountValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.INVALID_EMAIL_FORMAT.ToString()));
            });
        }
    }
}
