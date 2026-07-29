using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace myRecipeBook.Exception.ExceptionBase
{
    //como usamos o nome Exepction nas classes e preciso dizer de onde e que e para ele pegar  
    // em nenhum lugar pode ter new MyRecipeBookException
    //professor -  para que foi criado uma nova classe que erda esta aqui ErrorOnValidationException
    public abstract class MyRecipeBookException : System.Exception
    {
        public abstract HttpStatusCode GetStatusCode();
        public abstract List<string> GetErrorMessages();

    }
}
