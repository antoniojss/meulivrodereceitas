using Mapster;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.Recent
{
    public class GetRecentRecipesUseCase : IGetRecentRecipesUseCase
    {

        private readonly IRecipeReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public GetRecentRecipesUseCase(IRecipeReadOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRecipesJson> Execute()
        {
            var recipes = await _repository.GetRecentRecipes(_loggedUser.GetUserId());
           
            var response = new ResponseRecipesJson
            {
                Recipes = recipes.Adapt<IList<ResponseRecipeSummaryJson>>()
            };
            
            return response;
        }   
    }
}   
