using Mapster;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.AI;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.GeneratedRecipeAI
{
    public class GenerateRecipeAIUseCase : IGenerateRecipeAIUseCase 
    {
        private readonly IGenerateRecipeAI _generateRecipeAI;
        public GenerateRecipeAIUseCase(IGenerateRecipeAI generateRecipeAI)
        {
            _generateRecipeAI = generateRecipeAI;       
        }

        public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request)
        {
            var generatedRecipeDto = await _generateRecipeAI.GenerateRecipe(request.Prompt);

            if (generatedRecipeDto is null)
                throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_UNABLE_TO_GENERATE_RECIPE]);  

            return generatedRecipeDto.Adapt<ResponseGeneratedRecipeJson>();
        }
    }
}
