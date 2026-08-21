using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using myRecipeBook.API.Filters;
using System.Globalization;
using myRecipeBook.Application;
using myRecipeBook.Infrastructure;
using myRecipeBook.API.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Exception;
using Microsoft.OpenApi;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.API.Token;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//7 - esta alteração no controle e para pegar os dados do regex e colocar certo no nome
//converter o enum para texto ao invez de salvar o valor 
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new StringConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// teste de checkin 17-06-2026
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//1 - incluir o pacote do seagger no pacote nuget Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter only your access token. Swegger will add 'Bearer' prefix automatically",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(openApiDocument =>
    {
        return new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", openApiDocument),
                       // new List<string>() -- lista de string vazia
                       []
                    }

                };
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
//Colocar static na classe DependencyInjectionExtension para poder chamar o método
//AddInfrastructure() sem precisar instanciar a classe
//myRecipeBook.Infrastructure.DependencyInjectionExtension.AddInfrastructure(builder.Services);
//myRecipeBook.Application.DependencyInjectionExtension.AddApplication(builder.Services);
builder.Services.AddApplication();

builder.Services.AddScoped<IAccessTokenProvider, HttpContextTokenProvider>();
builder.Services.AddHttpContextAccessor();


//3 definir a injeção de dependencia para dar suporte a multiplos idiomas
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en"),
        new CultureInfo("pt-BR"),
        new CultureInfo("es"),
    };

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
});

//5 - injetar o serviço de exception, para receber quaisquer erros de execão que retorrnar da API
builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

//6 - converter todas as urls para minusculo, para não ter problemas de rotas com letras maiusculas e minusculas
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtoptions =>
    {
        var signingKey = builder.Configuration.GetValue<string>("Jwt:SigningKey")!;

        jwtoptions.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateLifetime = true,

            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };

        jwtoptions.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var subject = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(subject, out var userId) == false)
                {
                    context.Fail("Invalid Token Subject");
                    return;
                }

                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyRepository>();

                var UserExists = await userRepository.ExistActiveUserWithId(userId);
                if (UserExists == false)
                {
                    context.Fail("User not found");
                    //return;
                }
            },
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = context.AuthenticateFailure switch
                {
                    null => new ResponseErrorJson(ResourceMessagesException.VALIDATION_ACCESS_TOKEN_REQUIRED),
                    SecurityTokenExpiredException => new ResponseErrorJson("Token Expired", accessTokenExpired: true),
                    _ => new ResponseErrorJson(ResourceMessagesException.VALIDATION_RESOURCE_ACCESS_DENIED),
                };

                await context.Response.WriteAsJsonAsync(response);
            }

        };

    });

var app = builder.Build();

//acessando o serviço de injeção de dependencia 
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //2 - incluir os seviços do Swagger do pacote nuget Swashbuckle.AspNetCore
    app.MapSwagger();
    app.MapSwaggerUI();

    //3 - quando estiver desenvolvedo a aplicação abrir a pasta de Properties e alterar o arquivo launchSettings.json,
    //    launchBrowser: true,  "launchUrl": "Swagger", para aparecer na tela.


}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await ExecuteMigrations();

app.Run();

async Task ExecuteMigrations()
{
    await using var scope = app.Services.CreateAsyncScope();
    DatabaseMigration.ExecuteMigration(scope.ServiceProvider);
}
public partial class Program { } // Expose Program class for integration tests