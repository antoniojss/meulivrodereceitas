using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.PasswordHashing;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Login.WithEmailAndPassword
{
    public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        
        public LoginWithEmailAndPasswordUseCase(
            IPasswordHasher passwordHasher,
            IUserReadOnlyRepository userReadOnlyRepository)
        { 
            _passwordHasher = passwordHasher;
            _userReadOnlyRepository = userReadOnlyRepository;   
        }

        async Task<ResponseRegistredUserJson> ILoginWithEmailAndPasswordUseCase.Execute(RequestLoginJson request)
        {
            var user = await _userReadOnlyRepository.GetByEmail(request.Email);

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);

            return new ResponseRegistredUserJson
            {
                Name = user.Name 
            };
        }
    }
}
