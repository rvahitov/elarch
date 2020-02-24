#nullable enable
using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentTypeCreated : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {
        public DocumentTypeCreated(DocumentTypeName documentTypeName)
        {
            DocumentTypeName = documentTypeName ?? throw new ArgumentNullException(nameof(documentTypeName));
        }

        public DocumentTypeName DocumentTypeName { get; }
    }
}