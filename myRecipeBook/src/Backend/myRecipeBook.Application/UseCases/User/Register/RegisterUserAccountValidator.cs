using FluentValidation;
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
                .NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED.ToString())
                .MaximumLength(100).WithMessage(ResourceMessagesException.VALIDATION_NAME_EXCEED_QTY_CHARACTERS.ToString());
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUERID.ToString())
                .EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL_FORMAT.ToString());
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUERID)
                .MinimumLength(6).WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MUST_QTY_CHARACTERS.ToString());
            When(user => user.Email.IsNotEmpty(), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL_FORMAT.ToString());

            });
        }
    }
}
