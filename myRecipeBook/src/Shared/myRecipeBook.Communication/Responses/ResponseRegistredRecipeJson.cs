using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Responses
{
    public  class ResponseRegistredRecipeJson
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

    }
}
