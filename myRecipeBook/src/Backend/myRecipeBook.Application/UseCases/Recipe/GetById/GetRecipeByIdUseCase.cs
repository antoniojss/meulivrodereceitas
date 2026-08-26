using Mapster;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.Recipe;
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

        public GetRecipeByIdUseCase(IRecipeReadOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;   
        }

        public async Task<ResponseRecipeJson> Execute(Guid recipeId)
        {
            var recipe = await _repository.GetById(recipeId, _loggedUser.GetUserId()); 
            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.VALIDATION_INVALID_RECIPE_NOT_FOUND);
           
            return recipe.Adapt<ResponseRecipeJson>();
        }
    }
}
