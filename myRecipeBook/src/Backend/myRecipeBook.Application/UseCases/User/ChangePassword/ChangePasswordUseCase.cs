using FluentValidation.Results;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace myRecipeBook.Application.UseCases.User.ChangePassword
{
    public  class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;   

        public ChangePasswordUseCase(ILoggedUser loggedUser, 
            IPasswordHasher passwordHasher, 
            IUserUpdateOnlyRepository userUpdateOnlyRepository)
        {
            _loggedUser = loggedUser;
            _passwordHasher = passwordHasher;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;       
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.Get();

            Validate(request, loggedUser);

            var hashedPassword = _passwordHasher.HashPassword(request.NewPassword);    
            await _userUpdateOnlyRepository.UpdatePassword(loggedUser.Id, hashedPassword);
        }
        
        private void Validate(RequestChangePasswordJson request,Domain.Entities.User loggedUser)
        {
           var result = new ChangePasswordValidator().Validate(request);
            
            if (_passwordHasher.VerifyPassword(request.CurrentPassword, loggedUser.Password) == false)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_CURRENT_PASSWORD));
            }

            if(result.IsValid==false)
            {
                throw new ErrorOnValidationException(result.Errors.Select(err => err.ErrorMessage).ToList());
            }
        }
    }
}
