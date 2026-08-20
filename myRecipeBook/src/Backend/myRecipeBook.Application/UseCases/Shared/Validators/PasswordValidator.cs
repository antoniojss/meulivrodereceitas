using FluentValidation;
using myRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Shared.Validators
{
   public static class PasswordValidator
    {
        extension<TRequest> (IRuleBuilderInitial<TRequest, string> ruleBuilder)
        {
            internal   IRuleBuilderOptions<TRequest, string>  Password()
            {
                return ruleBuilder
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUERID)
                    .MinimumLength(6)
                    .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH);
            }

        }
    }
}
