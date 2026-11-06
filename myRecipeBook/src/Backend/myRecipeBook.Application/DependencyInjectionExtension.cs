using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using myRecipeBook.Application.Mappings;
using myRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using myRecipeBook.Application.UseCases.PasswordRecovery.RequestCode;
using myRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;
using myRecipeBook.Application.UseCases.Recipe.DeleteById;
using myRecipeBook.Application.UseCases.Recipe.Filters;
using myRecipeBook.Application.UseCases.Recipe.GeneratedRecipeAI;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Application.UseCases.Recipe.Recent;
using myRecipeBook.Application.UseCases.Recipe.Register;
using myRecipeBook.Application.UseCases.Recipe.UpdateById;
using myRecipeBook.Application.UseCases.Token.UserRefreshToken;
using myRecipeBook.Application.UseCases.User.ChangePassword;
using myRecipeBook.Application.UseCases.User.ChangeProfilePicture;
using myRecipeBook.Application.UseCases.User.Profile;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Application.UseCases.User.Update;
using myRecipeBook.Domain.Repositories.VerificationCode;
using myRecipeBook.Infrastructure.DataAccess.Repositories;
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
     
            //Verification Code  
            services.AddScoped <IRequestPasswordRecoveryCodeUseCase, RequestPasswordRecoveryCodeUseCase>();
            services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();

            services.AddScoped<IChangeProfilePictureUseCase, ChangeProfilePictureUseCase>();

            services.AddScoped<IGenerateRecipeAIUseCase, GenerateRecipeAIUseCase>();

            services.AddScoped<IUserRefreshTokenUseCase, UserRefreshTokenUseCase>();
        }
    }
}
