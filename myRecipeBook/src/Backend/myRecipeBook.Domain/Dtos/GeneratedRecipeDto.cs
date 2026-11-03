using myRecipeBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Dtos
{
    public record GeneratedRecipeDto
    {
        public string Title { get; init; } = string.Empty;      
        public string Description { get; init; } = string.Empty;        
        
        public string Difficulty { get; init; } = string.Empty; 

        public string Servings { get; init; } = string.Empty;

        public CookTime CookTime { get; init; }       

        public IList<GeneratedIngredientDto> Ingredients { get; init; } = [];

        public IList<GeneratedInstructionDto> Instructions { get; init; } = [];
        public byte[] Image { get; init; } = [];

    }

    public record GeneratedInstructionDto
    {
        public int Order{ get; init; }
        public string Description { get; init; } = string.Empty;
    }
    public record GeneratedIngredientDto
    {
        public string Name { get; init; } = string.Empty;
        public string Quantity { get; init; } = string.Empty;
        public string Unit { get; init; } = string.Empty;       
    }
}
