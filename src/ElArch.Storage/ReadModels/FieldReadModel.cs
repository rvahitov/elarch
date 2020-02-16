#nullable enable
using System;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.ReadModels
{
    public abstract class FieldReadModel
    {
        protected FieldReadModel(DocumentTypeId documentTypeId, FieldId fieldId)
        {
            DocumentTypeId = documentTypeId ?? throw new ArgumentNullException(nameof(documentTypeId));
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }

        public DocumentTypeId DocumentTypeId { get; }
        public FieldId FieldId { get; }

        public bool? IsRequired { get; set; }
    }

    public sealed class BooleanFieldReadModel : FieldReadModel
    {
        public BooleanFieldReadModel(DocumentTypeId documentTypeId, FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }
    }

    public sealed class IntegerFieldReadModel : FieldReadModel
    {
        public IntegerFieldReadModel(DocumentTypeId documentTypeId, FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }

    public sealed class DecimalFieldReadModel : FieldReadModel
    {
        public DecimalFieldReadModel(DocumentTypeId documentTypeId, FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }

        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }

    public sealed class DateTimeFieldReadModel : FieldReadModel
    {
        public DateTimeFieldReadModel(DocumentTypeId documentTypeId, FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }

        public DateTimeOffset? MinValue { get; set; }
        public DateTimeOffset? MaxValue { get; set; }
    }

    public sealed class TextFieldReadModel : FieldReadModel
    {
        public TextFieldReadModel(DocumentTypeId documentTypeId, FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }
    }

    public sealed class StringFieldReadModel : FieldReadModel
    {
        public StringFieldReadModel(DocumentTypeId documentTypeId, FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }

        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }

    internal sealed class FieldReadModelConfiguration : IEntityTypeConfiguration<FieldReadModel>
    {
        public void Configure(EntityTypeBuilder<FieldReadModel> builder)
        {
            builder.ToTable("Field");
            builder.HasKey(e => new {e.DocumentTypeId, e.FieldId});
            builder.Property(e => e.DocumentTypeId)
                .HasConversion(id => id.Value, value => DocumentTypeId.With(value));
            builder.Property(e => e.FieldId)
                .HasConversion(id => id.Value, value => new FieldId(value));

            builder.HasDiscriminator<string>("FieldType")
                .HasValue<BooleanFieldReadModel>("Boolean")
                .HasValue<IntegerFieldReadModel>("Integer")
                .HasValue<DecimalFieldReadModel>("Decimal")
                .HasValue<DateTimeFieldReadModel>("DateTime")
                .HasValue<TextFieldReadModel>("Text")
                .HasValue<StringFieldReadModel>("String");
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