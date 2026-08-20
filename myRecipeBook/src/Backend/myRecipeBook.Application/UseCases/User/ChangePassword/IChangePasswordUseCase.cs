using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.ChangePassword
{
    public  interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePasswordJson request);
    }
}
