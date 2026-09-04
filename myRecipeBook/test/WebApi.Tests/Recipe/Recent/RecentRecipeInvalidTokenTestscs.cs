using myRecipeBook.Communication.Requests;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebApi.Tests.Recipe.Recent
{
  
    public class RecentRecipeInvalidTokenTests   : BaseIntegrationTest
    {
        private const string REQUEST_URI = "/recipes/recent";

        private readonly string _tokenUserNotExistDatabase;

        public RecentRecipeInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenAccessTokenIsInvalid()
        {
            var request = new RequestRecipeJson();

            var response = await Get(REQUEST_URI, accessToken: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenAccessTokenIsMissing()
        {
            var request = new RequestRecipeJson();

            var response = await Get(REQUEST_URI, accessToken: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenWithUserNotFound()
        {
            var request = new RequestRecipeJson();

            var response = await Get(REQUEST_URI,accessToken: _tokenUserNotExistDatabase);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
