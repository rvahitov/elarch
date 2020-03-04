using System;
using Akkatecture.ValueObjects;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class DocumentItemTypeName : SingleValueObject<string>
    {
        public DocumentItemTypeName([NotNull] string value) : base(value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }
    }
}