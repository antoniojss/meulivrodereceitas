using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.VisualBasic;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Storage;
using myRecipeBook.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.Storage
{

    internal sealed class AzureStorageService : IStorageService
    {
        private const string ProfilePicturesFileName = "profile-picture";
        private const uint ProfilePictureExpirationInMinutes = 60;
        private const uint RecipeIllustrationExpirationInMinutes = 60;

        private readonly BlobServiceClient _blobServiceClient;

        public AzureStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task UploadProfilePicture(User user, Stream file, string contentType)
        {
            await Upload(user.Id, file, ProfilePicturesFileName, contentType);  

        }
        public async Task UploadIllustration(Recipe recipe, Stream file, string contentType)
        {
            await Upload(recipe.UserId, file, recipe.Id.ToString(), contentType);
        }

        public string GetProfilePictureUrl(User loggedUser)
        {
            return GenerateReadUrl(loggedUser.Id, ProfilePicturesFileName, ProfilePictureExpirationInMinutes);
        }

        public string GetIllustrationUrl(Recipe recipe)
        {
            return GenerateReadUrl(recipe.UserId, recipe.Id.ToString(), RecipeIllustrationExpirationInMinutes);
        }
       
        private async Task Upload(Guid userId, Stream file, string blobName, string contentType)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(userId.ToString());

            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(file, new BlobHttpHeaders
            {
                ContentType = contentType
            });

        }

        public string GetRecipeIllustrationUrl(Guid userId, Guid recipeId)
        {
            return GenerateReadUrl(userId, recipeId.ToString(), RecipeIllustrationExpirationInMinutes);
        }

        public string GenerateReadUrl(Guid userId, string blobName, uint expirationInMinutes)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(userId.ToString());

            var blobClient = containerClient.GetBlobClient(blobName);
            if (!blobClient.Exists())
            {
                return null;
            }
            //sasURi shared access signature URI, which allows access to the blob for a limited time without requiring authentication.
            var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(expirationInMinutes));
            return sasUri.ToString();
        }

  
    }
}
