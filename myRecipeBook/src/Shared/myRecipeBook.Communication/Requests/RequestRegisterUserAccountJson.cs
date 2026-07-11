using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Communication.Requests
{
    //tem que ser igual ao que esta declarado na classe User.cs,
    //para que o Mapster consiga mapear os campos corretamente

    public class RequestRegisterUserAccountJson
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
