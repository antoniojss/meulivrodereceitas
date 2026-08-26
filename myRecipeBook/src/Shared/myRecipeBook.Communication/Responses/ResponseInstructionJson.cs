using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Responses
{
    public class ResponseInstructionJson
    {
        public int Order { get; set; }
        public string Description { get; set; } = string.Empty;  
    }
}
