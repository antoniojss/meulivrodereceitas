using Moq;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
    public  class IUserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _mock;
        public IUserReadOnlyRepositoryBuilder() 
        {
            _mock = new Mock<IUserReadOnlyRepository>();    
        }
        public void ExistActiveUserWithEmail(string email)
        {
            _mock.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }

        public void GetByEmail(User user)
        {
            _mock.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);
        }

        public IUserReadOnlyRepository Build()
        {
            return _mock.Object;
        }       
    }
}
