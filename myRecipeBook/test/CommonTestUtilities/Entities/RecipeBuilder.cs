using Bogus;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Entities
{
  public class RecipeBuilder
    {
        public static Recipe Build(User user)
        {
            var instructionOrder = 1;

            return new Faker<Recipe>()
                .RuleFor(r => r.Title, f => f.Lorem.Word())
                .RuleFor(r => r.CookTime, f => f.PickRandom<CookTime>())
                .RuleFor(r => r.Ingredients, f => f.Make(3, () => new RecipeIngredient
                {
                    Item = f.Commerce.ProductName()
                }))
                .RuleFor(r => r.DishTypes, f => f.Make(2, () => new RecipeDishType
                {
                    Type = f.PickRandom<DishType>()
                }))
                .RuleFor(r => r.Instructions, f => f.Make(3, () => new RecipeInstruction
                {
                    Order = instructionOrder++,
                    Description = f.Lorem.Sentence(),

                }))
                .RuleFor(r => r.UserId, f => user.Id);
        }
    }
}
