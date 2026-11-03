using Mapster;
using myRecipeBook.Application.Extensions;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Domain.Storage;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace myRecipeBook.Application.UseCases.Recipe.Recent
{
    public class GetRecentRecipesUseCase : IGetRecentRecipesUseCase
    {

        private readonly IRecipeReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        private readonly IStorageService _storageService;

        public GetRecentRecipesUseCase(
            IRecipeReadOnlyRepository repository, 
            ILoggedUser loggedUser,
            IStorageService storageService)
        {
            _repository = repository;
            _loggedUser = loggedUser;
            _storageService = storageService;
        }

        public async Task<ResponseRecipesJson> Execute()
        {
            var recipes = await _repository.GetRecentRecipes(_loggedUser.GetUserId());
           
            var response = new ResponseRecipesJson
            {
                Recipes = recipes.ToResponseJson(_loggedUser.GetUserId(), _storageService)

            };
            
            return response;
        }   
    }
}   
