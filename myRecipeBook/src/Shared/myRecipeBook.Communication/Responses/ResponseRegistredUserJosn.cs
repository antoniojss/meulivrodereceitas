using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Responses
{
    public class ResponseRegistredUserJson
    {
        public string Name { get; set; } = string.Empty;    
        public ResponseTokensJson Tokens { get; set; } = new ResponseTokensJson();  
    }
}
