using System;
using System.Data;
using FluentMigrator;

namespace myRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_RECIPES, "Create Recipes and related tables")]
    public class Version0000002 : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Recipes
            Create.Table("Recipes")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Title").AsString(500).NotNullable()
                .WithColumn("CookTime").AsString(50).NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);

            // RecipeIngredients
            Create.Table("RecipeIngredients")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Item").AsString(500).NotNullable()
                .WithColumn("RecipeId").AsGuid().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);

            // RecipeInstructions
            Create.Table("RecipeInstructions")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Order").AsInt32().NotNullable()
                .WithColumn("Description").AsString(2000).NotNullable()
                .WithColumn("RecipeId").AsGuid().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);

            // RecipeDishTypes
            Create.Table("RecipeDishTypes")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Type").AsString(50).NotNullable()
                .WithColumn("RecipeId").AsGuid().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);

            // Foreign keys
            Create.ForeignKey("FK_Recipes_User_UserId")
                .FromTable("Recipes").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");
            
            Create.ForeignKey("FK_RecipeIngredients_Recipe_RecipeId")
                .FromTable("RecipeIngredients").ForeignColumn("RecipeId")
                .ToTable("Recipes").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_RecipeInstructions_Recipe_RecipeId")
                .FromTable("RecipeInstructions").ForeignColumn("RecipeId")
                .ToTable("Recipes").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_RecipeDishTypes_Recipe_RecipeId")
                .FromTable("RecipeDishTypes").ForeignColumn("RecipeId")
                .ToTable("Recipes").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }
    }
}
