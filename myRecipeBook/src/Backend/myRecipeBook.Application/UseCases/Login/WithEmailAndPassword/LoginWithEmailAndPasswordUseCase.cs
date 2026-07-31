using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Login.WithEmailAndPassword
{
    public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;


        public LoginWithEmailAndPasswordUseCase(
            IPasswordHasher passwordHasher,
            IUserReadOnlyRepository userReadOnlyRepository,
            IAccessTokenGenerator accessTokenGenerator)

        {
            _passwordHasher = passwordHasher;
            _userReadOnlyRepository = userReadOnlyRepository;
            _accessTokenGenerator = accessTokenGenerator;

        }

        public async Task<ResponseRegistredUserJson> Execute(RequestLoginJson request)
        {
            var user = await _userReadOnlyRepository.GetByEmail(request.Email);
            if (user is null)
                throw new InvalidLoginException();

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
            if (isPasswordValid == false)
                throw new InvalidLoginException();

            return new ResponseRegistredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user)
                }
            };
        }
    }
}
