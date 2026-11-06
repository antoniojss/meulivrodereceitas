using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Requests
{
    public  class RequestNewTokenJson
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
