using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Testcontainers.MySql;

namespace WebApi.Tests
{
    public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
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
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _mySqlContainer.StopAsync();
        }

    }

}