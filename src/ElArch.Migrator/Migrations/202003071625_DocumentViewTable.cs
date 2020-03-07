using System.Data;
using FluentMigrator;

namespace ElArch.Migrator.Migrations
{
    [TimestampedMigration(2020, 3, 3, 16, 25)]
    public sealed class DocumentViewTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("DocumentView")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("DocumentTypeId").AsString(256).NotNullable().Indexed()
                .WithColumn("ViewType").AsString(10).NotNullable().Indexed();
            
            Create.ForeignKey()
                .FromTable("DocumentView").ForeignColumn("DocumentTypeId")
                .ToTable("DocumentType").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
            
            Create.Index()
                .OnTable("DocumentView")
                .WithOptions().Unique()
                .OnColumn("DocumentTypeId").Ascending()
                .OnColumn("ViewType").Ascending();
        }
    }
}