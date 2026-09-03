using myRecipeBook.Communication.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Responses
{
    public class ResponseRecipesJson
    {
         public IList<ResponseRecipeSummaryJson> Recipes { get; set; } = [];
       

    }
}
