using System;

#nullable enable

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public abstract class Field
    {
        protected Field(FieldId fieldId)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }

        public FieldId FieldId { get; }
    }

    public sealed class BooleanField : Field
    {
        public BooleanField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class IntegerField : Field
    {
        public IntegerField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DecimalField : Field
    {
        public DecimalField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DateTimeField : Field
    {
        public DateTimeField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class StringField : Field
    {
        public StringField(FieldId fieldId) : base(fieldId)
        {
        }
    }
}