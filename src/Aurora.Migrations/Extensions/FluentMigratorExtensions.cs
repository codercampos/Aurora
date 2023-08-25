using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Aurora.Migrations.Extensions;

public static class FluentMigratorExtensions
{
    public static ICreateTableColumnOptionOrWithColumnSyntax CreateWithBaseColumns(
        this ICreateTableWithColumnSyntax table)
    {
        return table.WithColumn("Id").AsInt32().Identity().PrimaryKey()
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("DeletedAt").AsDateTime().Nullable();
    }
        
    public static ICreateTableColumnOptionOrWithColumnSyntax CreateWithBaseColumnsAndLongId(
        this ICreateTableWithColumnSyntax table)
    {
        return table.WithColumn("Id").AsInt64().Identity().PrimaryKey()
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("DeletedAt").AsDateTime().Nullable();
    }
}