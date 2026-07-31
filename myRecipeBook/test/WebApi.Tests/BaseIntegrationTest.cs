using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using ZstdSharp.Unsafe;

namespace WebApi.Tests
{
    public abstract class BaseIntegrationTest : IClassFixture<MyRecipeBookApplicationFactory>, IDisposable
    {
        private readonly IServiceScope _scope;

        private readonly HttpClient _httpClient;
        internal readonly MyRecipeBookDbContext DbContext;

        public  BaseIntegrationTest(MyRecipeBookApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();

            _scope = factory.Services.CreateScope();

            DbContext = _scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
        }
        protected async Task<HttpResponseMessage> Post(string requestURI,
            object request, string culture = "en-US")
        {
            ChangeRequestCulture(culture);
            return  await _httpClient.PostAsJsonAsync(requestURI, request);
        }

        private void ChangeRequestCulture(string culture)
        {
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
       }

        public void Dispose()
        {
            _scope?.Dispose();
            DbContext?.Dispose();    
        }
    }
}
