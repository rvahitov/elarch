using FluentMigrator;

namespace ElArch.Migrator.Migrations
{
    [TimestampedMigration(2020, 2, 24, 22, 23)]
    public sealed class CreateDocumentTypeTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("DocumentType")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("AggregateId").AsString(256).NotNullable().Unique()
                .WithColumn("Name").AsString(128).NotNullable()
                .WithColumn("CreationTime").AsDateTimeOffset().NotNullable()
                .WithColumn("ModificationTime").AsDateTimeOffset().NotNullable()
                .WithColumn("Version").AsInt32().NotNullable();
        }
    }
}