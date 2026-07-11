using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using myRecipeBook.API.Filters;
using System.Globalization;
using myRecipeBook.Application;
using myRecipeBook.Infrastructure;
using myRecipeBook.API.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//7 - esta alteração no controle e para pegar os dados do regex e colocar certo no nome 
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));  
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// teste de checkin 17-06-2026
 
builder.Services.AddOpenApi();
//1 - incluir o pacote do seagger no pacote nuget Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
//Colocar static na classe DependencyInjectionExtension para poder chamar o método
//AddInfrastructure() sem precisar instanciar a classe
//myRecipeBook.Infrastructure.DependencyInjectionExtension.AddInfrastructure(builder.Services);
//myRecipeBook.Application.DependencyInjectionExtension.AddApplication(builder.Services);
builder.Services.AddApplication();


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