using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using myRecipeBook.Domain.Entities;
namespace myRecipeBook.Infrastructure.DataAccess
{
    internal class MyRecipeBookDbContext : DbContext
    {
        public MyRecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
       
    public DbSet<User> Users { get; set; }
    }
}
