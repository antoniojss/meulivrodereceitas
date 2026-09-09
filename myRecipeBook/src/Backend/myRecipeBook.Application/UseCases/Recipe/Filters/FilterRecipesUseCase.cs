using Mapster;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Dtos;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.Filters
{
    public class FilterRecipesUseCase : IFilterRecipesUseCase
    {
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public FilterRecipesUseCase(IRecipeReadOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRecipesJson> Execute(RequestFilterRecipesJson? request)
        {  
            var filter = request is null ? new RecipeFilterDto() :
                new RecipeFilterDto
                {
                    SearchTerm = request.SearchTerm,
                    CookTime = (Domain.Enums.CookTime?)request.CookTime,
                    DishTypes = request.dishTypes.Select(dishType => (Domain.Enums.DishType)dishType).ToList()
                };


            var recipes = await _repository.FilterRecipes(_loggedUser.GetUserId(), filter);

            return new ResponseRecipesJson
            {
                Recipes = recipes.Adapt<IList<ResponseRecipeSummaryJson>>()
            };

        }
    }
}
