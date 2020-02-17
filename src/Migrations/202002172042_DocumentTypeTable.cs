using FluentMigrator;

namespace Migrations
{
    [TimestampedMigration(2020, 2, 17, 20, 42)]
    public sealed class DocumentTypeTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("DocumentType")
                .WithColumn("Id").AsString(256).PrimaryKey()
                .WithColumn("Name").AsString(512).Indexed()
                .WithColumn("CreationTime").AsDateTimeOffset().NotNullable()
                .WithColumn("ModificationTime").AsDateTimeOffset().NotNullable()
                .WithColumn("Version").AsInt32().NotNullable();
        }
    }
}