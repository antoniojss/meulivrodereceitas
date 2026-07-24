using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using myRecipeBook.Domain.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WebApi.Tests")]
namespace myRecipeBook.Infrastructure.DataAccess
{
    internal class MyRecipeBookDbContext : DbContext
    {
        public  MyRecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
       
    public DbSet<User> Users { get; set; }
    }
}
