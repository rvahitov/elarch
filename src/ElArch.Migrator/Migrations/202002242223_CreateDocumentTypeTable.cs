using FluentMigrator;

namespace ElArch.Migrator.Migrations
{
    [TimestampedMigration(2020, 2, 24, 22, 23)]
    public sealed class CreateDocumentTypeTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("DocumentType")
                .WithColumn("Id").AsString(256).PrimaryKey()
                .WithColumn("Name").AsString(128).NotNullable()
                .WithColumn("CreationTime").AsDateTimeOffset().NotNullable()
                .WithColumn("ModificationTime").AsDateTimeOffset().NotNullable()
                .WithColumn("SequenceNumber").AsInt32().Identity()
                .WithColumn("Version").AsInt32().NotNullable();
        }
    }
}