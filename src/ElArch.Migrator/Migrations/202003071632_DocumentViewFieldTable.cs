using System.Data;
using FluentMigrator;

namespace ElArch.Migrator.Migrations
{
    [TimestampedMigration(2020, 3, 7, 16, 32)]
    public sealed class DocumentViewFieldTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("DocumentViewField")
                .WithColumn("DocumentViewId").AsInt32().NotNullable().Indexed()
                .WithColumn("FieldId").AsString(128).NotNullable().Indexed()
                .WithColumn("ViewType").AsString(10).NotNullable().Indexed()
                .WithColumn("Title").AsString(128).NotNullable()
                .WithColumn("ViewOrder").AsInt32().NotNullable()
                .WithColumn("IsReadOnly").AsBoolean().Nullable();

            Create.PrimaryKey().OnTable("DocumentViewField").Columns("DocumentViewId", "FieldId");

            Create.ForeignKey()
                .FromTable("DocumentViewField").ForeignColumn("DocumentViewId")
                .ToTable("DocumentView").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.Index()
                .OnTable("DocumentViewField").WithOptions().Unique()
                .OnColumn("DocumentViewId").Ascending()
                .OnColumn("FieldId").Ascending()
                .OnColumn("ViewType").Ascending();
        }
    }
}