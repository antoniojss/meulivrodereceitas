using Microsoft.EntityFrameworkCore;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Enums;
using myRecipeBook.Domain.Repositories.VerificationCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.DataAccess.Repositories
{
    internal sealed class VerificationCodeRepository : IVerificationCodeWriteOnlyRepository, IVerificationCodeReadOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;
        public VerificationCodeRepository(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Replace(VerificationCode verificationCode)
        {
            var existingCode = await _dbContext.VerificationCodes
                .Where(vc => vc.UserId == verificationCode.UserId &&
                       vc.Type == verificationCode.Type)
                .ToListAsync();

            if (existingCode.Any())
            {
                _dbContext.VerificationCodes.RemoveRange(existingCode);
            }

            await _dbContext.VerificationCodes.AddAsync(verificationCode);

        }
        public async Task Delete(VerificationCode verificationCode)
        {
            await _dbContext.VerificationCodes
                 .Where(vc => vc.UserId == verificationCode.UserId &&
                        vc.Id == verificationCode.Id)
                 .ExecuteDeleteAsync();
        }

        public async Task<VerificationCode?> Get(Guid userId, string code, VerificationCodeType type)
        {
            return await _dbContext.VerificationCodes
                .AsNoTracking()
                .SingleOrDefaultAsync(vc =>
                       vc.UserId == userId &&
                       vc.Code == code &&
                       vc.Type == type);
        }
    }
}
