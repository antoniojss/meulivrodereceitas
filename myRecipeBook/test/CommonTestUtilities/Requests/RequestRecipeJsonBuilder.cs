using Bogus;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Enums;
using System;
using System.Collections.Generic;

namespace CommonTestUtilities.Requests
{
    public static class RequestRecipeJsonBuilder
    {
        public static RequestRecipeJson Build()
        {
            var instructionOrder = 1;

            return new Faker<RequestRecipeJson>()
                .RuleFor(r => r.Title, f => f.Lorem.Word())
                .RuleFor(r => r.CookTime, f => f.PickRandom<CookTime>())
                .RuleFor(r => r.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
                .RuleFor(r => r.DishTypes, f =>  f.Make(2, ()=> f.PickRandom<DishType>()).Distinct().ToList())
                .RuleFor(r => r.Instructions, f => f.Make(3, ()=> new RequestRecipeInstructionJson
                {
                    Order = instructionOrder++,
                    Description = f.Lorem.Sentence(),

                }));
        }
    }
}
