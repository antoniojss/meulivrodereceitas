using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.Profile
{
    public interface IGetUserProfileUseCase
    {
        Task<ResponseUserProfileJson> Execute();        
    }
}
