using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

// ReSharper disable ConstantConditionalAccessQualifier

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public interface IField
    {
        [NotNull] FieldId FieldId { get; }
        [NotNull] ImmutableList<IFieldValueValidator> Validators { get; }
    }

    public abstract class Field<TField, T> : IField where TField : Field<TField, T>
    {
        protected Field([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
            if (validators == null) throw new ArgumentNullException(nameof(validators));
            Validators = validators.RemoveAll(v => v is FieldValueOfTypeValidator<T>).Add(new FieldValueOfTypeValidator<T>());
        }

        protected Field([NotNull] FieldId fieldId) : this(fieldId, ImmutableList<IFieldValueValidator>.Empty)
        {
        }

        public FieldId FieldId { get; }
        public ImmutableList<IFieldValueValidator> Validators { get; }

        public bool IsRequired() => Validators.Any(v => v is RequiredFieldValueValidator);

        [NotNull]
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
        [JsonConstructor]
        private BooleanField([NotNull] FieldId fieldId, [NotNull] IEnumerable<IFieldValueValidator> validators)
            : this(fieldId, validators.ToImmutableList())
        {
        }

        public BooleanField([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators)
            : base(fieldId, validators)
        {
        }

        public BooleanField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public abstract class ValueRangeField<TField, T> : Field<TField, T>
        where TField : ValueRangeField<TField, T>
        where T : struct, IComparable<T>
    {
        protected ValueRangeField([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators)
            : base(fieldId, validators)
        {
        }

        protected ValueRangeField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }

        public T? MinValue() => Validators.OfType<FieldMinValueValidator<T>>().FirstOrDefault()?.MinValue;
        public T? MaxValue() => Validators.OfType<FieldMaxValueValidator<T>>().FirstOrDefault()?.MaxValue;

        [NotNull]
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

        [NotNull]
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
        [JsonConstructor]
        private IntegerField(FieldId fieldId, IEnumerable<IFieldValueValidator> validators) : this(fieldId, validators.ToImmutableList())
        {
        }

        public IntegerField([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public IntegerField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DecimalField : ValueRangeField<DecimalField, decimal>
    {
        [JsonConstructor]
        private DecimalField(FieldId fieldId, IEnumerable<IFieldValueValidator> validators) : this(fieldId, validators.ToImmutableList())
        {
        }

        public DecimalField([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public DecimalField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public sealed class DateTimeField : ValueRangeField<DateTimeField, DateTimeOffset>
    {
        [JsonConstructor]
        private DateTimeField(FieldId fieldId, IEnumerable<IFieldValueValidator> validators) : this(fieldId, validators.ToImmutableList())
        {
        }

        public DateTimeField([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public DateTimeField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }
    }

    public class TextField : Field<TextField, string>
    {
        [JsonConstructor]
        private TextField(FieldId fieldId, IEnumerable<IFieldValueValidator> validators) : this(fieldId, validators.ToImmutableList())
        {
        }

        public TextField([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public TextField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }

        public int? MinLength() => Validators.OfType<FieldMinLengthValidator>().FirstOrDefault()?.MinLength;
        public int? MaxLength() => Validators.OfType<FieldMaxLengthValidator>().FirstOrDefault()?.MaxLength ?? MaxAllowedLength;

        public virtual int? MaxAllowedLength => null;

        [NotNull]
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

        [NotNull]
        public TextField MaxLength(int? maxLength)
        {
            if (maxLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (maxLength >= MaxAllowedLength) throw new ArgumentException($"Value exceeds max allowed length {MaxAllowedLength}", nameof(maxLength));
            var currentMaxLength = MaxLength();
            if (Nullable.Equals(currentMaxLength, maxLength)) return this;
            var currentMinLength = MinLength();
            if (currentMinLength >= maxLength)
                throw new ArgumentOutOfRangeException($"Value should be greater than {currentMinLength}", nameof(maxLength));
            var validators = maxLength == null ? Validators.RemoveAll(v => v is FieldMaxLengthValidator) : Validators.Add(new FieldMaxLengthValidator(maxLength.Value));
            return new TextField(FieldId, validators);
        }
    }

    public sealed class StringField : TextField
    {
        [JsonConstructor]
        private StringField(FieldId fieldId, IEnumerable<IFieldValueValidator> validators) : this(fieldId, validators.ToImmutableList())
        {
        }

        public StringField([NotNull] FieldId fieldId, [NotNull] ImmutableList<IFieldValueValidator> validators) : base(fieldId, validators)
        {
        }

        public StringField([NotNull] FieldId fieldId) : base(fieldId)
        {
        }

        public override int? MaxAllowedLength => 500;
    }
}