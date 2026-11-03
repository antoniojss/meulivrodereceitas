using Mapster;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Domain.Storage;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {

        private readonly IRecipeReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        private readonly IStorageService _storageService;

        public GetRecipeByIdUseCase(
            IRecipeReadOnlyRepository repository, 
            ILoggedUser loggedUser,
            IStorageService storageService)
        {
            _repository = repository;
            _loggedUser = loggedUser;
            _storageService = storageService;
        }

        public async Task<ResponseRecipeJson> Execute(Guid recipeId)
        {
            var recipe = await _repository.GetById(recipeId, _loggedUser.GetUserId()); 
            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.VALIDATION_INVALID_RECIPE_NOT_FOUND);
           
            var response = recipe.Adapt<ResponseRecipeJson>();
            response.ImageUrl = recipe.HasImage ? _storageService.GetRecipeIllustrationUrl(userId: recipe.UserId, recipeId = recipe.Id) : string.Empty;
            return response;  
        }
    }
}
