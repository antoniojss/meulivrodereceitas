using FluentValidation;
using FluentValidation.Validators;
using myRecipeBook.Application.UseCases.Shared.Validators;
using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangePasswordValidator()
        {
            RuleFor(request => request.NewPassword).Password();
        }
    }
}
