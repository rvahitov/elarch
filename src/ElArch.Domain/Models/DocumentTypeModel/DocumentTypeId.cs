#nullable enable
using Akkatecture.Core;
using Akkatecture.ValueObjects;
using MassTransit;
using Newtonsoft.Json;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class DocumentTypeId : Identity<DocumentTypeId>
    {
        public DocumentTypeId(string value) : base(value)
        {
        }

        public static DocumentTypeId CreateNew() => With(NewId.NextGuid());
    }
}