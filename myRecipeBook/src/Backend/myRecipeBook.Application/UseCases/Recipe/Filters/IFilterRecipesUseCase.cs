using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.Filters
{
   public interface IFilterRecipesUseCase
    {
        Task<ResponseRecipesJson> Execute(RequestFilterRecipesJson? request);
    }
}
