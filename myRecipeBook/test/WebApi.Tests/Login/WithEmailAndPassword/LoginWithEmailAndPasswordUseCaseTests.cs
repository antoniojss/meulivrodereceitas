using CommonTestUtilities.Requests;
using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using myRecipeBook.Infrastructure.DataAccess;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Login.WithEmailAndPassword
{
    public class LoginWithEmailAndPasswordTests
        : IClassFixture<MyRecipeBookApplicationFactory>
    {
        private const string REQUEST_URI = "/authentication";

        private readonly UserIdentityManager _User1;

        private readonly HttpClient _httpClient;
     
        //private readonly MyRecipeBookDbContext _dbContext;


        public LoginWithEmailAndPasswordTests(MyRecipeBookApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
            _User1 = factory._User1;

            //var scope = factory.Services.CreateScope();

            //_dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
        }


        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _User1.GetEmail(),
                Password = _User1.GetPassword(),
            };

            var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().ShouldBe(_User1.GetName());
            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();

            //var userExists = await _dbContext.Users.AnyAsync(user => user.Active
            //&& user.Name.Equals(request.Name)
            //&& user.Email.Equals(request.Email));

            //userExists.ShouldBeTrue();

        }

        [Theory]
        [ClassData(typeof(CultureInLineData))]
        public async Task Validate_ShouldBeAnErrorResponse_WhenUserDontExist(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();
           
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);


            var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_LOGIN_INVALID", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(errorsList =>
            {
                errorsList.Count().ShouldBe(1);
                errorsList.ShouldContain(error => error.GetString().IsNotEmpty() &&
                error.GetString()!.Equals(expectedErrorMessage));
            });
        }
    }
}
