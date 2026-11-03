using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Mapster;
using myRecipeBook.Application.Extensions;
using myRecipeBook.Domain.Extensions;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Storage;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.ChangeProfilePicture
{
    public class ChangeProfilePictureUseCase : IChangeProfilePictureUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IStorageService _storageService;

        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        public ChangeProfilePictureUseCase(ILoggedUser loggedUser, IStorageService storageService, IUserUpdateOnlyRepository userUpdateOnlyRepository)
        {
            _loggedUser = loggedUser;
            _storageService = storageService;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
        }

        public async Task Execute(Stream profilePicture)
        {

            var isImageValid = profilePicture.DetectImageContentType();
            if (isImageValid.IsEmpty())
                throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_VERIFICATION_ONLY_IMAGES_ACCEPTED]);
            
            var loggedUser = await _loggedUser.Get(); 

            await _storageService.UploadProfilePicture(loggedUser, profilePicture, isImageValid);

            await _userUpdateOnlyRepository.UpdateProfilePictureStatus(loggedUser.Id, hasProfilePicture: true);
        }
    }
}