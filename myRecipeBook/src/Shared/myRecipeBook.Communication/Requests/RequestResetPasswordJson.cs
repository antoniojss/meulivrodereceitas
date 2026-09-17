using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Requests
{
    public  class RequestResetPasswordJson
    {
        public string Email { get; set; } = string.Empty;       
        public string Code { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
