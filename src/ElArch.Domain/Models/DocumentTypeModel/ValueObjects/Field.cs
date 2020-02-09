using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public abstract class Field
    {
        protected Field([NotNull] FieldId fieldId)
        {
            FieldId = fieldId;
        }

        [NotNull] public FieldId FieldId { get; }
    }

    public sealed class BooleanField : Field
    {
        public BooleanField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class IntegerField : Field
    {
        public IntegerField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DecimalField : Field
    {
        public DecimalField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DateTimeField : Field
    {
        public DateTimeField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class StringField : Field
    {
        public StringField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }
}