using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.RefreshToken;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace myRecipeBook.Application.UseCases.Token.UserRefreshToken
{
    public class UserRefreshTokenUseCase : IUserRefreshTokenUseCase
    {
        private const int ExpitarionTimeInDays = 7;

        private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
        private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        private readonly IAccessTokenGenerator _accessTokenGenerator;

        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        public readonly IUnitOfWork _unitOfWork;

        public UserRefreshTokenUseCase(
            IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
            IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
            _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
        {
            var refreshToken = await _refreshTokenReadOnlyRepository.Get(request.RefreshToken);
            if (refreshToken == null)
                throw new RefreshTokenExpiredException();

            var isValid = refreshToken.CreatedAt.AddDays(ExpitarionTimeInDays) > DateTime.UtcNow;
            if (isValid == false)
                throw new RefreshTokenExpiredException();

            var user = await _userReadOnlyRepository.GetById(refreshToken.UserId);

            var newRefreshToken = new RefreshToken
            {
                UserId = refreshToken.UserId,
                Value = _refreshTokenGenerator.GenerateToken()

            };
            await _refreshTokenWriteOnlyRepository.Replace(newRefreshToken);
            await _unitOfWork.Commit();

            return new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user!),
                RefreshToken = newRefreshToken.Value    
            };
        }

    }
}
