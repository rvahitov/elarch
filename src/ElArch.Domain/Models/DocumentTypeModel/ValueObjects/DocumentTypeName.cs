#nullable enable
using System;
using Akkatecture.ValueObjects;
using Newtonsoft.Json;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class DocumentTypeName : SingleValueObject<string>
    {
        public DocumentTypeName(string value) : base(value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
        }
    }
}