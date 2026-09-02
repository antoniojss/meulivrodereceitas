using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.UpdateById
{
    public interface IUpdateRecipeByIdUseCase
    {
        Task Execute(Guid recipeId, RequestRecipeJson request);
    }
}
