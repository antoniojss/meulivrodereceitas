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


        public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));
        }

        public async Task<User?> GetByEmail(string email)
        {  
            //pega somente os 2 registros que ele faca match com a condição 
            //asnotracking ele rastreia objeto para ver se não mudou nada
            //copia o objeto e verifica entre as duas 
            return await _dbContext.Users
                .AsNoTracking() 
                .SingleOrDefaultAsync(user => user.Active && user.Email.Equals(email));
        }

        public async Task<bool> ExistActiveUserWithId(Guid userId)
        {
            return await _dbContext.Users.AnyAsync(user => user.Active && user.Id.Equals(userId));
        }
    }
}
