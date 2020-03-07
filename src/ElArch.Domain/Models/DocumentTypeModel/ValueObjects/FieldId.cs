using System;
using Akkatecture.ValueObjects;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class FieldId : IEquatable<FieldId>, ISingleValueObject
    {
        public FieldId([NotNull] string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));
            Value = value;
        }

        [NotNull] public string Value { get; }
        public override string ToString() => Value;

        public bool Equals(FieldId? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        [NotNull]
        public object GetValue() => Value;

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is FieldId other && Equals(other);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

        public static bool operator ==(FieldId? left, FieldId? right) => Equals(left, right);

        public static bool operator !=(FieldId? left, FieldId? right) => !Equals(left, right);
    }
}