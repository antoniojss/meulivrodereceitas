using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.RefreshToken;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.Domain.Storage;
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

        private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;

        private readonly IAccessTokenGenerator _accessTokenGenerator;

        private readonly IStorageService _storageService;

        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        private readonly IUnitOfWork _unitOfWork;
               

        public LoginWithEmailAndPasswordUseCase(
            IPasswordHasher passwordHasher,
            IUserReadOnlyRepository userReadOnlyRepository,
            IAccessTokenGenerator accessTokenGenerator,
            IStorageService storageService,
            IRefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
            IUnitOfWork unitOfWork  )

        {
            _passwordHasher = passwordHasher;
            _userReadOnlyRepository = userReadOnlyRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _storageService = storageService;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegistredUserJson> Execute(RequestLoginJson request)
        {
            var user = await _userReadOnlyRepository.GetByEmail(request.Email);
            if (user is null)
                throw new InvalidLoginException();

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
            if (isPasswordValid == false)
                throw new InvalidLoginException();

            var refreshToken = await GenerateAndSaveRefreshToken(user.Id);

            await _unitOfWork.Commit();

            return new ResponseRegistredUserJson
            {
                Name = user.Name,
                ImageUrl = user.HasImage ? _storageService.GetProfilePictureUrl(user) : string.Empty,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user),
                    RefreshToken = refreshToken 
                }

            };
        }
        private async Task<string> GenerateAndSaveRefreshToken(Guid userId)
        {
            var refreshToken = new RefreshToken
            {
                Value = _refreshTokenGenerator.GenerateToken(),
                UserId = userId,
            };
            await _refreshTokenWriteOnlyRepository.Replace(refreshToken);
            return refreshToken.Value;
        }
    }
}
