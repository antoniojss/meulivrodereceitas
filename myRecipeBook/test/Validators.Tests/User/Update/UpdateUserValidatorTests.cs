using CommonTestUtilities.Requests;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Application.UseCases.User.Update;
using myRecipeBook.Exception;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Validators.Tests.User.Update
{
    public class UpdateUserValidatorTests
    {


        [Fact]
        public void Success()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("             ")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "use test nullable")]
        public void Validate_ShouldHaveError_WhenNameIsEmpty(string name)
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = name;
            
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

            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();

            request.Email = email;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUERID.ToString()));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenEmailIsInvalid()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "invalid-email";

        
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
