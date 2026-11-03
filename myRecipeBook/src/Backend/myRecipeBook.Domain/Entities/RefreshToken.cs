using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public string Value { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
        public Guid UserId { get; set; } 

    }
}
