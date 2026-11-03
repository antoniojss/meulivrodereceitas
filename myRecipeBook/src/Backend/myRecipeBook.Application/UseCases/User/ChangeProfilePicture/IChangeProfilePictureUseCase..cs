using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.ChangeProfilePicture
{
    public interface IChangeProfilePictureUseCase
    {
        Task Execute(Stream profilePicture);    
    }
}
