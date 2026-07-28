using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Login.WithEmailAndPassword
{
    public  interface ILoginWithEmailAndPasswordUseCase
    {
        Task<ResponseRegistredUserJson> Execute(RequestLoginJson request);
    }
}
