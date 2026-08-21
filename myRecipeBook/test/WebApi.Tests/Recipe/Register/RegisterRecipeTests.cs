using CommonTestUtilities.Requests;
using Mapster;
using Microsoft.EntityFrameworkCore;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using myRecipeBook.Infrastructure.DataAccess;
using Shouldly;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;
using Xunit;

namespace WebApi.Tests.Recipe.Register
{
    public class RegisterRecipeTests : BaseIntegrationTest
    {
        private const string REQUEST_URI = "/recipes";
        private readonly UserIdentityManager _user1;
        public RegisterRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _user1 = factory.User1;
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRecipeJsonBuilder.Build();

            //var accessToken = await GetAuthenticatedUserToken();

            var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);

            var recipeId = responseData.RootElement.GetProperty("id").GetGuid();

            // Verificar se foi registrado no banco de dados
            var recipeExists = await DbContext.Recipes.AnyAsync(recipe =>
            recipe.Id == recipeId &&
            recipe.Active &&
            recipe.Title.Equals(request.Title) &&
            recipe.UserId == _user1.GetId());
            
            recipeExists.ShouldBeTrue();
        }

        [Theory]
        [ClassData(typeof(CultureInLineData))]
        public async Task Validate_ShouldBeAnErrorResponse_WhenTitleIsEmpty(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;
         
            var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString(
                "VALIDATION_RECIPE_TITLE_REQUIRED", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(errorsList =>
            {
                errorsList.Count().ShouldBeGreaterThan(0);
                errorsList.ShouldContain(error => error.GetString().IsNotEmpty() &&
                    error.GetString()!.Equals(expectedErrorMessage));
            });

            // Verificar que nenhuma receita foi criada
            var recipeExists = await DbContext.Recipes.AnyAsync(recipe =>
            recipe.Active &&
            recipe.Title.Equals(request.Title) &&
            recipe.UserId == _user1.GetId());

            recipeExists.ShouldBeFalse();
        }
    }
}
