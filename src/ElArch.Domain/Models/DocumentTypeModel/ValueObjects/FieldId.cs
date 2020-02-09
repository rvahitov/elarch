using System;
using Akkatecture.ValueObjects;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class FieldId : SingleValueObject<string>
    {
        public FieldId([NotNull] string value) : base(value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));
            Value = value;
        }

        public string Value { get; }
    }
}