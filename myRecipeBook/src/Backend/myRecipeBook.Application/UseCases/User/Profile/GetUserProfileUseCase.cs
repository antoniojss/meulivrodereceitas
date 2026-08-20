using Mapster;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.Profile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;

        public GetUserProfileUseCase(ILoggedUser loggedUser)
        {
            _loggedUser = loggedUser;
        }

        public async Task<ResponseUserProfileJson> Execute()
        {
            var loggedUser = await _loggedUser.Get();

            return loggedUser.Adapt<ResponseUserProfileJson>();   
        }
    }
}
