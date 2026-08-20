using Moq;
using myRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
    public class IUserUpdateOnlyRepositoryBuilder
    {

        public static IUserUpdateOnlyRepository Build()
        {
            var mock = new Mock<IUserUpdateOnlyRepository>();
            return mock.Object;
        }
    }
}
