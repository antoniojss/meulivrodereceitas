using Moq;
using myRecipeBook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
    public class IRecipeWriteOnlyRepositoryBuilder
    {
        public static IRecipeWriteOnlyRepository Build()
        {
            var mock = new Mock<IRecipeWriteOnlyRepository>();
            return mock.Object;
        }
    }
}
