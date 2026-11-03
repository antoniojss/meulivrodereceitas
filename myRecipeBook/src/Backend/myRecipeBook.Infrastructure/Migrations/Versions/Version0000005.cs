using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.INCREASE_VERIFICATION_CODE_LENGTH, "Increasing Verification Code Length")]

   public class Version0000005   : ForwardOnlyMigration   
    {
        public override void Up()    
        {
            Alter.Table("VerificationCodes")
                .AlterColumn("Code").AsString(255).NotNullable();
        }
    }
}
