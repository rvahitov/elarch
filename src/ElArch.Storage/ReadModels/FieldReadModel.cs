using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.ReadModels
{
    public abstract class FieldReadModel
    {
        protected FieldReadModel([NotNull] DocumentTypeId documentTypeId, [NotNull] FieldId fieldId)
        {
            DocumentTypeId = documentTypeId;
            FieldId = fieldId;
        }

        [NotNull] public DocumentTypeId DocumentTypeId { get; }
        [NotNull] public FieldId FieldId { get; }
    }

    public sealed class BooleanFieldReadModel : FieldReadModel
    {
        public BooleanFieldReadModel([NotNull] DocumentTypeId documentTypeId, [NotNull] FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }
    }

    public sealed class IntegerFieldReadModel : FieldReadModel
    {
        public IntegerFieldReadModel([NotNull] DocumentTypeId documentTypeId, [NotNull] FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }
    }

    public sealed class DecimalFieldReadModel : FieldReadModel
    {
        public DecimalFieldReadModel([NotNull] DocumentTypeId documentTypeId, [NotNull] FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }
    }

    public sealed class DateTimeFieldReadModel : FieldReadModel
    {
        public DateTimeFieldReadModel([NotNull] DocumentTypeId documentTypeId, [NotNull] FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }
    }

    public sealed class StringFieldReadModel : FieldReadModel
    {
        public StringFieldReadModel([NotNull] DocumentTypeId documentTypeId, [NotNull] FieldId fieldId) : base(documentTypeId, fieldId)
        {
        }
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
                .HasValue<StringFieldReadModel>("String");
        }
    }
}