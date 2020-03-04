using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.Entities;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentItemTypeAdded : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {
        public DocumentItemTypeAdded(DocumentItemType documentItemType)
        {
            DocumentItemType = documentItemType ?? throw new ArgumentNullException(nameof(documentItemType));
        }

        public DocumentItemType DocumentItemType { get; }
    }
}