using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.VerificationCode
{
    public interface IVerificationCodeWriteOnlyRepository
    {
        Task Replace(Entities.VerificationCode verificationCode);
        Task Delete(Entities.VerificationCode verificationCode);
    }
}
