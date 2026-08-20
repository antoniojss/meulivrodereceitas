using Azure.Core;
using CommonTestUtilities.Requests;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using Org.BouncyCastle.Asn1.Ocsp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.ChangePassword
{
    public class ChangePasswordTests : BaseIntegrationTest
    {
        private const string REQUEST_URI = "/users/password";
        private readonly UserIdentityManager _user1;
        public ChangePasswordTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _user1 = factory.User1;
        }

        [Fact]

        public async Task Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            request.CurrentPassword = _user1.GetPassword();

            var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInLineData))]
        public async Task Validate_ShouldThrowException_WhenNewPasswordIsEmpty(string culture)
        {
            var request = new RequestChangePasswordJson
            {
                CurrentPassword = _user1.GetPassword(),
                NewPassword = string.Empty
            };
            var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors =  responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectdMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_PASSWORD_REQUERID", new CultureInfo(culture));    

            errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count().ShouldBe(1);
                errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectdMessage));
            });
        }
    }

}
