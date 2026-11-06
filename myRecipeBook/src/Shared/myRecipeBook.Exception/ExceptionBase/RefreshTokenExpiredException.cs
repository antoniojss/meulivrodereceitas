using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Versioning;
using System.Text;

namespace myRecipeBook.Exception.ExceptionBase
{
    public class RefreshTokenExpiredException : MyRecipeBookException
    {
        public override List<string> GetErrorMessages() => [ResourceMessagesException.REFRESH_TOKEN_EXPIRED];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }

}
