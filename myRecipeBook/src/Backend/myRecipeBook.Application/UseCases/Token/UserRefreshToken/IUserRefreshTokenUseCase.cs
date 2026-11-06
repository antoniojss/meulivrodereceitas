using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Token.UserRefreshToken
{
    public  interface IUserRefreshTokenUseCase
    {
        Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
    }
}
