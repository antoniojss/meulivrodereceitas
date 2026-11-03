using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.GeneratedRecipeAI
{
    public interface IGenerateRecipeAIUseCase
    {
        Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
    }
}
