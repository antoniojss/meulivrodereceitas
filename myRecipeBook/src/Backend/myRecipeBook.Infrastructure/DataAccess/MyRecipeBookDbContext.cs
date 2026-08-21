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
        public MyRecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecipeDishType>()
                .ToTable("RecipeDishTypes")
                .Property(dishType => dishType.Type).HasConversion<string>();

            modelBuilder.Entity<RecipeIngredient>().ToTable("RecipeIngredients");
            modelBuilder.Entity<RecipeInstruction>().ToTable("RecipeInstructions");

            modelBuilder.Entity<Recipe>().Property(recipe => recipe.CookTime).HasConversion<string>();

        }
    }
}
