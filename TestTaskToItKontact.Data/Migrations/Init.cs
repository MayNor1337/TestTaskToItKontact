using FluentMigrator;

namespace TestTaskToItKontact.Data.Migrations;

[Migration(1)]
public sealed class Init : Migration
{
    public override void Up()
    {
        Create.Table("StoredFile")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("FileData").AsBinary(int.MaxValue).NotNullable()
            .WithColumn("CreatedAt").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
    }

    public override void Down()
    {
        Delete.Table("StoredFile");
    }
}