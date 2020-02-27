using System;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.DocumentType.ReadModels
{
    public abstract class FieldReadModel
    {
        public FieldId FieldId { get; set; }
        public DocumentTypeId DocumentTypeId { get; set; }
        public string ValueType { get; set; }
        public bool IsRequired { get; set; }
        public DateTimeOffset CreationTime { get; set; }
    }

    public class BooleanFieldReadModel : FieldReadModel
    {
    }

    public abstract class ValueRangeFieldModel<T> : FieldReadModel
        where T : struct, IComparable<T>
    {
        public T? MinValue { get; set; }
        public T? MaxValue { get; set; }
    }

    public class IntegerFieldReadModel : ValueRangeFieldModel<int>
    {
    }

    public class DecimalFieldReadModel : ValueRangeFieldModel<decimal>
    {
    }

    public class DateTimeFieldReadModel : ValueRangeFieldModel<DateTimeOffset>
    {
    }

    public class TextFieldReadModel : FieldReadModel
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }

    internal sealed class FieldReadModelConfiguration : IEntityTypeConfiguration<FieldReadModel>
    {
        public void Configure(EntityTypeBuilder<FieldReadModel> builder)
        {
            builder.ToTable("Field");
            builder.HasKey(e => new {e.FieldId, e.DocumentTypeId});
            builder.Property(e => e.FieldId).HasConversion(id => id.Value, value => new FieldId(value));
            builder.Property(e => e.DocumentTypeId).HasConversion(id => id.Value, value => DocumentTypeId.With(value));
            builder.HasDiscriminator(e => e.ValueType)
                .HasValue<BooleanFieldReadModel>("Boolean")
                .HasValue<IntegerFieldReadModel>("Integer")
                .HasValue<DecimalFieldReadModel>("Decimal")
                .HasValue<DateTimeFieldReadModel>("DateTime")
                .HasValue<TextFieldReadModel>("Text");
        }
    }

    internal sealed class IntegerFieldReadModelConfiguration : IEntityTypeConfiguration<IntegerFieldReadModel>
    {
        public void Configure(EntityTypeBuilder<IntegerFieldReadModel> builder)
        {
            builder.Property(e => e.MinValue).HasColumnName("IntegerMinValue");
            builder.Property(e => e.MaxValue).HasColumnName("IntegerMaxValue");
        }
    }

    internal sealed class DecimalFieldReadModelConfiguration : IEntityTypeConfiguration<DecimalFieldReadModel>
    {
        public void Configure(EntityTypeBuilder<DecimalFieldReadModel> builder)
        {
            builder.Property(e => e.MinValue).HasColumnName("DecimalMinValue");
            builder.Property(e => e.MaxValue).HasColumnName("DecimalMaxValue");
        }
    }

    internal sealed class DateTimeFieldReadModelConfiguration : IEntityTypeConfiguration<DateTimeFieldReadModel>
    {
        public void Configure(EntityTypeBuilder<DateTimeFieldReadModel> builder)
        {
            builder.Property(e => e.MinValue).HasColumnName("DateTimeMinValue");
            builder.Property(e => e.MaxValue).HasColumnName("DateTimeMaxValue");
        }
    }
}