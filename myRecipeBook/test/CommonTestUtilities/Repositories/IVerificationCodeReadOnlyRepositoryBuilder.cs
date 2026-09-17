using Moq;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.VerificationCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
   public class IVerificationCodeReadOnlyRepositoryBuilder
    {
        private readonly Mock<IVerificationCodeReadOnlyRepository> _mock = new();

        public IVerificationCodeReadOnlyRepositoryBuilder Get(VerificationCode verificationCode)
        {
            _mock
                .Setup(repo => repo.Get(verificationCode.UserId, verificationCode.Code,verificationCode.Type))
                .ReturnsAsync(verificationCode);
            return this;
        }  
        public IVerificationCodeReadOnlyRepository Build()
        {
            return _mock.Object;
        }
    }
}
