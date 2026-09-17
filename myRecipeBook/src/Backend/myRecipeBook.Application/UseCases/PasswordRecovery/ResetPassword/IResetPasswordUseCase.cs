using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword
{
    public  interface IResetPasswordUseCase
    {
        Task Execute(RequestResetPasswordJson request);
    }
}
