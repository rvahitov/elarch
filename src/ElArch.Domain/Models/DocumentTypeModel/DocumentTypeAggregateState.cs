using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    [UsedImplicitly]
    public sealed class DocumentTypeAggregateState : AggregateState<DocumentTypeAggregate, DocumentTypeId>,
        IEmit<DocumentTypeCreated>, IEmit<DocumentTypeNameChanged>
    {
        public DocumentTypeName? DocumentTypeName { get; private set; }

        public void Apply(DocumentTypeCreated aggregateEvent)
        {
            DocumentTypeName = aggregateEvent.DocumentTypeName;
        }

        public void Apply(DocumentTypeNameChanged aggregateEvent)
        {
            DocumentTypeName = aggregateEvent.DocumentTypeName;
        }
    }
}