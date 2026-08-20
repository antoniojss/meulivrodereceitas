using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Testcontainers.MySql;
using WebApi.Tests.Resources;

namespace WebApi.Tests
{
    public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public UserIdentityManager User1 { get; private set;  }  =default!; 

        public string TOKEN_USER_NOT_FOUND_IN_DATABASE {get; private set; } = string.Empty;    

        private readonly MySqlContainer _mySqlContainer;
        internal object ServicesProvider;

        public MyRecipeBookApplicationFactory()
        {
            _mySqlContainer = new MySqlBuilder("mysql:8.0")
            .WithDatabase("meulivrodereceitas")
            .Build();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Tests")
                .ConfigureAppConfiguration((_, configuration) =>
                {
                    var parametres = new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DbConnection"] = _mySqlContainer.GetConnectionString()
                    };

                    configuration.AddInMemoryCollection(parametres);

                });
        }
        public async Task InitializeAsync()
        {
            await _mySqlContainer.StartAsync();

            await using var scope = Services.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

            var passwordHashr = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

            // Arrange
            var (user, password) = UserBuilder.Build();

            user.Password = passwordHashr.HashPassword(password);

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var user1AcessToken = accessTokenGenerator.Generate(user);

            User1 = new UserIdentityManager(user, password, user1AcessToken);

            TOKEN_USER_NOT_FOUND_IN_DATABASE = accessTokenGenerator.Generate(new myRecipeBook.Domain.Entities.User());
        }
        
        Task IAsyncLifetime.DisposeAsync()
        {
            return _mySqlContainer.StopAsync();
        }



    }

}