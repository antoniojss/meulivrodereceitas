using System;
using System.Collections.Generic;
using System.Text;
using myRecipeBook.Domain.Repositories;
using Moq;
namespace CommonTestUtilities.Repositories
{
    public  class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock.Object;
        }
    }
}
