using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Application.UseCases.User.Register;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
        }
    }
}
