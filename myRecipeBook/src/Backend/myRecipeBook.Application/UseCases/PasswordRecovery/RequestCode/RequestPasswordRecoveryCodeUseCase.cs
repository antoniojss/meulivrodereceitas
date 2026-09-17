using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Enums;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Repositories.VerificationCode;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Domain.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace myRecipeBook.Application.UseCases.PasswordRecovery.RequestCode
{
    public class RequestPasswordRecoveryCodeUseCase : IRequestPasswordRecoveryCodeUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        private readonly IVerificationCodeWriteOnlyRepository _verificationCodeWriteOnlyRepository;

        private readonly IUnitOfWork _unitOfWork;

        public RequestPasswordRecoveryCodeUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IVerificationCodeWriteOnlyRepository verificationCodeWriteOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _verificationCodeWriteOnlyRepository = verificationCodeWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task Execute(RequestPasswordRecoveryJson request)
        {
            var user = await _userReadOnlyRepository.GetByEmail(request.Email);
            if (user is null)
                return;

            var code = RandomNumberGenerator.GetInt32(1, 1_000_000);

            var verificationCode = new VerificationCode
            {
                Code = code.ToString("D6"),
                UserId = user.Id,
                Type = VerificationCodeType.PasswordRecovery
            };


            //TODO: Send email with the code to user 

            await _verificationCodeWriteOnlyRepository.Replace(verificationCode);
            await _unitOfWork.Commit();
        }
    }
}
