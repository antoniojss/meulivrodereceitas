using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_REFRESH_TOKENS, "Creating Refresh Tokens Table")]

   public class Version0000006  : ForwardOnlyMigration   
    {
        public override void Up()    
        {
            // Refresh Tokens
            Create.Table("RefreshTokens")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Value").AsString(255).NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefaultValue(DateTime.UtcNow)
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);

            // Foreign keys
            Create.ForeignKey("FK_RefreshTokens_User_UserId")
                  .FromTable("RefreshTokens").ForeignColumn("UserId")
                  .ToTable("Users").PrimaryColumn("Id");
        }
   
    }
}
