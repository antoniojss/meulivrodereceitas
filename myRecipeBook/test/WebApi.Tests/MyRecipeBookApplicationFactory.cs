using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Domain.Security.PasswordHashing;
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
        public UserIdentityManager _User1 { get; private set;  } 

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

            // Arrange
            var (user, password) = UserBuilder.Build();

            user.Password = passwordHashr.HashPassword(password);

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            _User1 = new UserIdentityManager(user, password);
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _mySqlContainer.StopAsync();
        }



    }

}