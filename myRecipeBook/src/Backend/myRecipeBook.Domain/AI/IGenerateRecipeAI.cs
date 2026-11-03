using myRecipeBook.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.AI
{
    public interface IGenerateRecipeAI
    {
        Task<GeneratedRecipeDto?> GenerateRecipe(string prompt);
    }
}
