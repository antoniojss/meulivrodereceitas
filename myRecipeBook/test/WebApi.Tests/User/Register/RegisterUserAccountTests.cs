using CommonTestUtilities.Requests;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Exception;
using myRecipeBook.Infrastructure.DataAccess;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.User.Register
{
    public class RegisterUserAccountTests : IClassFixture<MyRecipeBookApplicationFactory>
    {
        private const string REQUEST_URI = "/users";

        private readonly HttpClient _httpClient;
        private readonly MyRecipeBookDbContext _dbContext;


        public RegisterUserAccountTests(MyRecipeBookApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();

            var scope  = factory.Services.CreateScope();    

            _dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
        }


        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();

            var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
 
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();
            
            var userExists = await _dbContext.Users.AnyAsync(user => user.Active
            && user.Name.Equals(request.Name) 
            && user.Email.Equals(request.Email));

            userExists.ShouldBeTrue();  

        }

        [Theory]
        [ClassData(typeof(CultureInLineData))]
        public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
        {
            var request = RequestRegisterUserAccountJsonBuilder.Build();
            request.Name = culture;

            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            _httpClient.DefaultRequestHeaders.AcceptLanguage.TryParseAdd(culture);


            var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody); 

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));  

            errors.ShouldSatisfyAllConditions(errorsList=>
            { errorsList.Count().ShouldBe(1);
                errorsList.ShouldContain(error => error.GetString().IsNotEmpty() &&
                error.GetString()!.Equals(expectedErrorMessage));
            });

            var userExists = await _dbContext.Users.AnyAsync(user => user.Active 
            && user.Name.Equals(request.Name) 
            && user.Email.Equals(request.Email));

            userExists.ShouldBeFalse();


        }
    }
}
