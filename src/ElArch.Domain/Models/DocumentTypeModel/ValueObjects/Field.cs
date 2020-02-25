using System;
using System.Collections.Immutable;
using System.Linq;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public interface IField
    {
        FieldId FieldId { get; }
        ImmutableList<IFieldValueValidator> Validators { get; }
    }

    public abstract class Field<TField, T> : IField where TField : Field<TField, T>
    {
        protected Field(FieldId fieldId, ImmutableList<IFieldValueValidator> validators)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
            if (validators == null) throw new ArgumentNullException(nameof(validators));
            Validators = validators.RemoveAll(v => v is FieldValueOfTypeValidator<T>).Add(new FieldValueOfTypeValidator<T>());
        }

        protected Field(FieldId fieldId) : this(fieldId, ImmutableList<IFieldValueValidator>.Empty)
        {
        }

        public FieldId FieldId { get; }
        public ImmutableList<IFieldValueValidator> Validators { get; }

        public bool IsRequired() => Validators.Any(v => v is RequiredFieldValueValidator);

        public TField IsRequired(bool value)
        {
            if (IsRequired() == value) return (TField) this;
            var validators = Validators.RemoveAll(v => v is RequiredFieldValueValidator);
            validators = value ? validators.Add(new RequiredFieldValueValidator()) : validators;
            return (TField) Activator.CreateInstance(typeof(TField), FieldId, validators);
        }
    }

    public sealed class BooleanField : Field<BooleanField, bool>
    {
        public BooleanField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public BooleanField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public abstract class ValueRangeField<TField, T> : Field<TField, T>
        where TField : ValueRangeField<TField, T>
        where T : struct, IComparable<T>
    {
        protected ValueRangeField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        protected ValueRangeField(FieldId fieldId) : base(fieldId)
        {
        }

        public T? MinValue() => Validators.OfType<FieldMinValueValidator<T>>().FirstOrDefault()?.MinValue;
        public T? MaxValue() => Validators.OfType<FieldMaxValueValidator<T>>().FirstOrDefault()?.MaxValue;

        public TField MinValue(T? minValue)
        {
            var currentMinValue = MinValue();
            if (Nullable.Equals(currentMinValue, minValue)) return (TField) this;
            var currentMaxValue = MaxValue();
            if (minValue != null && currentMaxValue?.CompareTo(minValue.Value) <= 0)
                throw new ArgumentOutOfRangeException($"Value should be less than {currentMaxValue}", nameof(minValue));
            var validators = minValue == null ? Validators.RemoveAll(v => v is FieldMinValueValidator<T>) : Validators.Add(new FieldMinValueValidator<T>(minValue.Value));
            return (TField) Activator.CreateInstance(typeof(TField), FieldId, validators);
        }

        public TField MaxValue(T? maxValue)
        {
            var currentMaxValue = MaxValue();
            if (Nullable.Equals(currentMaxValue, maxValue)) return (TField) this;
            var currentMinValue = MinValue();
            if (maxValue != null && currentMinValue?.CompareTo(maxValue.Value) >= 0)
                throw new ArgumentOutOfRangeException($"Value should be greater than {currentMinValue}", nameof(maxValue));
            var validators = maxValue == null ? Validators.RemoveAll(v => v is FieldMaxValueValidator<T>) : Validators.Add(new FieldMaxValueValidator<T>(maxValue.Value));
            return (TField) Activator.CreateInstance(typeof(TField), FieldId, validators);
        }
    }

    public sealed class IntegerField : ValueRangeField<IntegerField, int>
    {
        public IntegerField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public IntegerField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DecimalField : ValueRangeField<DecimalField, decimal>
    {
        public DecimalField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public DecimalField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DateTimeField : ValueRangeField<DateTimeField, DateTimeOffset>
    {
        public DateTimeField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public DateTimeField(FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class TextField : Field<TextField, string>
    {
        public TextField(FieldId fieldId, ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public TextField(FieldId fieldId) : base(fieldId)
        {
        }

        public int? MinLength() => Validators.OfType<FieldMinLengthValidator>().FirstOrDefault()?.MinLength;
        public int? MaxLength() => Validators.OfType<FieldMaxLengthValidator>().FirstOrDefault()?.MaxLength;

        public TextField MinLength(int? minLength)
        {
            if (minLength <= 0) throw new ArgumentOutOfRangeException(nameof(minLength));
            var currentMinLength = MinLength();
            if (Nullable.Equals(currentMinLength, minLength)) return this;
            var currentMaxLength = MaxLength();
            if (currentMaxLength <= minLength)
                throw new ArgumentOutOfRangeException($"Value should be less than {currentMaxLength}", nameof(minLength));
            var validators = minLength == null ? Validators.RemoveAll(v => v is FieldMinLengthValidator) : Validators.Add(new FieldMinLengthValidator(minLength.Value));
            return new TextField(FieldId, validators);
        }

        public TextField MaxLength(int? maxLength)
        {
            if (maxLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            var currentMaxLength = MaxLength();
            if (Nullable.Equals(currentMaxLength, maxLength)) return this;
            var currentMinLength = MinLength();
            if (currentMinLength >= maxLength)
                throw new ArgumentOutOfRangeException($"Value should be greater than {currentMinLength}", nameof(maxLength));
            var validators = maxLength == null ? Validators.RemoveAll(v => v is FieldMaxLengthValidator) : Validators.Add(new FieldMaxLengthValidator(maxLength.Value));
            return new TextField(FieldId, validators);
        }
    }
}