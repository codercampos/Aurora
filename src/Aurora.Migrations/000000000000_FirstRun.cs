using FluentMigrator;

namespace Aurora.Migrations
{
    [Migration(000000000000)]
    public class _000000000000_FirstRun : Migration
    {
        public override void Down()
        {
            Delete.Table("Users");
        }

        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Test").AsInt32().Identity().NotNullable();
        }
    }
}
