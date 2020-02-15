#nullable enable
using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentTypeFieldRemoved : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {
        public DocumentTypeFieldRemoved(Field field)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public Field Field { get; }
    }
}