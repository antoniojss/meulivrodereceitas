using Mapster;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
[assembly:InternalsVisibleTo("UseCases.Tests")]

namespace myRecipeBook.Application.Mappings
{
    internal static class MapsterConfiguration
    {
       internal static void Configure()
        {
            TypeAdapterConfig<RequestRegisterUserAccountJson, User>
                 .NewConfig()
                 .Ignore(destination => destination.Password);

            TypeAdapterConfig<RequestRecipeJson, Recipe>
                .NewConfig()
                .Map(destination => destination.Ingredients,
                     request => request.Ingredients.Select(ingredient => new RecipeIngredient
                     {
                         Item = ingredient
                     }))
                .Map(destination => destination.DishTypes,
                     request => request.DishTypes.Select(dishtypes => new RecipeDishType
                     {
                        Type = (Domain.Enums.DishType)dishtypes
                     }));
        }
    }
}
