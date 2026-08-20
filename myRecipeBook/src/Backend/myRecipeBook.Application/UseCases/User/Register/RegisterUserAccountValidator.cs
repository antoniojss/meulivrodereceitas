using FluentValidation;
using myRecipeBook.Application.UseCases.Shared.Validators;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUserAccountJson>
    {
        public RegisterUserAccountValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED.ToString());
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUERID.ToString());
            RuleFor(user => user.Password).Password();
            When(user => user.Email.IsNotEmpty(), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL_FORMAT.ToString());

            });
        }
    }
}
