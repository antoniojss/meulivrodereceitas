using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Requests
{
    public  class RequestUpdateUserJson
    {
        public string Name { get; set; } = string.Empty;   
        public string Email { get; set; } = string.Empty;  

    }
}
