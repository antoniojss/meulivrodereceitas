using myRecipeBook.Communication.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Responses
{
    public  class ResponseGeneratedRecipeJson
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Difficulty { get; init; } = string.Empty;
        public string Servings { get; init; } = string.Empty;
        public CookTime CookTime { get; set; }
        public IList<GeneratedIngredientJson> Ingredients { get; set; } = new List<GeneratedIngredientJson>();
        public IList<ResponseInstructionJson> Instructions { get; set; } = new List<ResponseInstructionJson>();
        public byte[] Image { get; set; } = [];

        public class GeneratedIngredientJson
        {
            public string Quantity { get; init; } = string.Empty;
            public string Unit { get; init; } = string.Empty;
            public string Name { get; init; } = string.Empty;

        }
    }
}
