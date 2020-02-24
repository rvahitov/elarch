#nullable enable
using System;
using Akkatecture.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public sealed class DocumentTypeName : SingleValueObject<string>
    {
        public DocumentTypeName([NotNull] string value) : base(value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }
    }
}