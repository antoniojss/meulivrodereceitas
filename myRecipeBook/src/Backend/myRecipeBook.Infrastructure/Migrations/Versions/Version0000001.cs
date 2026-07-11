using System;
using System.Collections.Generic;
using System.Text;
using FluentMigrator;
namespace myRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USERS,"Create Users table")]
    public class Version0000001 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(250).NotNullable()
                .WithColumn("Email").AsString(250).NotNullable()
                .WithColumn("Password").AsString(2000).NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);
        }

        /* usando a forword migration não é necessário implementar o down, pois não é possível reverter a migração, 
         * caso seja necessário reverter a migração, 
         * deve-se criar uma nova migração que reverta as alterações feitas na migração anterior.
        public override void Down()
        {
            throw new NotImplementedException();
        }
        */

    }
}
