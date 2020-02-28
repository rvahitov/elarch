using FluentMigrator;

namespace ElArch.Migrator.Migrations
{
    [TimestampedMigration(2020, 2, 28, 14, 21)]
    public sealed class FieldTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Field")
                .WithColumn("FieldId").AsString(128).NotNullable().Indexed()
                .WithColumn("DocumentTypeId").AsString(256).NotNullable().Indexed()
                .WithColumn("ValueType").AsString(50).NotNullable().Indexed()
                .WithColumn("IsRequired").AsBoolean().NotNullable()
                .WithColumn("IntegerMinValue").AsInt32().Nullable()
                .WithColumn("IntegerMaxValue").AsInt32().Nullable()
                .WithColumn("DecimalMinValue").AsDecimal().Nullable()
                .WithColumn("DecimalMaxValue").AsDecimal().Nullable()
                .WithColumn("DateTimeMinValue").AsDateTimeOffset().Nullable()
                .WithColumn("DateTimeMaxValue").AsDateTimeOffset().Nullable()
                .WithColumn("MinLength").AsInt32().Nullable()
                .WithColumn("MaxLength").AsInt32().Nullable()
                .WithColumn("CreationTime").AsDateTimeOffset().NotNullable();
        }
    }
}