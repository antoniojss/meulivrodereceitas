using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using myRecipeBook.Communication.Requests;
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

namespace WebApi.Tests.User.Update
{
    public class UpdateUserTests : BaseIntegrationTest
    {

        private const string REQUEST_URI = "/users/profile";

        private readonly UserIdentityManager _user1;

        public UpdateUserTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _user1 = factory.User1;
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            var userExists = await DbContext
                .Users
                .AnyAsync(user => user.Active && user.Id == _user1.GetId()
                                              && user.Name.Equals(request.Name)
                                              && user.Email.Equals(request.Email));
            userExists.ShouldBeTrue();

        }

        [Theory]
        [ClassData(typeof(CultureInLineData))]
        public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectdMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count().ShouldBe(1);
                errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectdMessage));
            });
        }
    }
}