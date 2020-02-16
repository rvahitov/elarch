#nullable enable
using System;
using OneOf;
using OneOf.Types;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public interface IFieldValueValidator
    {
        OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value);
    }

    public abstract class AbstractFieldValueValidator : IFieldValueValidator
    {
        protected OneOf<object?, Error<string>> Success(object? value) =>
            OneOf<object?, Error<string>>.FromT0(value);

        protected OneOf<object?, Error<string>> Failure(string message) =>
            OneOf<object?, Error<string>>.FromT1(new Error<string>(message));

        public abstract OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value);
    }

    public sealed class FieldRequiredValueValidator : AbstractFieldValueValidator
    {
        public override OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            return value is null
                ? Failure($"Value for field {fieldId} is required")
                : Success(value);
        }
    }

    public sealed class ValueOfTypeValidator<T> : AbstractFieldValueValidator
    {
        public override OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            return value is null || value is T
                ? Success(value)
                : Failure($"Value for field {fieldId} should be of type {nameof(T)}");
        }
    }

    public sealed class FieldMinValueValidator<T> : AbstractFieldValueValidator
        where T : struct, IComparable<T>
    {
        public T MinValue { get; }

        public FieldMinValueValidator(T minValue)
        {
            MinValue = minValue;
        }

        public override OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            return value is null || !(value is T tValue) || MinValue.CompareTo(tValue) <= 0
                ? Success(value)
                : Failure($"Value for field {fieldId} should not be less than {MinValue}");
        }
    }

    public sealed class FieldMaxValueValidator<T> : AbstractFieldValueValidator
        where T : struct, IComparable<T>
    {
        public T MaxValue { get; }

        public FieldMaxValueValidator(T maxValue)
        {
            MaxValue = maxValue;
        }

        public override OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            return value is null || !(value is T tValue) || MaxValue.CompareTo(tValue) >= 0
                ? Success(value)
                : Failure($"Value for field {fieldId} should not be greater than {MaxValue}");
        }
    }

    public sealed class FieldValueMinLengthValidator : AbstractFieldValueValidator
    {
        public int MinLength { get; }

        public FieldValueMinLengthValidator(int minLength)
        {
            if (minLength <= 0) throw new ArgumentOutOfRangeException(nameof(minLength));
            MinLength = minLength;
        }

        public override OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            return value is null || !(value is string sValue) || sValue.Length >= MinLength
                ? Success(value)
                : Failure($"Value for field {fieldId} should have minimum {MinLength} characters");
        }
    }

    public sealed class FieldValueMaxLengthValidator : AbstractFieldValueValidator
    {
        public int MaxLength { get; }

        public FieldValueMaxLengthValidator(int maxLength)
        {
            if (maxLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            MaxLength = maxLength;
        }

        public override OneOf<object?, Error<string>> Validate(FieldId fieldId, object? value)
        {
            return value is null || !(value is string sValue) || sValue.Length <= MaxLength
                ? Success(value)
                : Failure($"Value for field {fieldId} should have maximum {MaxLength} characters");
        }
    }
}