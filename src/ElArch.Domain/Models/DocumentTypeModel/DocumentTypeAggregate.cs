using Akkatecture.Aggregates;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    [UsedImplicitly]
    public sealed class DocumentTypeAggregate : AggregateRoot<DocumentTypeAggregate, DocumentTypeId, DocumentTypeAggregateState>
    {
        public DocumentTypeAggregate(DocumentTypeId id) : base(id)
        {
        }
    }
}