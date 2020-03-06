using System.Collections.Immutable;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    [UsedImplicitly]
    public sealed class DocumentTypeAggregateState : AggregateState<DocumentTypeAggregate, DocumentTypeId>,
        IEmit<DocumentTypeCreated>, IEmit<DocumentTypeNameChanged>,
        IEmit<DocumentTypeFieldAdded>, IEmit<DocumentTypeFieldRemoved>
    {
        public DocumentTypeName? DocumentTypeName { get; private set; }

        public ImmutableDictionary<FieldId, IField> Fields { get; private set; } = ImmutableDictionary<FieldId, IField>.Empty;

        public void Apply(DocumentTypeCreated aggregateEvent)
        {
            DocumentTypeName = aggregateEvent.DocumentTypeName;
        }

        public void Apply(DocumentTypeFieldAdded aggregateEvent)
        {
            Fields = Fields.Add(aggregateEvent.Field.FieldId, aggregateEvent.Field);
        }

        public void Apply(DocumentTypeFieldRemoved aggregateEvent)
        {
            Fields = Fields.Remove(aggregateEvent.Field.FieldId);
        }

        public void Apply(DocumentTypeNameChanged aggregateEvent)
        {
            DocumentTypeName = aggregateEvent.DocumentTypeName;
        }
    }
}