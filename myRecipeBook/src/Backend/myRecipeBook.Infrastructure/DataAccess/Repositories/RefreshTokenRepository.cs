using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.RefreshToken;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace myRecipeBook.Infrastructure.DataAccess.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenWriteOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public RefreshTokenRepository(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Replace(RefreshToken refreshToken)
        {
            var existingRefreshToken = await _dbContext.RefreshTokens
                .Where(rt => rt.UserId == refreshToken.UserId)
                .ToListAsync();

            _dbContext.RefreshTokens.RemoveRange(existingRefreshToken);
            await _dbContext.RefreshTokens.AddAsync(refreshToken);


        }
    }
}
