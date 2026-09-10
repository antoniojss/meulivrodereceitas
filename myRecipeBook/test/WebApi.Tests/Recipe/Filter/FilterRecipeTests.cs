using myRecipeBook.Communication.Enums;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Dtos;
using myRecipeBook.Domain.Extensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.Filter
{
    public class FilterRecipeTests : BaseIntegrationTest
    {
        private const string REQUEST_URI = "/recipes/filter";
        private readonly UserIdentityManager _user1;
        public FilterRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _user1 = factory.User1;
        }

        [Fact]
        public async Task Success()
        {
            var recipe = _user1.GetRecipe();


            var request = new RequestFilterRecipesJson
               {
                   SearchTerm = recipe.Title,
                   CookTime = (CookTime)recipe.CookTime,
                   DishTypes = recipe.DishTypes.Select(dishType => (DishType)dishType.Type).ToList()
               };
            
            var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var recipies = responseData.RootElement.GetProperty("recipes").EnumerateArray();

            recipies.ShouldSatisfyAllConditions(recipiesList =>
            {
                recipiesList.Count().ShouldBeGreaterThan(0);
                recipiesList.ShouldContain(recipeElement =>
                    recipeElement.GetProperty("id").GetGuid() == recipe.Id &&
                    recipeElement.GetProperty("title").GetString().IsNotEmpty() &&
                    recipeElement.GetProperty("title").GetString()!.Equals(recipe.Title));
            });
        }

    }
}

