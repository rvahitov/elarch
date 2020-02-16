using System;
using System.Collections.Immutable;
using System.Linq;

#nullable enable

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public interface IField
    {
        FieldId FieldId { get; }
        ImmutableList<IFieldValueValidator> Validators { get; }
    }

    public abstract class Field<TField, T> : IField
        where TField : Field<TField, T>
    {
        protected Field(FieldId fieldId, ImmutableList<IFieldValueValidator> validators)
        {
            if (validators == null) throw new ArgumentNullException(nameof(validators));
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
            Validators = validators.RemoveAll(v => v is ValueOfTypeValidator<T>)
                .Add(new ValueOfTypeValidator<T>());
        }

        protected Field(FieldId fieldId)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
            Validators = ImmutableList.Create<IFieldValueValidator>(new ValueOfTypeValidator<T>());
        }

        public FieldId FieldId { get; }
        public ImmutableList<IFieldValueValidator> Validators { get; }

        public bool IsRequired() => Validators.Any(v => v is FieldRequiredValueValidator);

        public TField IsRequired(bool value)
        {
            if (IsRequired() && value) return (TField) this;
            var validators = value
                ? Validators.Add(new FieldRequiredValueValidator())
                : Validators.RemoveAll(v => v is FieldRequiredValueValidator);
            return (TField) Activator.CreateInstance(typeof(TField), FieldId, validators);
        }
    }

    public sealed class BooleanField : Field<BooleanField, bool>
    {
        public BooleanField(FieldId fieldId) : base(fieldId)
        {
        }

        public BooleanField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }
    }

    public abstract class MinMaxValueField<TField, T> : Field<TField, T>
        where TField : MinMaxValueField<TField, T>
        where T : struct, IComparable<T>
    {
        protected MinMaxValueField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        protected MinMaxValueField(FieldId fieldId) : base(fieldId)
        {
        }

        public T? MinValue() => Validators.OfType<FieldMinValueValidator<T>>().FirstOrDefault()?.MinValue;
        public T? MaxValue() => Validators.OfType<FieldMaxValueValidator<T>>().FirstOrDefault()?.MaxValue;

        public TField MinValue(T? value)
        {
            if (value != null && MaxValue() != null && Nullable.Compare(value, MaxValue()) >= 0)
                throw new ArgumentException("MinValue should be less than MaxValue", nameof(value));
            if (Nullable.Equals(MinValue(), value)) return (TField) this;
            var validators = value == null
                ? Validators.RemoveAll(v => v is FieldMinValueValidator<T>)
                : Validators.Add(new FieldMinValueValidator<T>(value.Value));
            return (TField) Activator.CreateInstance(typeof(TField), FieldId, validators);
        }

        public TField MaxValue(T? value)
        {
            if (value != null && MinValue() != null && Nullable.Compare(value, MinValue()) <= 0)
                throw new ArgumentException("MaxValue should be greater than MinValue", nameof(value));
            if (Nullable.Equals(value, MaxValue())) return (TField) this;
            var validators = value == null
                ? Validators.RemoveAll(v => v is FieldMaxValueValidator<T>)
                : Validators.Add(new FieldMaxValueValidator<T>(value.Value));
            return (TField) Activator.CreateInstance(typeof(TField), FieldId, validators);
        }
    }

    public sealed class IntegerField : MinMaxValueField<IntegerField, int>
    {
        public IntegerField(FieldId fieldId) : base(fieldId)
        {
        }

        public IntegerField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }
    }

    public sealed class DecimalField : MinMaxValueField<DecimalField, decimal>
    {
        public DecimalField(FieldId fieldId) : base(fieldId)
        {
        }

        public DecimalField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }
    }

    public sealed class DateTimeField : MinMaxValueField<DateTimeField, DateTimeOffset>
    {
        public DateTimeField(FieldId fieldId) : base(fieldId)
        {
        }

        public DateTimeField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }
    }

    public sealed class TextField : Field<StringField, string>
    {
        public TextField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public TextField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class StringField : Field<StringField, string>
    {
        public StringField(FieldId fieldId) : base(fieldId)
        {
        }

        public StringField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public int? MinLength() => Validators.OfType<FieldValueMinLengthValidator>().FirstOrDefault()?.MinLength;
        public int? MaxLength() => Validators.OfType<FieldValueMaxLengthValidator>().FirstOrDefault()?.MaxLength;

        public StringField MinLength(int? value)
        {
            if (Nullable.Compare(value, MaxLength()) >= 0)
                throw new ArgumentException("MinValue should be less than MaxValue", nameof(value));
            if (value == MinLength()) return this;
            var validators = value == null
                ? Validators.RemoveAll(v => v is FieldValueMinLengthValidator)
                : Validators.Add(new FieldValueMinLengthValidator(value.Value));
            return new StringField(FieldId, validators);
        }

        public StringField MaxLength(int? value)
        {
            if (Nullable.Compare(value, MinLength()) <= 0)
                throw new ArgumentException("MaxValue should be greater than MinValue", nameof(value));
            if (value == MaxLength()) return this;
            var validators = value == null
                ? Validators.RemoveAll(v => v is FieldValueMaxLengthValidator)
                : Validators.Add(new FieldValueMaxLengthValidator(value.Value));
            return new StringField(FieldId, validators);
        }
    }
}