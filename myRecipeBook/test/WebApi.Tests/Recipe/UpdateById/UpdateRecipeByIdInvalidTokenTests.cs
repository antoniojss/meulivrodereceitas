using myRecipeBook.Communication.Requests;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebApi.Tests.Recipe.UpdateById
{
    public class UpdateRecipeByIdInvalidTokenTests : BaseIntegrationTest
    {
        private const string REQUEST_URI = "/recipes";

        private readonly string _tokenUserNotExistDatabase;

        public UpdateRecipeByIdInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenAccessTokenIsInvalid()
        {
            var request = new RequestRecipeJson();

            var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenAccessTokenIsMissing()
        {

            var request = new RequestRecipeJson();

            var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenUserFromAccessTokenDoesNotExist()
        {

            var request = new RequestRecipeJson();

            var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: _tokenUserNotExistDatabase);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

    }
}
