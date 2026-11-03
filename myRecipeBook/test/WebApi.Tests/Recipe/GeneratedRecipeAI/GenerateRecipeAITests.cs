using Bogus;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.AI;
using myRecipeBook.Domain.Dtos;
using myRecipeBook.Domain.Enums;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.GeneratedRecipeAI
{
    public class GenerateRecipeAITests : BaseIntegrationTest
    {
        private const string REQUEST_URI = "/recipes/generate";
        private readonly UserIdentityManager _user1;

        public GenerateRecipeAITests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _user1 = factory.User1;
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestGenerateRecipeJsonBuilder.Build();

            var response = await Post(REQUEST_URI, request, _user1.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("title").GetString().ShouldBe("Pão de queijo");
            responseData.RootElement.GetProperty("description").GetString().ShouldBe("Receita simples e saborosa.");
            responseData.RootElement.GetProperty("difficulty").GetString().ShouldBe("Easy");
            responseData.RootElement.GetProperty("servings").GetString().ShouldBe("2");
            responseData.RootElement.GetProperty("cookTime").GetString().ShouldBe("UpTo30Minutes");

            var ingredients = responseData.RootElement.GetProperty("ingredients");
            ingredients.GetArrayLength().ShouldBeGreaterThan(0);

            var instructions = responseData.RootElement.GetProperty("instructions");
            instructions.GetArrayLength().ShouldBeGreaterThan(0);
        }
       
        [Theory]
        [ClassData(typeof(CultureInLineData))]
        public async Task Validate_ShouldBeAnErrorResponse_WhenGenerationFails(string culture)
        {
            var request = RequestGenerateRecipeJsonBuilder.Build();
            request.Prompt = string.Empty;  

           var response = await Post(REQUEST_URI, request, _user1.GetAccessToken(), culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString(
                "VALIDATION_UNABLE_TO_GENERATE_RECIPE", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(errorMessages =>
            {
                errorMessages.Count().ShouldBeGreaterThan(0);
                errorMessages.ShouldContain(error => error.GetString().IsNotEmpty() &&
                    error.GetString()!.Equals(expectedErrorMessage));
            });
        }
    }
}

