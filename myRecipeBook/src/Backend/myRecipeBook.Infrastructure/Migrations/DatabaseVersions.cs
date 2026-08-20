using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.Migrations
{
    internal abstract class DatabaseVersions
    {
        internal const int TABLE_USERS = 1; 
        internal const int TABLE_RECIPES = 2; 
    }
}
