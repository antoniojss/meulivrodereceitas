using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Dtos;
using myRecipeBook.Domain.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.Extensions
{
    internal static class RecipeSummaryExtensions
    {
        extension(IList<RecipeSummaryDto> recipes)
        {
            internal IList<ResponseRecipeSummaryJson> ToResponseJson(Guid userId, IStorageService storageService)
            {
                return recipes
                       .Select(recipe => new ResponseRecipeSummaryJson
                       {
                           Id = recipe.Id,
                           Title = recipe.Title,
                           ImageUrl = recipe.HasImage ? storageService.GetRecipeIllustrationUrl(userId: userId, recipeId: recipe.Id) : string.Empty
                       }).ToList();
        } }
    }
}
