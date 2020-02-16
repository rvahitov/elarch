#nullable enable
using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentTypeFieldAdded : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {
        public DocumentTypeFieldAdded(IField field)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public IField Field { get; }
    }
}