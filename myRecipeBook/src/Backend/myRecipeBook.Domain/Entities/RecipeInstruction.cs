using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Entities
{
   public class RecipeInstruction : EntityBase
    {
        public int Order { get; set; }      
        public string Description { get; set; } = string.Empty;
        public Guid RecipeId { get; set; }
    }
}
