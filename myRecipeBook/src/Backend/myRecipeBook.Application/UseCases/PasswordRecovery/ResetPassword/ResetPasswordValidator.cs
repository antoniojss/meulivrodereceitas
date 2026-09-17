using FluentValidation;
using myRecipeBook.Application.UseCases.Recipe;
using myRecipeBook.Application.UseCases.Shared.Validators;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<RequestResetPasswordJson> 
    {
        public ResetPasswordValidator()
        {
            RuleFor(request => request.NewPassword).Password();
        }
    }
}
