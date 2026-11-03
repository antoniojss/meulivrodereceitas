using Mapster;
using myRecipeBook.Application.Extensions;
using myRecipeBook.Application.UseCases.Recipe.DeleteById;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Domain.Storage;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.Register
{
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        private readonly IStorageService _storageService;   
        private readonly IRecipeWriteOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterRecipeUseCase(
             IRecipeWriteOnlyRepository repository,
             ILoggedUser loggedUser,
             IUnitOfWork unitOfWork,
             IStorageService storageService
            )
        {

             _repository = repository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _storageService = storageService;

        }

        public async Task<ResponseRegistredRecipeJson> Execute(
            RequestRecipeJson request, Stream? recipeIllustration)
        {
            ValidateAndThrowOnFailure(request);

            var recipe = request.Adapt<Domain.Entities.Recipe>();

            //MapsterConfiguration no projeto de Application
            //Ingredients e DishTypes , são enum, eles tem que serem convertidos para uma entidade 
            // o Mapster tem que receber uma configuração especifica par esta finalidade

            recipe.UserId = _loggedUser.GetUserId();

            if (recipeIllustration is not null)
            {
                var contentType = recipeIllustration.DetectImageContentType();
                if (contentType.IsEmpty())
                    throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_VERIFICATION_ONLY_IMAGES_ACCEPTED]);
            
                recipe.HasImage = true; 
                await _storageService.UploadIllustration(recipe , recipeIllustration, contentType);

            }

            await _repository.Add(recipe);

            await _unitOfWork.Commit();

            return new  ResponseRegistredRecipeJson 
            {
                Id = recipe.Id,
                Title = recipe.Title,
                ImageUrl = recipe.HasImage ? _storageService.GetRecipeIllustrationUrl(userId: recipe.UserId, recipeId: recipe.Id) : string.Empty
            };


       }

        public static void ValidateAndThrowOnFailure(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);
               
           if(result.IsValid==false)
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }

    }
}
