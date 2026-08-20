using FluentValidation;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.Update
{
    public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
    {
        public UpdateUserValidator()
        {
            RuleFor(request =>request.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED.ToString());
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUERID.ToString());
            When(request => request.Email.IsNotEmpty(), () =>
            {
                RuleFor(request =>request.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL_FORMAT.ToString());

            });
        }
    }
}
