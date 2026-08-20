using Microsoft.Identity.Client;
using myRecipeBook.Communication.Requests;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebApi.Tests.User.Profile
{
    public class GetUserProfileInvalidTokenTests : BaseIntegrationTest
    {

        private const string REQUEST_URI = "/users";        

        private readonly string  _tokenUserNotExistDatabase;

        public GetUserProfileInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
        }

        [Fact]
        public async Task Error_Token_Invaid()
        {
            var response = await Get(REQUEST_URI, accessToken: "tokenInvalid");
    
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var response = await Get(REQUEST_URI, accessToken: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Error_Token_With_User_NotFoud()
        {
            var response = await Get(REQUEST_URI, accessToken: _tokenUserNotExistDatabase);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        }
    }
}
