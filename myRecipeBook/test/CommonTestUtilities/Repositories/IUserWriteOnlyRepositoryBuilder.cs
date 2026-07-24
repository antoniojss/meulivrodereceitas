using Moq;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
    public  class IUserWriteOnlyRepositoryBuilder
    {

        public static IUserWriteOnlyRepository Build()
        {
            var mock = new Mock<IUserWriteOnlyRepository>();
            return mock.Object;
        }
    }
}
