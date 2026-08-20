using System;
using System.Collections.Generic;

namespace myRecipeBook.Communication.Requests
{
    // Request utilizada tanto para criação quanto para atualização de Recipe
    using myRecipeBook.Communication.Enums;

    public class RequestRecipeJson
    {
        // Id opcional: quando null ou Guid.Empty -> fluxo de criação; quando preenchido -> fluxo de atualização
        
        public Guid? Id { get; set; } = null;

        public string Title { get; set; } = string.Empty;

        public CookTime CookTime { get; set; }

        //**

        public IList<string> Ingredients { get; set; } = new List<string>();

        public IList<RequestRecipeInstructionJson> Instructions { get; set; } = new List<RequestRecipeInstructionJson>();
        //** 
        public IList<DishType> DishTypes { get; set; } = new List<DishType>();

    }
}
