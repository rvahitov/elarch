#nullable enable
using System;
using Akkatecture.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public sealed class DocumentTypeName : SingleValueObject<string>
    {
        public DocumentTypeName(string value) : base(value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }
    }
}