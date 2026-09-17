using FluentValidation;
using myRecipeBook.Application.UseCases.Recipe;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Enums;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Repositories.VerificationCode;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace myRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword
{
    public class ResetPasswordUseCase : IResetPasswordUseCase   
    {
        private const int VERIFICATION_CODE_EXPIRATION_MINUTES = 10;

        public readonly IUserReadOnlyRepository _userReadOnlyRepository;
        public readonly IVerificationCodeReadOnlyRepository _verificationCodeReadOnlyRepository;
        public readonly IVerificationCodeWriteOnlyRepository _verificationCodeWriteOnlyRepository;
        public readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        public readonly IPasswordHasher _passwordHasher;


        public ResetPasswordUseCase(IUserReadOnlyRepository userReadOnlyRepository,
          IVerificationCodeReadOnlyRepository verificationCodeReadOnlyRepository,
          IVerificationCodeWriteOnlyRepository verificationCodeWriteOnlyRepository,
          IUserUpdateOnlyRepository userUpdateOnlyRepository,   
          IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;   
            _userReadOnlyRepository = userReadOnlyRepository;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
            _verificationCodeReadOnlyRepository = verificationCodeReadOnlyRepository;
            _verificationCodeWriteOnlyRepository = verificationCodeWriteOnlyRepository; 
        }


        public async Task Execute(RequestResetPasswordJson request)
        {
            var user = await _userReadOnlyRepository.GetByEmail(request.Email);
            if (user is null)
                throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_VERIFICATION_CODE_INVALID]);   
                    
            var verificationCode = await _verificationCodeReadOnlyRepository.Get(user.Id, request.Code, VerificationCodeType.PasswordRecovery);

            if (verificationCode is null)
                throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_VERIFICATION_CODE_INVALID]);

          ValidateAndThrowOnFailures(request, verificationCode);
             
             var hashedPassword = _passwordHasher.HashPassword(request.NewPassword);    

            await _userUpdateOnlyRepository.UpdatePassword(user.Id, hashedPassword);    

            await _verificationCodeWriteOnlyRepository.Delete(verificationCode);      

        }

        public static void ValidateAndThrowOnFailures(RequestResetPasswordJson request, VerificationCode verificationCode)
        {
            var isCodeValid = verificationCode.CreatedAt.AddMinutes(VERIFICATION_CODE_EXPIRATION_MINUTES) >= DateTime.UtcNow;
            if (!isCodeValid)
                throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_VERIFICATION_CODE_INVALID]);
    
            var result = new ResetPasswordValidator().Validate(request);

            if (result.IsValid == false)
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }

       
    }
}
