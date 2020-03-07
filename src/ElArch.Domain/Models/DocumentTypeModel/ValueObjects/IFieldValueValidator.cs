#nullable enable
using System;
using JetBrains.Annotations;
using OneOf;
using OneOf.Types;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public interface IFieldValueValidator
    {
        OneOf<object?, Error<string>> Validate([NotNull] FieldId fieldId, [CanBeNull] object? value);
    }

    public sealed class RequiredFieldValueValidator : IFieldValueValidator
    {
        public OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            if (fieldId == null) throw new ArgumentNullException(nameof(fieldId));
            return value is null
                ? OneOf<object?, Error<string>>.FromT1(new Error<string>($"Value for field {fieldId} is required"))
                : OneOf<object?, Error<string>>.FromT0(value);
        }
    }

    public sealed class FieldValueOfTypeValidator<T> : IFieldValueValidator
    {
        public OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            if (value is null || value is T) return OneOf<object?, Error<string>>.FromT0(value);
            return OneOf<object?, Error<string>>.FromT1(new Error<string>($"Value for field {fieldId} should be of type {typeof(T)}"));
        }
    }

    public sealed class FieldMinValueValidator<T> : IFieldValueValidator
        where T : struct, IComparable<T>
    {
        public FieldMinValueValidator(T minValue)
        {
            MinValue = minValue;
        }

        public T MinValue { get; }

        public OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            if (value is null || !(value is T tValue) || MinValue.CompareTo(tValue) <= 0)
                return OneOf<object?, Error<string>>.FromT0(value);
            return OneOf<object?, Error<string>>.FromT1(new Error<string>($"Value for field {fieldId} should be greater than or equal to {MinValue}"));
        }
    }

    public sealed class FieldMaxValueValidator<T> : IFieldValueValidator
        where T : struct, IComparable<T>
    {
        public FieldMaxValueValidator(T maxValue)
        {
            MaxValue = maxValue;
        }

        public T MaxValue { get; }

        public OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            if (value is null || !(value is T tValue) || MaxValue.CompareTo(tValue) >= 0)
                return OneOf<object?, Error<string>>.FromT0(value);
            return OneOf<object?, Error<string>>.FromT1(new Error<string>($"Value for field {fieldId} should be less than or equal to {MaxValue}"));
        }
    }

    public sealed class FieldMinLengthValidator : IFieldValueValidator
    {
        public FieldMinLengthValidator(int minLength)
        {
            if (minLength <= 0) throw new ArgumentOutOfRangeException(nameof(minLength));
            MinLength = minLength;
        }

        public int MinLength { get; }

        public OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            if (value is null || !(value is string sValue) || sValue.Length >= MinLength)
                return OneOf<object?, Error<string>>.FromT0(value);
            return OneOf<object?, Error<string>>.FromT1(new Error<string>($"Value for field {fieldId} should contain minimum {MinLength} characters"));
        }
    }

    public sealed class FieldMaxLengthValidator : IFieldValueValidator
    {
        public FieldMaxLengthValidator(int maxLength)
        {
            if (maxLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            MaxLength = maxLength;
        }

        public int MaxLength { get; }

        public OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            if (value is null || !(value is string sValue) || sValue.Length <= MaxLength)
                return OneOf<object?, Error<string>>.FromT0(value);
            return OneOf<object?, Error<string>>.FromT1(new Error<string>($"Value for field {fieldId} should contain maximum {MaxLength} characters"));
        }
    }
}