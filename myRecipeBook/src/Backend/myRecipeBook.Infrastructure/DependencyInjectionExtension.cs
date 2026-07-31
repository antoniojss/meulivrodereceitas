using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.Infrastructure.DataAccess;
using myRecipeBook.Infrastructure.DataAccess.Repositories;
using myRecipeBook.Infrastructure.Security.PasswordHashing;
using myRecipeBook.Infrastructure.Security.Tokens.Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration.Internal;
using System.Reflection;
using System.Text;

namespace myRecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        extension(IServiceCollection services)
        {
            public void AddInfrastructure(IConfiguration configuration)
            {
                services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

                services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

                services.AddScoped<IUserReadOnlyRepository, UserRepository>();

                services.AddScoped<IUnitOfWork, UnitOfWork>();

                services.AddDbContext<MyRecipeBookDbContext>(config =>
                {
                    var connectionString = configuration.GetConnectionString("DbConnection");

                    config.UseMySQL(connectionString!);
                }
                );

                services.AddFluentMigratorCore()
                    .ConfigureRunner(config =>
                    {

                        config
                        .AddMySql5()
                        .WithGlobalConnectionString(_ =>
                        {
                            var connectionString = configuration.GetConnectionString("DbConnection")!;
                            return connectionString;
                        })
                        .ScanIn(Assembly.Load("myRecipeBook.Infrastructure"))
                        .For.All();
                    });


                services.AddScoped<IAccessTokenGenerator>(
                    provider =>
                    {
                        var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
                        var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

                        return new JwtTokenHandler(expirationTimeMinutes, signingKey);

                    });

            }
        }
    }
}
