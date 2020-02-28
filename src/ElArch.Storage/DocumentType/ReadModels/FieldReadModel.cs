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
        public bool IsRequired { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        public static FieldReadModel FromDomainModel(IField field) =>
            field switch
            {
                null => throw new ArgumentNullException(nameof(field)),
                BooleanField booleanField => new BooleanFieldReadModel {FieldId = booleanField.FieldId, IsRequired = booleanField.IsRequired()},
                IntegerField integerField => new IntegerFieldReadModel {FieldId = integerField.FieldId, IsRequired = integerField.IsRequired(), MinValue = integerField.MinValue(), MaxValue = integerField.MaxValue()},
                DecimalField decimalField => new DecimalFieldReadModel {FieldId = decimalField.FieldId, IsRequired = decimalField.IsRequired(), MinValue = decimalField.MinValue(), MaxValue = decimalField.MaxValue()},
                DateTimeField dateTimeField => new DateTimeFieldReadModel {FieldId = dateTimeField.FieldId, IsRequired = dateTimeField.IsRequired(), MinValue = dateTimeField.MinValue(), MaxValue = dateTimeField.MaxValue()},
                TextField textField => new TextFieldReadModel {FieldId = textField.FieldId, IsRequired = textField.IsRequired(), MaxLength = textField.MaxLength(), MinLength = textField.MinLength()},
                _ => throw new NotSupportedException($"Field of type {field.GetType()} is not supported")
            };

        public IField ToDomainModel() => this switch
        {
            BooleanFieldReadModel booleanField => new BooleanField(booleanField.FieldId).IsRequired(booleanField.IsRequired),
            IntegerFieldReadModel integerField => new IntegerField(integerField.FieldId).IsRequired(integerField.IsRequired).MinValue(integerField.MinValue).MaxValue(integerField.MaxValue),
            DecimalFieldReadModel decimalField => new DecimalField(decimalField.FieldId).IsRequired(decimalField.IsRequired).MinValue(decimalField.MinValue).MaxValue(decimalField.MaxValue),
            DateTimeFieldReadModel dateTimeField => new DateTimeField(dateTimeField.FieldId).IsRequired(dateTimeField.IsRequired).MinValue(dateTimeField.MinValue).MaxValue(dateTimeField.MaxValue),
            TextFieldReadModel textField => new TextField(textField.FieldId).IsRequired(textField.IsRequired).MinLength(textField.MinLength).MaxLength(textField.MaxLength),
            _ => throw new NotSupportedException($"Field read model of type {GetType()} is not supported")
        };
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
            builder.HasDiscriminator<string>("ValueType")
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