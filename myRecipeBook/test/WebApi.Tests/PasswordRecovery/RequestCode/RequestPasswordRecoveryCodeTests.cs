using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Enums;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApi.Tests.Resources;

namespace WebApi.Tests.PasswordRecovery.RequestCode
{
    public class RequestPasswordRecoveryCodeTests : BaseIntegrationTest
    {

        private const string REQUEST_URI = "/authentication/password-recovery";
        private readonly UserIdentityManager _user1;
        public RequestPasswordRecoveryCodeTests(MyRecipeBookApplicationFactory factory) : base(factory)
        {
            _user1 = factory.User1; 
        }

        [Fact]
        public async Task Success()
        {
            var recipe = _user1.GetRecipe();


            var request = new RequestPasswordRecoveryJson
            {
                Email = _user1.GetEmail()
            };

            var response = await Post(REQUEST_URI, request);

            response.StatusCode.ShouldBe(HttpStatusCode.Accepted);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.ShouldBeEmpty();

            var codeGenerated = await DbContext.VerificationCodes.AnyAsync(code => 
            code.UserId == _user1.GetId() &&
            code.Type == VerificationCodeType.PasswordRecovery);
            codeGenerated.ShouldBeTrue();
        }

        [Fact]
        public async Task Validate_WhenUserNotFound()
        {

            var request = RequestPasswordRecoveryJsonBuilder.Build();

            var codesBefore = await DbContext.VerificationCodes.AsNoTracking().CountAsync();

            var response = await Post(REQUEST_URI, request);
            
            response.StatusCode.ShouldBe(HttpStatusCode.Accepted);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.ShouldBeEmpty();

            var codesAfter = await DbContext.VerificationCodes.AsNoTracking().CountAsync();

            codesAfter.ShouldBe(codesBefore);
        }

    }
}
