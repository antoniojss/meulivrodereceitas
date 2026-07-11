using Microsoft.EntityFrameworkCore;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.User;
namespace myRecipeBook.Infrastructure.DataAccess.Repositories
{

    internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public UserRepository(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //=> vai tocar { await _dbContext.Users.AddAsync(user); }

        public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));
        }
    }
}
