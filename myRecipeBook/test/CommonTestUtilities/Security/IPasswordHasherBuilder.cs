using Moq;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.PasswordHashing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Security
{
    public class IPasswordHasherBuilder
    {
        private readonly Mock<IPasswordHasher> _mock;

        public IPasswordHasherBuilder()
        {
            _mock = new Mock<IPasswordHasher>();

            _mock.Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>())).Returns("hashed_password");
        }
        public void VerifyPassword(string password)
        {
            _mock.Setup(repository => repository.VerifyPassword(password, It.IsAny<string>())).Returns(true);
        }
        public IPasswordHasher Build()
        {
            return _mock.Object;
        }
    }
}
