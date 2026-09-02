using Moq;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Repositories
{
    public class IRecipeWriteOnlyRepositoryBuilder
    {
        private readonly Mock<IRecipeWriteOnlyRepository> _mock;

        public IRecipeWriteOnlyRepositoryBuilder()
        {
            _mock = new Mock<IRecipeWriteOnlyRepository>();
        }
        public  IRecipeWriteOnlyRepository Build() => _mock.Object; 

        public  IRecipeWriteOnlyRepositoryBuilder DeleteById(Recipe recipe)
        {
            _mock.Setup(repository => repository.DeleteById(recipe.Id, recipe.UserId)).ReturnsAsync(true);
            return this;
        }

    }
}
