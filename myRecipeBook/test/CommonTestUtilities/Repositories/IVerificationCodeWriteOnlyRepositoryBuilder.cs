using Moq;
using myRecipeBook.Domain.Repositories.VerificationCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
    public  class  IVerificationCodeWriteOnlyRepositoryBuilder
    {
        public static IVerificationCodeWriteOnlyRepository Build()
        {
            var mock = new Mock<IVerificationCodeWriteOnlyRepository>();    

            return mock.Object; 
        }

    }
}
