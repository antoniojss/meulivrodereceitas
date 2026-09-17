using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.PasswordRecovery.RequestCode
{
    public  interface IRequestPasswordRecoveryCodeUseCase
    {
        Task Execute(RequestPasswordRecoveryJson request);
    }
}
