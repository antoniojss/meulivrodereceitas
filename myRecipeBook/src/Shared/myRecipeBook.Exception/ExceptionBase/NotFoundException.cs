using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace myRecipeBook.Exception.ExceptionBase
{
    public class NotFoundException : MyRecipeBookException
    {
        private readonly string _message;
        public NotFoundException(string message)
        {
            _message = message;

        }
        public override List<string> GetErrorMessages() => [_message];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
