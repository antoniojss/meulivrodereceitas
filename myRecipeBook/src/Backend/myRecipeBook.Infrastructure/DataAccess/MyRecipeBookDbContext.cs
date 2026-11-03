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
        public DbSet<VerificationCode> VerificationCodes { get; set; }
         
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecipeDishType>()
                .ToTable("RecipeDishTypes")
                .Property(dishType => dishType.Type).HasConversion<string>();

            // para informar ao entity framework que o valor do enum não será gerado
            // automaticamente, pois ele é definido usuario(Guid Id)

            modelBuilder.Entity<RecipeDishType>()
              .ToTable("RecipeDishTypes")
              .Property(dishType => dishType.Id).ValueGeneratedNever();

            modelBuilder.Entity<RecipeIngredient>()
                .ToTable("RecipeIngredients")
                .Property(ingredient => ingredient.Id).ValueGeneratedNever();

            modelBuilder.Entity<RecipeInstruction>()
                .ToTable("RecipeInstructions")
                .Property(instruction  => instruction.Id).ValueGeneratedNever();

            modelBuilder.Entity<Recipe>().Property(recipe => recipe.CookTime).HasConversion<string>();

            modelBuilder.Entity<Recipe>().HasOne<User>().WithMany().HasForeignKey(recipe => recipe.UserId);

            modelBuilder.Entity<VerificationCode>().Property(verificationCode => verificationCode.Type).HasConversion<string>();

            modelBuilder.Entity<VerificationCode>().HasOne<User>().WithMany().HasForeignKey(verificationCode => verificationCode.UserId);

            modelBuilder.Entity<RefreshToken>().HasOne<User>().WithMany().HasForeignKey(refreshToken => refreshToken.UserId);
        }
    }
}
