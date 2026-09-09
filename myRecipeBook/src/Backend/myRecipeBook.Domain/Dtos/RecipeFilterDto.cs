using myRecipeBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Dtos
{
    public record RecipeFilterDto
    {
        public string? SearchTerm { get; init; }
        public CookTime? CookTime { get; init; }
        public IList<DishType> DishTypes { get; init; } = [];
    }
}
