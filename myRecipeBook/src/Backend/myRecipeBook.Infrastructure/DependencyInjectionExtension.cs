using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Domain.AI;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Domain.Repositories.RefreshToken;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Repositories.VerificationCode;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.Domain.Storage;
using myRecipeBook.Infrastructure.AI;
using myRecipeBook.Infrastructure.DataAccess;
using myRecipeBook.Infrastructure.DataAccess.Repositories;
using myRecipeBook.Infrastructure.Identity;
using myRecipeBook.Infrastructure.Security.PasswordHashing;
using myRecipeBook.Infrastructure.Security.Tokens.Access;
using myRecipeBook.Infrastructure.Security.Tokens.Refresh;
using myRecipeBook.Infrastructure.Storage;
using OllamaSharp;
using System.Reflection;

namespace myRecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        extension(IServiceCollection services)
        {
            public void AddInfrastructure(IConfiguration configuration)
            {
                services.AddRepositories();

                services.AddAI(configuration);

                services.AddSecurity(configuration);

                services.AddScoped<IStorageService>(config =>
                 {
                     var blobStorageConnectionString = configuration.GetConnectionString("BlobStorage")!;
                     var blobServiceClient = new BlobServiceClient(blobStorageConnectionString);

                     return new AzureStorageService(blobServiceClient);
                 });

                services.AddScoped<ILoggedUser, LoggedUser>();

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
            }

            private void AddRepositories()
            {
                //UserRepository
                services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

                services.AddScoped<IUserReadOnlyRepository, UserRepository>();

                services.AddScoped<IUnitOfWork, UnitOfWork>();

                services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

                // RecipeRepository
                services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
                services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
                services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();

                // VerificationCodeRepository   
                services.AddScoped<IVerificationCodeWriteOnlyRepository, VerificationCodeRepository>();
                services.AddScoped<IVerificationCodeReadOnlyRepository, VerificationCodeRepository>();


                services.AddScoped<IRefreshTokenWriteOnlyRepository, RefreshTokenRepository>();

                services.AddScoped<IRefreshTokenReadOnlyRepository, RefreshTokenRepository>();
            }


            private void AddSecurity(IConfiguration configuration)
            {
                services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

                services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

                services.AddScoped<IAccessTokenGenerator>(
                provider =>
                {
                    var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
                    var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

                    return new JwtTokenHandler(expirationTimeMinutes, signingKey);

                });
            }

            private  void AddAI(IConfiguration configuration)
            {
                // Registrar o IOllamaApiClient (configurando o HttpClient com o Timeout do appsettings)
                services.AddSingleton<IOllamaApiClient>(provider =>
                {
                    var endpoint = configuration.GetValue<string>("Settings:Ollama:Endpoint")!;
                    var timeoutSeconds = configuration.GetValue<int>("Settings:Ollama:TimeoutSeconds");

                    // Criamos o HttpClient manualmente para aplicar o Timeout configurado
                    var httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(endpoint),
                        Timeout = TimeSpan.FromSeconds(timeoutSeconds > 0 ? timeoutSeconds : 300) // Fallback para 5 min se não configurado
                    };

                    // Passamos o httpClient e o modelo padrão (opcional no construtor do client, mas boa prática)
                    var modelName = configuration.GetValue<string>("Settings:Ollama:ModelName")!;
                    return new OllamaApiClient(httpClient, modelName);
                });

                // Registrar o OllamaSharp.Chat (serviço de chat)
                services.AddSingleton<Chat>(provider =>
                {
                    var apiClient = provider.GetRequiredService<IOllamaApiClient>();
                    var modelName = configuration.GetValue<string>("Settings:Ollama:ModelName")!;

                    // Criar um Chat com o modelo configurado
                    var chat = new Chat(apiClient, modelName)
                    {
                        Model = modelName,  // Garantir que o modelo está definido
                    };

                    return chat;
                });

                // Registrar o serviço de IA do domínio
                services.AddScoped<IGenerateRecipeAI, ChatAIServices>();
            }
        }
    }
}
