using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace myRecipeBook.Exception.ExceptionBase
{
    public  class InvalidLoginException : MyRecipeBookException
    {
        public override List<string> GetErrorMessages() =>[ResourceMessagesException.VALIDATION_LOGIN_INVALID];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
