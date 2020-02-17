using FluentMigrator;

namespace Migrations
{
    [TimestampedMigration(2020, 2, 17, 20, 56)]
    public sealed class FieldTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Field")
                .WithColumn("FieldId").AsString(128).NotNullable()
                .WithColumn("DocumentId").AsString(256).NotNullable()
                .WithColumn("FieldType").AsString(20).NotNullable().Indexed()
                .WithColumn("IsRequired").AsBoolean()
                .WithColumn("IntegerMinValue").AsInt32().Nullable()
                .WithColumn("IntegerMaxValue").AsInt32().Nullable()
                .WithColumn("DecimalMinValue").AsDecimal().Nullable()
                .WithColumn("DecimalMaxValue").AsDecimal().Nullable()
                .WithColumn("DateTimeMinValue").AsDateTimeOffset().Nullable()
                .WithColumn("DateTimeMaxValue").AsDateTimeOffset().Nullable()
                .WithColumn("MinLength").AsInt32().Nullable()
                .WithColumn("MaxLength").AsInt32().Nullable();

            Create.PrimaryKey().OnTable("Field")
                .Columns("FieldId", "DocumentId");

            Create.ForeignKey()
                .FromTable("Field").ForeignColumn("DocumentId")
                .ToTable("DocumentType").PrimaryColumn("Id");
        }
    }
}