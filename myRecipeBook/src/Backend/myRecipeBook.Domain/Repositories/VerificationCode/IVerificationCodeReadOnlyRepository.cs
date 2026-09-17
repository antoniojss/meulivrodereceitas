using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories.VerificationCode
{
    public  interface IVerificationCodeReadOnlyRepository
    {
        Task<Entities.VerificationCode?> Get(Guid userId, string code, Enums.VerificationCodeType type);
    }
}
