using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.ADD_HAS_IMAGE_COLUMNS, "Adding Verification Codes Table Users and Recipes")]

   public class Version0000004 : ForwardOnlyMigration   
    {
        public override void Up()    
        {
            Alter.Table("Users")
                .AddColumn("HasImage").AsBoolean().NotNullable().SetExistingRowsTo(false);
            
            Alter.Table("Recipes")
                .AddColumn("HasImage").AsBoolean().NotNullable().SetExistingRowsTo(false);
        }
    }
}
