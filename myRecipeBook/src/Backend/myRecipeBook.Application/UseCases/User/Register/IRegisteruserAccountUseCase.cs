using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.Register
{
    public interface IRegisteruserAccountUseCase
    {
        Task <ResponseRegistredUserJson> Execute(RequestRegisterUserAccountJson request);
    }
}
