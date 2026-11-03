using Bogus;
using Bogus.DataSets;
using CommonTestUtilities.AI;
using CommonTestUtilities.Requests;
using Moq;
using myRecipeBook.Application.Mappings;
using myRecipeBook.Application.UseCases.Recipe.GeneratedRecipeAI;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.AI;
using myRecipeBook.Domain.Dtos;
using myRecipeBook.Domain.Enums;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Shouldly;
using System.Collections.Generic;
using System.Net;

namespace UseCases.Tests.Recipe.GeneratedRecipeAI
{
    public class GenerateRecipeAIUseCaseTests
    {
        static GenerateRecipeAIUseCaseTests()
        {
            MapsterConfiguration.Configure();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestGenerateRecipeJsonBuilder.Build();

            var useCase = CreateUseCase();  

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Title.ShouldNotBeNullOrEmpty();
            result.Description.ShouldNotBeNullOrEmpty();
            result.Difficulty.ShouldNotBeNullOrEmpty();
            result.Servings.ShouldNotBeNull();
            result.Instructions.ShouldNotBeEmpty();
            result.Ingredients.ShouldNotBeEmpty(); 
            result.Image.ShouldNotBeEmpty();  
        }

        [Fact]
        public async Task Validate_ShouldThrowException_WhenRecipeGenerationFails()
        {
            var request = RequestGenerateRecipeJsonBuilder.Build();
            request.Prompt = string.Empty;
            
            var useCase = CreateUseCase();

            var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();
                        
            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count.ShouldBe(1);
                errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_UNABLE_TO_GENERATE_RECIPE);
            });
        }

        private static GenerateRecipeAIUseCase CreateUseCase()
        {
            var generateRecipeAi = IGenerateRecipeAIBuilder.Build();
            return new GenerateRecipeAIUseCase(generateRecipeAi);
        }
    }
}
