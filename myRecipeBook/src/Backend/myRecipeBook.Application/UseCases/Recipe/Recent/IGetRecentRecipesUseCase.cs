using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.Recent
{
   public  interface IGetRecentRecipesUseCase
    {
        Task<ResponseRecipesJson> Execute();
    }
}
