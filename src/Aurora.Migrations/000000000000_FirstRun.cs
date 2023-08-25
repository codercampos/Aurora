using Aurora.Migrations.Extensions;
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
            // Security related tables
            CreateRolesTable();
            CreateUsersTable();
            CreateUserRolesTable();
        }

        private void CreateRolesTable()
        {
            Create.Table("Roles")
                .CreateWithBaseColumns()
                .WithColumn("Name").AsString(200).NotNullable()
                .WithColumn("Description").AsString(500).Nullable();
        }

        private void CreateUsersTable()
        {
            Create.Table("Users")
                .CreateWithBaseColumns()
                .WithColumn("UserName").AsString().NotNullable().Unique()
                .WithColumn("Password").AsString(int.MaxValue).NotNullable()
                .WithColumn("FirstName").AsString(int.MaxValue).NotNullable()
                .WithColumn("LastName").AsString(int.MaxValue).NotNullable()
                .WithColumn("Email").AsString(int.MaxValue).NotNullable()
                .WithColumn("PhoneNumber").AsString().Nullable()
                .WithColumn("TimeZone").AsString().NotNullable()
                .WithColumn("LastLogin").AsDateTime().Nullable()
                .WithColumn("Salt").AsString().NotNullable();
        }

        private void CreateUserRolesTable()
        {
            Create.Table("UserRoles")
                .WithColumn("UserId").AsInt32().PrimaryKey()
                .WithColumn("RoleId").AsInt32().PrimaryKey();

            Create.ForeignKey()
                .FromTable("UserRoles").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("UserRoles").ForeignColumn("RoleId")
                .ToTable("Roles").PrimaryColumn("Id");
        }
    }
}
