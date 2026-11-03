using myRecipeBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Storage
{
    public interface IStorageService
    {
        Task UploadProfilePicture(User user, Stream file, string contentType);
        Task UploadIllustration(Recipe recipe, Stream file, string contentType);
        string GetProfilePictureUrl(User loggedUser);
        string GetRecipeIllustrationUrl(Guid userId, Guid recipeId);
    }
}