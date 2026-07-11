using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
