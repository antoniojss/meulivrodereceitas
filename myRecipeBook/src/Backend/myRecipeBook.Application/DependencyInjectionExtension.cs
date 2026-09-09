using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Application.Mappings;
using myRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using myRecipeBook.Application.UseCases.Recipe.DeleteById;
using myRecipeBook.Application.UseCases.Recipe.Filters;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Application.UseCases.Recipe.Recent;
using myRecipeBook.Application.UseCases.Recipe.Register;
using myRecipeBook.Application.UseCases.Recipe.UpdateById;
using myRecipeBook.Application.UseCases.User.ChangePassword;
using myRecipeBook.Application.UseCases.User.Profile;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Application.UseCases.User.Update;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace myRecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
           services.AddUseCases();
           MapsterConfiguration.Configure();
        }
        public static void AddUseCases(this IServiceCollection services)
        {
            //Users 
            services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
            services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            
            //Recipe
            services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
            services.AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCase>();
            services.AddScoped<IDeleteRecipeByIdUseCase, DeleteRecipeByIdUseCase>();
            services.AddScoped<IUpdateRecipeByIdUseCase, UpdateRecipeByIdUseCase>();
            services.AddScoped<IGetRecentRecipesUseCase, GetRecentRecipesUseCase>();
            services.AddScoped <IFilterRecipesUseCase, FilterRecipesUseCase>();

        }
    }
}
