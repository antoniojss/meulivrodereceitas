using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Exception.ExceptionBase
{
    // esta classe herda um conjunto de exception da clase MyRecipeBookException que e do tipo System.Exception
    // readonly é proque não pode ser altarado em outros lugares que não aqui
    public class ErrorOnValidationException : MyRecipeBookException
    {
        private readonly List<string> _errors;
        public ErrorOnValidationException(List<string> errorMessages)
        {
            _errors = errorMessages;
        }

        /* esta e a forma tradicional de criar um metodo para retornar a lista de erros, mas a forma 
         * mais moderna e usando a sintaxe de expressão lambda
         
        public List<string> GetErrorMessages() 
        {
            return _errors;
        }
        */

        public List<string> GetErrorMessages() => _errors;
    }

}
