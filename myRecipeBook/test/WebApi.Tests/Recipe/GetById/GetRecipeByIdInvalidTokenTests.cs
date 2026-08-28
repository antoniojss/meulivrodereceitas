using DotNet.Testcontainers.Builders;
using myRecipeBook.Communication.Requests;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebApi.Tests.Recipe.GetById
{
    public class GetRecipeByIdInvalidTokenTests :BaseIntegrationTest
    {
        private const string REQUEST_URI = "/recipes";

        private readonly string _tokenUserNotExistDatabase;

        public GetRecipeByIdInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenAccessTokenIsInvalid()
        {
            var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenAccessTokenIsMissing()
        {
          
            var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Validate_ShouldBeAnErrorRespose_WhenUserFromAccessTokenDoesNotExist()
        {
            
            var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _tokenUserNotExistDatabase);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}

