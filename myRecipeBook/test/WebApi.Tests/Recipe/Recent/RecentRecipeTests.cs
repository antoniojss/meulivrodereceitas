using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.Recent
{   
    public class RecentRecipeTests : BaseIntegrationTest
    {
        private const string REQUEST_URI = "/recipes/recent";
        private readonly UserIdentityManager _user1;
        public RecentRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _user1 = factory.User1;
        }

        [Fact]
        public async Task Success()
        {
            var recipe = _user1.GetRecipe();

            var response = await Get(REQUEST_URI, accessToken: _user1.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var recipies = responseData.RootElement.GetProperty("recipes").EnumerateArray();

            recipies.ShouldSatisfyAllConditions(recipiesList =>
            {
                recipiesList.Count().ShouldBeGreaterThan(0);
                recipiesList.ShouldContain(recipeElement =>
                    recipeElement.GetProperty("id").GetGuid() == recipe.Id &&
                    recipeElement.GetProperty("title").GetString() == recipe.Title &&
                    recipeElement.GetProperty("title").GetString()!.Equals(recipe.Title));    
            });
        }
   
    }
}
