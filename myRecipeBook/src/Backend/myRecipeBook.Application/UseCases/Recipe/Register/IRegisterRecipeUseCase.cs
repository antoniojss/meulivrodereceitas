using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.Register
{
  public interface IRegisterRecipeUseCase
    {
        Task<ResponseRegistredRecipeJson> Execute(RequestRecipeJson request);
    }
}
