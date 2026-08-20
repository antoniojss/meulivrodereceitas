using myRecipeBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Entities
{
   public  class RecipeDishType : EntityBase    
    {
        public DishType Type { get; set; } 
        public Guid RecipeId { get; private set; }      
    }
}
