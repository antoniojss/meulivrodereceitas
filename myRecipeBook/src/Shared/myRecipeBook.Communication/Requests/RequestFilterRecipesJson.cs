using myRecipeBook.Communication.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Requests
{
    public class RequestFilterRecipesJson
    {
        public string? SearchTerm { get; set; }
        public CookTime? CookTime { get; set; }
        public IList<DishType> dishTypes { get; set; } = [];
    }
}
