using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Responses
{
    public  class ResponseErrorJson
    {
        public List<string> Errors { get; private set; }

        public bool AccessTokenExpired { get; private set; }   

        public ResponseErrorJson(List<string> erroMessages) => Errors = erroMessages;

        /* modo normal e o novo modo e mais simples e mais moderno, usando a sintaxe de expressão lambda
        public ResponseErrorJson(List<string> errorMenssage) 
        {
            Errors = errorMessage;
        }
        */

        public ResponseErrorJson(string errorMessage) => Errors = [errorMessage]; 

        public ResponseErrorJson(string errorMessage, bool accessTokenExpired)
        {
            Errors = [errorMessage];
            AccessTokenExpired = accessTokenExpired;
        }

    }
}
