using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace myRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_VERIFICATION_CODES, "Create Verification Codes Table")]
    public class Version0000003 : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Verification Codes
            Create.Table("VerificationCodes")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Code").AsString(6).NotNullable()
                .WithColumn("Type").AsString(50).NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefaultValue(DateTime.UtcNow)
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);

            // Foreign keys
            Create.ForeignKey("FK_VerificationCodes_User_UserId")
                  .FromTable("VerificationCodes").ForeignColumn("UserId")
                  .ToTable("Users").PrimaryColumn("Id");
        }
    }
}