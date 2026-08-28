using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.DeleteById
{
   public  interface IDeleteRecipeByIdUseCase
    {
        Task Execute(Guid recipeId);
    }
}
